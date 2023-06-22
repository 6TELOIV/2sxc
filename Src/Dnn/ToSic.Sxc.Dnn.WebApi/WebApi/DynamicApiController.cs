﻿using System;
using System.IO;
using System.Web.Http.Controllers;
using ToSic.Eav.Generics;
using ToSic.Eav.Plumbing;
using ToSic.Lib.Documentation;
using ToSic.Lib.Logging;
using ToSic.Eav.WebApi;
using ToSic.Lib.DI;
using ToSic.Lib.Helpers;
using ToSic.Lib.Services;
using ToSic.Sxc.Apps;
using ToSic.Sxc.Code;
using ToSic.Sxc.Dnn;
using ToSic.Sxc.Dnn.Code;
using ToSic.Sxc.Dnn.Run;
using ToSic.Sxc.Dnn.WebApi.Logging;
using ToSic.Sxc.LookUp;
using ToSic.Sxc.WebApi.Adam;
using ToSic.Eav.Code.InfoSystem;

namespace ToSic.Sxc.WebApi
{
    /// <summary>
    /// This is the foundation for both the old SxcApiController and the new Dnn.ApiController.
    /// incl. the current App, DNN, Data, etc.
    /// For others, please use the SxiApiControllerBase, which doesn't have all that, and is usually then
    /// safer because it can't accidentally mix the App with a different appId in the params
    /// </summary>
    [PrivateApi("This is an internal base class used for the App ApiControllers. Make sure the implementations don't break")]
    // Note: 2022-02 2dm I'm not sure if this was ever published as the official api controller, but it may have been?
    [DnnLogExceptions]
    public abstract class DynamicApiController : SxcApiControllerBase<DummyControllerReal>, ICreateInstance, IHasDynamicCodeRoot
    {
        #region Constructor & DI

        /// <summary>
        /// Note: normally dependencies are Constructor injected.
        /// This doesn't work in DNN.
        /// But for consistency, we're building a comparable structure here.
        /// </summary>
        public class MyServices: MyServicesBase
        {
            public LazySvc<AppConfigDelegate> AppConfigDelegateLazy { get; }
            public LazySvc<Apps.App> AppOverrideLazy { get; }
            public DnnCodeRootFactory DnnCodeRootFactory { get; }
            public DnnAppFolderUtilities AppFolderUtilities { get; }

            public MyServices(
                DnnCodeRootFactory dnnCodeRootFactory,
                DnnAppFolderUtilities appFolderUtilities,
                LazySvc<Apps.App> appOverrideLazy,
                LazySvc<AppConfigDelegate> appConfigDelegateLazy)
            {
                ConnectServices(
                    DnnCodeRootFactory = dnnCodeRootFactory,
                    AppFolderUtilities = appFolderUtilities,
                    AppOverrideLazy = appOverrideLazy,
                    AppConfigDelegateLazy = appConfigDelegateLazy
                );
            }
        }

        /// <summary>
        /// Empty constructor is important for inheriting classes
        /// </summary>
        protected DynamicApiController() : base("DynApi") { }
        protected DynamicApiController(string logSuffix): base(logSuffix) { }

        private MyServices Services => _depsGetter.Get(() => GetService<MyServices>().ConnectServices(Log));
        private readonly GetOnce<MyServices> _depsGetter = new GetOnce<MyServices>();

        #endregion

        /// <summary>
        /// The name of the logger in insights.
        /// The inheriting class should provide the real name to be used.
        /// </summary>
        [Obsolete("Deprecated in v13.03 - doesn't serve a purpose any more. Will just remain to avoid breaking public uses of this property.")]
        // Note: Probably almost never used, except by 2sic. Must determine if we just remove it
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        protected virtual string HistoryLogName { get; }


        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            // Note that the CmsBlock is created by the BaseClass, if it's detectable. Otherwise it's null
            var block = GetBlockAndContext()?.LoadBlock();
            Log.A($"HasBlock: {block != null}");
            var compatibilityLevel = this is IDynamicCode16
                ? Constants.CompatibilityLevel16
                : this is ICompatibleToCode12
                    ? Constants.CompatibilityLevel12
                    : Constants.CompatibilityLevel10;
            _DynCodeRoot = Services.DnnCodeRootFactory
                .BuildDynamicCodeRoot(this)
                .InitDynCodeRoot(block, Log, compatibilityLevel);
            _AdamCode = GetService<AdamCode>();
            _AdamCode.ConnectToRoot(_DynCodeRoot, Log);

            // In case SxcBlock was null, there is no instance, but we may still need the app
            if (_DynCodeRoot.App == null)
            {
                Log.A("DynCode.App is null");
                TryToAttachAppFromUrlParams();
            }

            var reqProperties = controllerContext.Request.Properties;

            // must run this after creating AppAndDataHelpers
            reqProperties.Add(DnnConstants.DnnContextKey, Dnn);

            if (reqProperties.TryGetTyped(CodeCompiler.SharedCodeRootPathKeyInCache, out string path))
                CreateInstancePath = path;

            // 16.02 - try to log more details about the current API call
            var currentPath = reqProperties.TryGetTyped(CodeCompiler.SharedCodeRootFullPathKeyInCache, out string p2) ? p2.AfterLast("/") : null;
            WebApiLogging?.AddLogSpecs(block, _DynCodeRoot.App, currentPath, GetService<CodeInfosInScope>());

        }

        /// <summary>
        /// Get a service of a specified type. 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        /// <remarks>
        /// This will override the base functionality to ensure that any services created will be able to get the CodeContext.
        /// </remarks>
        public override TService GetService<TService>() => _DynCodeRoot != null 
                ? _DynCodeRoot.GetService<TService>()
                : base.GetService<TService>();          // If the CodeRoot isn't ready, use standard functionality


        [PrivateApi]
        public IDynamicCodeRoot _DynCodeRoot { get; private set; }

        // ReSharper disable once InconsistentNaming
        [PrivateApi]
#pragma warning disable IDE1006 // Naming Styles
        public AdamCode _AdamCode { get; private set; }
#pragma warning restore IDE1006 // Naming Styles

        public IDnnContext Dnn => (_DynCodeRoot as IDnnDynamicCode)?.Dnn;

        private IApp TryToAttachAppFromUrlParams() 
        {
            var l = Log.Fn<IApp>();
            try
            {
                var routeAppPath = Services.AppFolderUtilities.GetAppFolder(Request, false);
                var appState = SharedContextResolver.SetAppOrNull(routeAppPath)?.AppState;

                if (appState != default)
                {
                    var siteCtx = SharedContextResolver.Site();
                    // Look up if page publishing is enabled - if module context is not available, always false
                    l.A($"AppId: {appState.AppId}");
                    var app = Services.AppOverrideLazy.Value
                        .PreInit(siteCtx.Site)
                        .Init(appState, Services.AppConfigDelegateLazy.Value.Build());
                    _DynCodeRoot.AttachApp(app);
                    return l.Return(app, $"found #{app.AppId}");
                }
            }
            catch
            {
                l.ReturnNull("error, ignore");
                /* ignore */
            }

            return l.ReturnNull("no app detected");
        }


        #region Adam - Shared Code Across the APIs
        /// <summary>
        /// See docs of official interface <see cref="IDynamicWebApi"/>
        /// </summary>
        public Sxc.Adam.IFile SaveInAdam(string noParamOrder = Eav.Parameters.Protector, 
            Stream stream = null, 
            string fileName = null, 
            string contentType = null, 
            Guid? guid = null, 
            string field = null,
            string subFolder = "") =>
            _AdamCode.SaveInAdam(
                stream: stream,
                fileName: fileName,
                contentType: contentType,
                guid: guid,
                field: field,
                subFolder: subFolder);

        #endregion

        public string CreateInstancePath { get; set; }

        public dynamic CreateInstance(string virtualPath, 
            string noParamOrder = Eav.Parameters.Protector,
            string name = null, 
            string relativePath = null, 
            bool throwOnError = true) =>
            _DynCodeRoot.CreateInstance(virtualPath, noParamOrder, name,
                CreateInstancePath, throwOnError);
    }
}
