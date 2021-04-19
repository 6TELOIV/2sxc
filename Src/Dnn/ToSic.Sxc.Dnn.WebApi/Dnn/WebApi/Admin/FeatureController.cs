﻿using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using DotNetNuke.Application;
using DotNetNuke.Security;
using DotNetNuke.Web.Api;
using ToSic.Eav.Configuration;
using ToSic.Eav.WebApi.PublicApi;
using ToSic.Sxc.WebApi.Features;

namespace ToSic.Sxc.Dnn.WebApi.Admin
{
    /// <summary>
    /// Provide information about activated features which will be managed externally. 
    /// </summary>
    /// <remarks>
    /// Added in 2sxc 10
    /// </remarks>
    [SupportedModules("2sxc,2sxc-app")]
    [ValidateAntiForgeryToken]
    public class FeatureController : DnnApiControllerWithFixes, IFeatureController
    {
        /// <summary>
        /// Name of this class in the insights logs.
        /// </summary>
        protected override string HistoryLogName => "Api.Feats";

        /// <summary>
        /// Used to be GET a list of Features
        /// </summary>
        /// <remarks>
        /// Added in 2sxc 10
        /// </remarks>
        [HttpGet]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Admin)]
        public IEnumerable<Feature> List(bool reload = false) => 
            GetService<FeaturesBackend>().Init(Log).GetAll(reload);

        /// <summary>
        /// Used to be GET the URL which will open the UI to enable/disable features.
        /// Note that the implementation for Oqtane will have to be different. 
        /// </summary>
        /// <remarks>
        /// Added in 2sxc 10
        /// </remarks>
        [HttpGet]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Host)]
        public string RemoteManageUrl() =>
            "//gettingstarted.2sxc.org/router.aspx?"
            + $"DnnVersion={DotNetNukeContext.Current.Application.Version.ToString(4)}"
            + $"&2SexyContentVersion={Settings.ModuleVersion}"
            + $"&fp={HttpUtility.UrlEncode(Fingerprint.System)}"
            + $"&DnnGuid={DotNetNuke.Entities.Host.Host.GUID}"
            + $"&ModuleId={Request.FindModuleInfo().ModuleID}" // needed for callback later on
            + "&destination=features";

        /// <summary>
        /// Used to be POST updated features JSON configuration.
        /// These must always be signed by the 2sxc.org private key.
        /// </summary>
        /// <remarks>
        /// Added in 2sxc 10
        /// </remarks>
        [HttpPost]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Host)]
        public bool Save([FromBody] FeaturesDto featuresManagementResponse) => 
            GetService<FeaturesBackend>().Init(Log).SaveFeatures(featuresManagementResponse);
    }
}