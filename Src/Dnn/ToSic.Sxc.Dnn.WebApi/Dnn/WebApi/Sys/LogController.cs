﻿using DotNetNuke.Security;
using DotNetNuke.Web.Api;
using System.Web.Http;
using ToSic.Eav.WebApi.Sys;
using ToSic.Sxc.Dnn.Run;
using ToSic.Sxc.Dnn.WebApi.Logging;

namespace ToSic.Sxc.Dnn.WebApi.Sys
{
    /// <summary>
    /// This one supplies portal-wide (or cross-portal) settings / configuration
    /// </summary>
	[SupportedModules("2sxc,2sxc-app")]
    [DnnLogExceptions]
    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Admin)]
    [ValidateAntiForgeryToken]
    public class LogController : DnnApiControllerWithFixes<LogControllerReal>
    {
        public LogController() : base(LogControllerReal.LogSuffix) { }

        #region Enable extended logging

        /// <summary>
        /// Used to be GET System/ExtendedLogging
        /// </summary>
        /// <param name="duration"></param>
        /// <returns></returns>
        [HttpGet]
        public string EnableDebug(int duration = 1) => Real.EnableDebug(DnnLogging.ActivateForDuration, duration);

        #endregion
    }
}