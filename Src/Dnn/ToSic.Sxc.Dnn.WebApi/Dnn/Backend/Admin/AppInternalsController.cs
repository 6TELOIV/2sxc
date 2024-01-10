﻿using System.Web.Http;
using DotNetNuke.Security;
using DotNetNuke.Web.Api;
using ToSic.Eav.WebApi.Admin;
using ToSic.Eav.WebApi.Dto;
using ToSic.Sxc.Dnn.WebApi.Logging;
using ToSic.Sxc.WebApi;
using RealController = ToSic.Eav.WebApi.Admin.AppInternalsControllerReal;

namespace ToSic.Sxc.Dnn.Backend.Admin;

/// <summary>
/// Proxy Class to the AppInternalsController (Web API Controller)
/// </summary>
[DnnLogExceptions]
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
public class AppInternalsController() : SxcApiControllerBase(RealController.LogSuffix), IAppInternalsController
{
    private RealController Real => SysHlp.GetService<RealController>();

    /// <inheritdoc/>
    [HttpGet]
    [ValidateAntiForgeryToken]
    [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
    public AppInternalsDto Get(int appId, int targetType, string keyType, string key) 
        => Real.Get(appId, targetType, keyType, key);
}