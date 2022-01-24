﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oqtane.Shared;
using System.Collections.Generic;
using ToSic.Eav.Configuration;
using ToSic.Eav.WebApi.PublicApi;
using ToSic.Sxc.Oqt.Server.Controllers;
using ToSic.Sxc.Oqt.Server.Integration;
using ToSic.Sxc.Oqt.Shared;
using ToSic.Sxc.Run;
using ToSic.Sxc.WebApi.Features;

namespace ToSic.Sxc.Oqt.Server.WebApi.Admin
{
    [ValidateAntiForgeryToken]

    // Release routes
    [Route(WebApiConstants.ApiRoot + "/admin/[controller]/[action]")]
    [Route(WebApiConstants.ApiRoot2 + "/admin/[controller]/[action]")]
    [Route(WebApiConstants.ApiRoot3 + "/admin/[controller]/[action]")]

    // Beta routes
    [Route(WebApiConstants.WebApiStateRoot + "/admin/[controller]/[action]")]
    public class FeatureController : OqtStatefulControllerBase, IFeatureController
    {
        private readonly FeaturesBackend _featuresBackend;
        private readonly OqtModuleHelper _oqtModuleHelper;
        private readonly RemoteRouterLink _remoteRouterLink;
        protected override string HistoryLogName => "Api.Feats";

        public FeatureController(FeaturesBackend featuresBackend, OqtModuleHelper oqtModuleHelper, RemoteRouterLink remoteRouterLink)
        {
            _featuresBackend = featuresBackend;
            _oqtModuleHelper = oqtModuleHelper;
            _remoteRouterLink = remoteRouterLink;
        }

        /// <summary>
        /// Used to be GET System/Features
        /// </summary>
        [HttpGet]
        [Authorize(Roles = RoleNames.Admin)]
        public IEnumerable<FeatureState> List(bool reload = false) => _featuresBackend.Init(Log).GetAll(reload);

        /// <summary>
        /// Used to be GET System/ManageFeaturesUrl
        /// </summary>
        [HttpGet]
        [Authorize(Roles = RoleNames.Host)]
        public string RemoteManageUrl()
        {
            var ctx = GetContext();
            var site = ctx.Site;
            var module = ctx.Module;
            
            var link = _remoteRouterLink.LinkToRemoteRouter(RemoteDestinations.Features,
                site,
                module.Id,
                app: null,
                _oqtModuleHelper.IsContentApp(module.Id)
                );
            return link;
        }

        /// <summary>
        /// Used to be GET System/SaveFeatures
        /// </summary>
        [HttpPost]
        [Authorize(Roles = RoleNames.Host)]
        public bool Save([FromBody] FeaturesDto featuresManagementResponse) =>
            _featuresBackend.Init(Log).SaveFeatures(featuresManagementResponse);

    }
}