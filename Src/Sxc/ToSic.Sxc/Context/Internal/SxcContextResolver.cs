﻿using ToSic.Eav.Apps;
using ToSic.Eav.Context;
using ToSic.Eav.Context.Internal;
using ToSic.Lib.DI;
using ToSic.Lib.Helpers;
using ToSic.Sxc.Blocks.Internal;
using ToSic.Sxc.Services;

namespace ToSic.Sxc.Context.Internal;

[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
internal partial class SxcContextResolver(
    LazySvc<AppIdResolver> appIdResolverLazy,
    Generator<IContextOfSite> contextOfSite,
    Generator<IContextOfApp> contextOfApp,
    Lazy<IFeaturesService> featuresService)
    : ContextResolver(contextOfSite, contextOfApp, "Sxc.CtxRes", connect: [appIdResolverLazy]), ISxcContextResolver
{

    /// <summary>
    /// Get the best possible context which can give us insights about the user permissions.
    ///
    /// TODO: WIP - requires that if an app is to be used, it was accessed before - not yet perfect...
    /// </summary>
    /// <returns></returns>
    public IContextOfUserPermissions UserPermissions() => _ctxUserPerm.Get(() => BlockContextOrNull() ?? LatestAppContext ?? Site());
    private readonly GetOnce<IContextOfUserPermissions> _ctxUserPerm = new();

    public IContextOfApp SetAppOrNull(string nameOrPath)
    {
        if (string.IsNullOrWhiteSpace(nameOrPath)) return null;
        var zoneId = Site().Site.ZoneId;
        var id = appIdResolverLazy.Value.GetAppIdFromPath(zoneId, nameOrPath, false);
        return id <= Eav.Constants.AppIdEmpty ? null : SetApp(new AppIdentity(zoneId, id));
    }

    #region Blocks

    public void AttachBlock(BlockWithContextProvider blockWithContextProvider)
    {
        _blcCtx = blockWithContextProvider;
        _block.Reset();
        _blockContext.Reset();
    }
    private BlockWithContextProvider _blcCtx;

    public IBlock BlockOrNull() => _block.Get(() => _blcCtx?.LoadBlock());
    private readonly GetOnce<IBlock> _block = new();

    public IBlock BlockRequired() => BlockOrNull() ?? throw new("Block required but missing. It was not attached");

    public IContextOfBlock BlockContextRequired() => BlockContextOrNull() ?? throw new("Block context required but not known. It was not attached.");

    public IContextOfBlock BlockContextOrNull() => _blockContext.Get(() => _blcCtx?.ContextOfBlock);
    private readonly GetOnce<IContextOfBlock> _blockContext = new();


    #endregion
}