﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Oqtane.Shared;
using System.Reflection;
using ToSic.Eav.Internal.Environment;
using ToSic.Eav.WebApi.ApiExplorer;
using ToSic.Eav.WebApi.Routing;
using ToSic.Lib.DI;
using ToSic.Lib.Logging;
using ToSic.Sxc.Apps;
using ToSic.Sxc.Code.Internal.HotBuild;
using ToSic.Sxc.Oqt.Server.Code.Internal;
using ToSic.Sxc.Oqt.Server.Controllers;
using ToSic.Sxc.Oqt.Server.Controllers.AppApi;
using ToSic.Sxc.Oqt.Server.Plumbing;
using ToSic.Sxc.Oqt.Server.Run;
using ToSic.Sxc.Polymorphism.Internal;
using RealController = ToSic.Eav.WebApi.ApiExplorer.ApiExplorerControllerReal;

namespace ToSic.Sxc.Oqt.Server.WebApi.Admin;

// Release routes
[Route(OqtWebApiConstants.ApiRootWithNoLang + $"/{AreaRoutes.Admin}")]
[Route(OqtWebApiConstants.ApiRootPathOrLang + $"/{AreaRoutes.Admin}")]
[Route(OqtWebApiConstants.ApiRootPathNdLang + $"/{AreaRoutes.Admin}")]

[ValidateAntiForgeryToken]
//[DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Admin)]
[Authorize(Roles = RoleNames.Admin)]

[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
public class ApiExplorerController() : OqtStatefulControllerBase(RealController.LogSuffix), IApiExplorerController
{
    private RealController Real => GetService<RealController>();
    private Generator<Compiler> Compiler => GetService<Generator<Compiler>>();

    [HttpGet]
    public IActionResult Inspect(string path)
    {
        // Make sure the Scoped ResponseMaker has this controller context
        CtxHlp.SetupResponseMaker();
        return Real.Inspect(path, GetCompiledAssembly);
    }

    private Assembly GetCompiledAssembly(string path)
    {
        // get path from root
        var siteStateInitializer = GetService<SiteStateInitializer>();
        var siteId = siteStateInitializer.InitializedState.Alias.SiteId;
        var appFolder = GetService<AppFolder>().GetAppFolder();
        var pathFromRoot = OqtServerPaths.GetAppApiPath(siteId, appFolder, path);

        var spec = new HotBuildSpec(CtxHlp.BlockOptional?.AppId ?? Eav.Constants.AppIdEmpty); 
        
        // Figure out the current edition
        var block = CtxHlp.BlockOptional;
        if (block != null)
            spec = new HotBuildSpec(spec.AppId,
                edition: PolymorphConfigReader.UseViewEditionLazyGetEdition(block.View,() => GetService<PolymorphConfigReader>().Init(block.Context.AppState.List)));

        var appCodeLoader = GetService<LazySvc<AppCodeLoader>>();
        Log.A($"Controller path from root: {pathFromRoot}");

        // get full path
        var oqtServerPaths = GetService<IServerPaths>();
        var apiFile = oqtServerPaths.FullContentPath(pathFromRoot);

        if (!System.IO.File.Exists(apiFile))
            throw new($"Error: can't find controller file: {pathFromRoot}");

        // get dll name
        var controllerFolder = pathFromRoot.Substring(0, pathFromRoot.LastIndexOf(@"\"));
        var dllName = AppApiDynamicRouteValueTransformer.GetDllName(controllerFolder, apiFile);

        return Compiler.New().Compile(apiFile, dllName, spec).Assembly;
    }
}