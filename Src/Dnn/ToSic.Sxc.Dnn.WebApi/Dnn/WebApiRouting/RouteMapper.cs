﻿using System;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using DotNetNuke.Web.Api;
using ToSic.Sxc.Dnn.Providers;
using ToSic.Sxc.Dnn.WebApi;
using ToSic.Sxc.Dnn.WebApi.Admin;
using ToSic.Sxc.Dnn.WebApi.App;
using ToSic.Sxc.Dnn.WebApi.Cms;
using ToSic.Sxc.Dnn.WebApi.Sys;

namespace ToSic.Sxc.Dnn.WebApiRouting
{
    // ReSharper disable once UnusedMember.Global
    public class RouteMapper : IServiceRouteMapper
    {
        // DNN Module Name used in the route
        const string Mod2Sxc = "2sxc";

        // Route Concept
        // starting with eav means it's a rather low-level admin function, always needs an AppId
        // eav
        // eav-???
        // starting with app means that it's a app-specific action, more for the JS developers working with content
        // app-content  will do basic content-actions like get one, edit, update, delete
        // app-query    will try to request a query
        // app-api      will call custom c# web-apis of a specific app

        static readonly string[] StdNsWebApi = {typeof(AppContentController).Namespace /* "ToSic.Sxc.WebApi.App" */};
        static readonly string[] AdamNamespace = {typeof(AdamController).Namespace};
        private IMapRoute _mapRouteManager;
        private static readonly object appContentDefs = new {controller = ControllerNames.AppContent, id = RouteParameter.Optional };


    public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            _mapRouteManager = mapRouteManager;

            // old API routes before 08.10
            var idNullOrNumber = RegisterOldRoutesBefore0810();

            #region new API routes after 08.10

            // ADAM routes
            AddWD("adam-auto",              Roots.ContentAuto + "/" + TokenSet.TypeGuidField, ControllerNames.Adam, AdamNamespace);
            AddWD("adam2-auto",             Roots.ContentAuto + "/" + TokenSet.TypeGuidFieldAction, ControllerNames.Adam, AdamNamespace);

            // App Content routes - for GET/DELETE/PUT entities using REST
            // 1. Type and null or int-id
            // 2. Type and guid-id
            foreach (var part in Roots.Content)
            {
                AddWC("2sxc-" + part.Name,      $"{part.Path}/{TokenSet.TypeId}", appContentDefs, idNullOrNumber, StdNsWebApi);
                AddWD("2sxc-guid-" + part.Name, $"{part.Path}/{TokenSet.TypeGuid}",  ControllerNames.AppContent, StdNsWebApi);
            }

            // App-API routes - for the custom code API calls of an app
            foreach (var part in Roots.AppAutoNamedInclEditions)
                AddNs("app-api" + part.Name, part.Path + "/" + RouteParts.RouteApiControllerAction, StdNsWebApi); // new, v08.10+


            // App-Query routes - to access designed queries
            // new routes, v08.10+
            foreach (var part in Roots.QueryRoots)
            {
                AddWD("2sxc-auto-" + part.Name,        $"{part.Path}/{Token.Name}", ControllerNames.AppQuery, StdNsWebApi);
                AddWD("2sxc-auto-slash" + part.Name,   $"{part.Path}/{Token.Name}/", ControllerNames.AppQuery, StdNsWebApi);
                AddWD("2sxc-auto-stream" + part.Name,  $"{part.Path}/{Token.Name}/{Token.Stream}", ControllerNames.AppQuery, StdNsWebApi);
            }
            #endregion


            #region New routes in 2sxc 11.06+ which should replace most previous internal routes

            AddTy("2sxc-sys",     Root.Sys + "/" + TokenSet.ConAct,           typeof(InsightsController));
            AddTy("2sxc-cms",     Root.Cms + "/" + TokenSet.ConAct,           typeof(BlockController));
            AddTy("2sic-admin",   Root.Admin + "/" + TokenSet.ConAct,         typeof(MetadataController));

            #endregion

            // DNN: System calls to dnn - this is just for module delete
            AddTy("dnn", "dnn/" + TokenSet.ConAct, typeof(ModuleController));


            // Add custom service locator into the chain of service-locators
            // this is needed to enable custom API controller lookup for the app-api
            var config = GlobalConfiguration.Configuration;
            var previousSelector = config.Services.GetService(typeof(IHttpControllerSelector)) as IHttpControllerSelector;
            config.Services.Replace(typeof(IHttpControllerSelector), new AppApiControllerSelector(config) { PreviousSelector = previousSelector });

            // Attempt to add another Module Resolver to the list which will work with the header PageId instead of TabId
            GlobalConfiguration.Configuration.AddTabAndModuleInfoProvider(new ModifiedTabAndModuleInfoProvider());
        }

        private object RegisterOldRoutesBefore0810()
        {
            // ADAM routes
            var oldContentRoot = "app-content";
            AddWD("adam-old-81", oldContentRoot + "/" + TokenSet.TypeGuidField, ControllerNames.Adam, AdamNamespace);
            AddWD("adam", oldContentRoot + "/" + TokenSet.TypeGuidFieldAction, ControllerNames.Adam, AdamNamespace);

            // App Content routes - for GET/DELETE/PUT entities using REST
            // 1. Type and null or int-id
            // 2. Type and guid-id
            var idNullOrNumber = new {id = @"^\d*$"}; // Only matches if "id" is null, or built only with digits.
            AddWC("app-content", $"{oldContentRoot}/{TokenSet.TypeId}", appContentDefs, idNullOrNumber, StdNsWebApi);
            AddWD("app-content-guid", $"{oldContentRoot}/{TokenSet.TypeGuid}", ControllerNames.AppContent, StdNsWebApi);

            // App-API routes - for the custom code API calls of an app
            // these are the old routes, before 2sxc v08.10
            AddWD(Mod2Sxc, "app-api-old-81", "app-api/" + TokenSet.ConAct, StdNsWebApi);

            // App-Query routes - to access designed queries
            // these are the old routes, before 2sxc v08.10
            const string rootQueryPre0810 = "app-query";
            AddWD("app-query-old-81", $"{rootQueryPre0810}/{Token.Name}", ControllerNames.AppQuery, StdNsWebApi);
            // Note 2020-04-09 - this had "appname" instead of "apppath" in it - probably for 2 years! only 1 app (Manor) now had trouble, so I think this is not in use elsewhere
            // 2020-11-12 will turn off for now - leave comment in till 2021-03
            //AddWD("app-query-nomod-old-81", $"{rootQueryPre0810}/{Token.AppPath}/{Token.Name}", ControllerNames.AppQuery, StdNsWebApi); // keep for backward compatibility...

            return idNullOrNumber;
        }

        #region "Add" shorthands



        /// <summary>
        /// Add WD - With Defaults
        /// </summary>
        void AddWD(string name, string url, object defaults, string[] namespaces)
        {
            defaults = StringToControllerDefaults(defaults);
            _mapRouteManager.MapHttpRoute(Mod2Sxc, name, url, defaults, namespaces);
        }

        /// <summary>
        /// Add WC - With Constraints
        /// </summary>
        void AddWC(string name, string url, object defaults, object constraints, string[] namespaces)
        {
            defaults = StringToControllerDefaults(defaults);
            _mapRouteManager.MapHttpRoute(Mod2Sxc, name, url, defaults, constraints, namespaces);
        }

        private static object StringToControllerDefaults(object defaults)
            => defaults is string controllerName ? new { controller = controllerName } : defaults;

        /// <summary>
        /// Add just with namespaces
        /// </summary>
        void AddNs(string name, string url, string[] namespaces)
            => _mapRouteManager.MapHttpRoute(Mod2Sxc, name, url, namespaces);

        /// <summary>
        /// Add with type
        /// </summary>
        void AddTy(string name, string url, Type nsType)
            => AddNs(name, url, new[] { nsType.Namespace });


        #endregion

    }
}