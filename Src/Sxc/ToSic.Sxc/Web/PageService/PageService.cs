﻿using System;
using ToSic.Lib.DI;
using ToSic.Lib.Documentation;
using ToSic.Razor.Blade;
using ToSic.Sxc.Services;
using ToSic.Sxc.Web.ContentSecurityPolicy;

namespace ToSic.Sxc.Web.PageService
{
    [PrivateApi]
    public partial class PageService: ServiceForDynamicCode, 
            // Important: Write with namespace, because it's easy to confuse with IPageService it supports
            ToSic.Sxc.Services.IPageService,
#pragma warning disable CS0618
            // Important: Write with namespace, because it's easy to confuse with IPageService it supports
            ToSic.Sxc.Web.IPageService    // Keep for compatibility with some Apps released in v12
#pragma warning restore CS0618
    {

        public PageService(
            PageServiceShared pageServiceShared,
            ILazySvc<ContentSecurityPolicyService> cspServiceLazy,
            ILazySvc<IHtmlTagService> htmlTagsLazy,
            ILazySvc<ITurnOnService> turnOn,
            ILazySvc<IModuleService> moduleService
        ) : base("2sxc.PgeSrv")
        {
            ConnectServices(_cspServiceLazy = cspServiceLazy,
                _htmlTagsLazy = htmlTagsLazy,
                _moduleService = moduleService,
                _turnOn = turnOn,
                PageServiceShared = pageServiceShared
            );
        }

        private readonly ILazySvc<ContentSecurityPolicyService> _cspServiceLazy;
        private readonly ILazySvc<IHtmlTagService> _htmlTagsLazy;
        private readonly ILazySvc<IModuleService> _moduleService;
        private readonly ILazySvc<ITurnOnService> _turnOn;
        public PageServiceShared PageServiceShared { get; }

        /// <summary>
        /// How the changes given to this object should be processed.
        /// </summary>
        [PrivateApi("not final yet, will probably change")]
        public PageChangeModes ChangeMode { get; set; } = PageChangeModes.Auto;


        public bool CspIsEnabled => _cspServiceLazy.Value.IsEnabled;

        public bool CspIsEnforced => _cspServiceLazy.Value.IsEnforced;

        public string AddCsp(string name, params string[] values)
        {
            _cspServiceLazy.Value.Add(name, values);
            return "";
        }
    }
}
