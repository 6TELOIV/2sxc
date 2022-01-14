﻿using System.Collections.Generic;
using System.Web.Http;
using DotNetNuke.Security;
using DotNetNuke.Services.Localization;
using DotNetNuke.Web.Api;
using ToSic.Sxc.Dnn.WebApi.Logging;
using ToSic.Sxc.WebApi;
using ToSic.Sxc.WebApi.Languages;
using ToSic.Sxc.WebApi.PublicApi;
using ToSic.Sxc.WebApi.Zone;

namespace ToSic.Sxc.Dnn.WebApi.Admin
{
    /// <summary>
    /// This one supplies portal-wide (or cross-portal) settings / configuration
    /// </summary>
	[SupportedModules("2sxc,2sxc-app")]
    [DnnLogExceptions]
    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Admin)]
    [ValidateAntiForgeryToken]
    public class ZoneController : SxcApiControllerBase, IZoneController
    {
        protected override string HistoryLogName => "Api.Zone";

        private LanguagesBackend LanguagesBackend() => GetService<LanguagesBackend>().Init(Log);

        /// <inheritdoc />
        [HttpGet]
        public IList<SiteLanguageDto> GetLanguages() =>
            LanguagesBackend().GetLanguages(PortalSettings.PortalId);

        /// <inheritdoc />
        [HttpGet]
        public void SwitchLanguage(string cultureCode, bool enable) =>
            LanguagesBackend().Toggle(
                PortalSettings.PortalId,
                cultureCode,
                enable,
                LocaleController.Instance.GetLocale(cultureCode).Text);

        /// <inheritdoc />
        [HttpGet]
        public SystemInfoSetDto GetSystemInfo() => GetService<ZoneBackend>().Init(Log)
            .GetSystemInfo(PortalSettings.PortalId);
    }
}