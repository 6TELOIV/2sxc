﻿using Microsoft.AspNetCore.Mvc;

namespace ToSic.Sxc.Oqt.Server.WebApi.App
{
    //[DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)] // use view, all methods must re-check permissions
    //[Authorize(Roles = RoleNames.Everyone)] commented because of http403 issue
    // TODO: 2DM please check permissions

    // Release routes
    [Route(WebApiConstants.AppRootNoLanguage + "/{appName}/assets")]
    [Route(WebApiConstants.AppRootPathOrLang + "/{appName}/assets")]
    [Route(WebApiConstants.AppRootPathNdLang + "/{appName}/assets")]

    // Beta routes
    [Route(WebApiConstants.WebApiStateRoot + "/assets/{appName}")]
    public class AppAssetsController: AppAssetsControllerBase
    {
        public override string Route => "assets";

        public AppAssetsController(AppAssetsDependencies dependencies) : base(dependencies, "Assets")
        { }

    }
}
