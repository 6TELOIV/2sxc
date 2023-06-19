﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Compilation;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.Routing;
using ToSic.Eav.Context;
using ToSic.Lib.DI;
using ToSic.Eav.Helpers;
using ToSic.Lib.Logging;
using ToSic.Eav.WebApi.Routing;
using ToSic.Sxc.Code;
using ToSic.Sxc.Code.Errors;
using ToSic.Sxc.Dnn.Context;
using ToSic.Sxc.Dnn.WebApi.Sys;

namespace ToSic.Sxc.Dnn.WebApiRouting
{
    /// <inheritdoc />
    /// <summary>
    /// This controller will check if it's responsible (based on url)
    /// ...and if yes, compile / run the app-specific api controllers
    /// ...otherwise hand processing back to next api controller up-stream
    /// </summary>
    public class AppApiControllerSelector : IHttpControllerSelector
    {
        private readonly HttpConfiguration _config;
        public IHttpControllerSelector PreviousSelector { get; set; }

        public AppApiControllerSelector(HttpConfiguration configuration)
        {
            _config = configuration;
        }



        public IDictionary<string, HttpControllerDescriptor> GetControllerMapping() => PreviousSelector.GetControllerMapping();

        private static readonly string[] AllowedRoutes = {"desktopmodules/2sxc/api/app-api/", "api/2sxc/app-api/"}; // old routes, dnn 7/8 & dnn 9


        // new in 2sxc 9.34 #1651 - added "([^/]+\/)?" to allow an optional edition parameter
        private static readonly string[] RegExRoutes =
        {
            @"desktopmodules\/2sxc\/api\/app\/[^/]+\/([^/]+\/)?api",
            @"api\/2sxc\/app\/[^/]+\/([^/]+\/)?api"
        };

        private const string ApiErrPrefix = "2sxc Api Controller Finder Error: ";
        private const string ApiErrGeneral = "Error selecting / compiling an API controller. ";
        private const string ApiErrSuffix = "Check event-log, code and inner exception. ";


        /// <summary>
        /// Verify if this request is one which should be handled by this system
        /// </summary>
        /// <param name="request"></param>
        /// <returns>true if we want to handle it</returns>
        private bool HandleRequestWithThisController(HttpRequestMessage request)
        {
            var routeData = request.GetRouteData();
            var simpleMatch = AllowedRoutes.Any(a => routeData.Route.RouteTemplate.ToLowerInvariant().Contains(a));
            if (simpleMatch)
                return true;

            var rexMatch = RegExRoutes.Any(
                a => new Regex(a, RegexOptions.None).IsMatch(routeData.Route.RouteTemplate.ToLowerInvariant()) );
            return rexMatch;

        }

        public HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            // Do this once and early, to be really sure we always use the same one
            var sp = DnnStaticDi.GetPageScopedServiceProvider();

            // Log this lookup and add to history for insights
            var log = new Log("Sxc.Http", null, request?.RequestUri?.AbsoluteUri);
            AddToInsightsHistory(sp, request?.RequestUri?.AbsoluteUri, log);

            var l = log.Fn<HttpControllerDescriptor>();

            if (!HandleRequestWithThisController(request))
                return l.Return(PreviousSelector.SelectController(request), "upstream");

            var routeData = request.GetRouteData();

            var controllerTypeName = routeData.Values[VarNames.Controller] + "Controller";

            // Now Handle the 2sxc app-api queries
            
            // Figure out the Path, or show error for that
            var appFolder = sp.Build<DnnAppFolderUtilities>(log).GetAppFolder(request, true);

            try
            {
                // new for 2sxc 9.34 #1651
                var edition = GetEdition(routeData);
                l.A($"Edition: {edition}");

                var site = (DnnSite)sp.Build<ISite>(log);

                var controllerFolder = GetControllerFolder(site, appFolder, edition, shared: false);
                l.A($"Controller Folder: {controllerFolder}");

                var controllerPath = GetControllerPath(controllerFolder, controllerTypeName);
                l.A($"Controller Path: {controllerPath}");

                // note: this may look like something you could optimize/cache the result, but that's a bad idea
                // because when the file changes, the type-object will be different, so please don't optimize :)
                if (File.Exists(HostingEnvironment.MapPath(controllerPath)))
                    return l.ReturnAsOk(HttpControllerDescriptor(request, controllerFolder, controllerPath, controllerTypeName));

                l.A("path not found, will check on shared location");

                var sharedControllerFolder = GetControllerFolder(site, appFolder, edition, shared: true);
                l.A($"Shared Controller Folder: {sharedControllerFolder}");

                var sharedControllerPath = GetControllerPath(sharedControllerFolder, controllerTypeName);
                l.A($"Shared Controller Path: {sharedControllerPath}");

                if (File.Exists(HostingEnvironment.MapPath(sharedControllerPath)))
                    return l.ReturnAsOk(HttpControllerDescriptor(request, sharedControllerFolder, sharedControllerPath, controllerTypeName));

                l.A("path not found in shared, error will be thrown in a moment");

                var msgFinal = $"2sxc Api Controller Finder: Controller {controllerTypeName} not found in app. " +
                               $"We checked the virtual path '{controllerPath}'";
                throw l.Done(DnnHttpErrors.LogAndReturnException(request, HttpStatusCode.NotFound, new Exception(), msgFinal, sp.Build<CodeErrorHelpService>()));
            }
            catch (Exception e)
            {
                const string msg = ApiErrPrefix + ApiErrGeneral + ApiErrSuffix;
                throw l.Done(DnnHttpErrors.LogAndReturnException(request, HttpStatusCode.InternalServerError, e, msg, sp.Build<CodeErrorHelpService>()));
            }
        }

        private static string GetEdition(IHttpRouteData routeData)
        {
            var edition = "";
            if (routeData.Values.ContainsKey(VarNames.Edition))
                edition = routeData.Values[VarNames.Edition].ToString();
            if (!string.IsNullOrEmpty(edition))
                edition += "/";
            return edition;
        }

        private static string GetControllerFolder(DnnSite site, string appFolder, string edition, bool shared = false) 
            => Path.Combine(shared ? site.SharedAppsRootRelative: site.AppsRootRelative, appFolder, edition + "api/")
                .ForwardSlash();

        private static string GetControllerPath(string controllerFolder, string controllerTypeName)
            => Path.Combine(controllerFolder + controllerTypeName + ".cs");

        private HttpControllerDescriptor HttpControllerDescriptor(HttpRequestMessage request, 
            string controllerFolder, string controllerPath, string controllerTypeName)
        {
            var assembly = BuildManager.GetCompiledAssembly(controllerPath);
            var type = assembly.GetType(controllerTypeName, true, true);

            // help with path resolution for compilers running inside the created controller
            request?.Properties.Add(CodeCompiler.SharedCodeRootPathKeyInCache, controllerFolder);
            request?.Properties.Add(CodeCompiler.SharedCodeRootFullPathKeyInCache, controllerPath);

            return new HttpControllerDescriptor(_config, type.Name, type);
        }

        private static void AddToInsightsHistory(IServiceProvider sp, string url, ILog log)
        {
            // 2022-12-21 ATM we seem to have an error adding this - must review later
            // TODO:
            try
            {
                var addToHistory = true;
#pragma warning disable CS0162
                if (InsightsController.InsightsLoggingEnabled)
                    addToHistory = (url?.Contains(InsightsController.InsightsUrlFragment) ?? false);
#pragma warning restore CS0162
                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (addToHistory) sp.Build<ILogStore>().Add("http-request", log);
            }
            catch { /* ignore */ }
        }
    }
}