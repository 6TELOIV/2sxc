﻿using Microsoft.AspNetCore.Http;
using Oqtane.Models;
using Oqtane.Repository;
using Oqtane.Shared;
using System;
using System.Linq;
using Oqtane.Infrastructure;
using ToSic.Lib.DI;
using ToSic.Lib.Services;
using ToSic.Sxc.Oqt.Shared;

namespace ToSic.Sxc.Oqt.Server.Plumbing;

[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
public class SiteStateInitializer: ServiceBase
{
    private readonly LazySvc<SiteState> _siteStateLazy;
    private readonly LazySvc<AliasAccessor> _aliasAccessorLazy;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly LazySvc<IAliasRepository> _aliasRepositoryLazy;

    public SiteStateInitializer(LazySvc<SiteState> siteStateLazy, LazySvc<AliasAccessor> aliasAccessorLazy, IHttpContextAccessor httpContextAccessor,
        LazySvc<IAliasRepository> aliasRepositoryLazy): base($"{OqtConstants.OqtLogPrefix}.SSInit")
    {
        ConnectServices(
            _siteStateLazy = siteStateLazy,
            _aliasAccessorLazy = aliasAccessorLazy,
            _httpContextAccessor = httpContextAccessor,
            _aliasRepositoryLazy = aliasRepositoryLazy
        );
    }

    /// <summary>
    /// Use this from inner code, which must always have an initialized state.
    /// Usually this has been ensured at the very top - when razor starts or when WebApi start
    /// </summary>
    public SiteState InitializedState
    {
        get
        {
            if (_siteStateLazy.Value?.Alias != null && _aliasAccessorLazy.Value?.Alias != null) return _siteStateLazy.Value;
            InitIfEmpty();
            return _siteStateLazy.Value;
        }
    }

    /// <summary>
    /// Will initialize the SiteState if it has not been initialized yet
    /// </summary>
    /// <returns></returns>
    internal bool InitIfEmpty(int? siteId = null)
    {
        var siteState = _siteStateLazy.Value;
        var aliasAccessor = _aliasAccessorLazy.Value; // In Oqtane 5.1 Alias is preserved in 2 places.

        // This would indicate it was called improperly, because we need the shared SiteState variable to work properly
        if (siteState == null) throw new ArgumentNullException(nameof(siteState));

        // This would indicate it was called improperly, because we need the shared AliasAccessor variable to work properly
        if (aliasAccessor == null) throw new ArgumentNullException(nameof(aliasAccessor));

        // Check if alias already set and if is for provided siteId (if provided), in which case we skip this
        if (siteState.Alias != null && aliasAccessor.Alias != null
                                    && (!siteId.HasValue || siteId.Value == siteState?.Alias?.SiteId || siteId.Value == aliasAccessor?.Alias?.SiteId)) return true;

        // For anything else we need the httpContext, otherwise skip
        var context = _httpContextAccessor?.HttpContext;
        
        // Oqtane cache on request
        var items = context?.Items;
        if ((items?.TryGetValue(Constants.HttpContextAliasKey, out var alias) ?? false) && alias != null)
        {
            siteState.Alias ??= (Alias)alias;
            return false;
        }

        // Try HACK
        if ((items?.TryGetValue("AliasFor2sxc", out var alias2Sxc) ?? false) && alias2Sxc != null)
        {
            siteState.Alias ??= (Alias)alias2Sxc;
            UpdateAliasAccessor((Alias)alias2Sxc);
            return false;
        }

        var request = context?.Request;
        if (request == null) return false;

        // Try to get alias with info for HttpRequest and eventual SiteId.
        if (siteId.HasValue || (request.Path is { HasValue: true, Value: not null } && request.Path.Value.Contains("/_blazor")))
        {
            var url = $"{request.Host}";

            var aliases = _aliasRepositoryLazy.Value.GetAliases().ToList(); // cached by Oqtane

            if (siteId.HasValue) // acceptable solution
                siteState.Alias = aliases.OrderByDescending(a => /*a.IsDefault*/  a.Name.Length) // TODO: a.IsDefault DESC after upgrade to Oqt v3.0.3+
                    //.ThenByDescending(a => a.Name.Length)
                    .ThenBy(a => a.Name)
                    .FirstOrDefault(a => a.SiteId == siteId.Value && a.Name.StartsWith(url, StringComparison.InvariantCultureIgnoreCase));
            else // fallback solution, wrong site is possible
                siteState.Alias = aliases.OrderByDescending(a => a.Name)
                    .FirstOrDefault(a => a.Name.StartsWith(url, StringComparison.InvariantCultureIgnoreCase));
        }
        else // great solution
        {
            var url = $"{request.Host}{request.Path}";

            var aliases = _aliasRepositoryLazy.Value.GetAliases().ToList(); // cached by Oqtane
            siteState.Alias = aliases.OrderByDescending(a => a.Name.Length)
                .ThenBy(a => a.Name)
                .FirstOrDefault(a => url.StartsWith(a.Name, StringComparison.InvariantCultureIgnoreCase));
        }

        UpdateAliasAccessor(siteState.Alias);

        return siteState.Alias != null;
    }

    private void UpdateAliasAccessor(Alias alias)
    {
        if (_httpContextAccessor?.HttpContext != null && alias != null)
            _httpContextAccessor.HttpContext.Items[Constants.HttpContextAliasKey] ??= alias;
    }
}