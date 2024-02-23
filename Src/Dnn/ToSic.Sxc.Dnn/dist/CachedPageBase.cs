﻿using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using DotNetNuke.Common.Extensions;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Framework;
using Microsoft.Extensions.DependencyInjection;
using ToSic.Eav.Helpers;
using ToSic.Eav.Integration.Environment;
using ToSic.Eav.Plumbing;
using ToSic.Lib.DI;
using ToSic.Lib.Helpers;
using ToSic.Sxc.Dnn.Web;
using ToSic.Sxc.Web.Internal.EditUi;
using ToSic.Sxc.Web.Internal.JsContext;
using static System.StringComparison;

namespace ToSic.Sxc.Dnn.dist;

public class CachedPageBase : CDefault // HACK: inherits dnn default.aspx to preserve correct language cookie
{
    private const int UnknownSiteId = -1;
    private const int UnknownPageId = -1;

    #region GetService and Service Provider

    /// <summary>
    /// Get the service provider only once - ideally in Dnn9.4 we will get it from Dnn
    /// If we would get it multiple times, there are edge cases where it could be different each time! #2614
    /// </summary>
    private IServiceProvider ServiceProvider => _serviceProvider.Get(Log, DnnStaticDi.CreateModuleScopedServiceProvider);
    private readonly GetOnce<IServiceProvider> _serviceProvider = new();
    private TService GetService<TService>() => ServiceProvider.Build<TService>(Log);

    #endregion

    #region Logging

    private ILog Log { get; } = new Log("Sxc.Dnn.CachedPageBase");
    private IEnvironmentLogger EnvLogger => _envLogger.Get(Log, GetService<IEnvironmentLogger>);
    private readonly GetOnce<IEnvironmentLogger> _envLogger = new();
    #endregion

    protected string PageOutputCached(string virtualPath, EditUiResourceSettings settings)
    {
        var key = CacheKey(virtualPath);
        if (!(Cache.Get(key) is string html))
        {
            var path = GetPath(virtualPath);
            html = File.ReadAllText(path);
            html = HtmlDialog.CleanImport(html);
            Cache.Insert(key, html, new CacheDependency(path));
        }

        // portalId should be provided in query string (because of DNN special handling of aspx pages in DesktopModules)
        var portalIdString = Request.QueryString[DnnJsApiService.PortalIdParamName];
        var siteId = portalIdString.HasValue() ? Convert.ToInt32(portalIdString) : UnknownSiteId;
        var addOn = $"&{DnnJsApiService.PortalIdParamName}={siteId}";

        // pageId should be provided in query string
        var pageIdString = Request.QueryString[HtmlDialog.PageIdInUrl];
        var pageId = pageIdString.HasValue() ? Convert.ToInt32(pageIdString) : UnknownPageId;
        var siteRoot = GetSiteRoot(pageId, siteId);

        var sp = HttpContext.Current.GetScope().ServiceProvider;
        var editUiResources = sp.GetService<EditUiResources>();
        var assets = editUiResources.GetResources(true, siteId, settings);

        var dnnJsApi = sp.GetService<IJsApiService>();
        var content = dnnJsApi.GetJsApiJson(pageId, siteRoot);

        var customHeaders = assets.HtmlHead;
        return HtmlDialog.UpdatePlaceholders(html, content, pageId, addOn, customHeaders, "");
    }

    private static string CacheKey(string virtualPath) => $"2sxc-edit-ui-page-{virtualPath}";
        
    internal string GetPath(string virtualPath)
    {
        var path = Server.MapPath(virtualPath);
        if (!File.Exists(path)) throw new Exception("File not found: " + path);
        return path;
    }

    /// <summary>
    /// portalId and pageId context is lost on DesktopModules/ToSIC_SexyContent/dist/...aspx
    /// and DNN Framework can not resolve site root, so we need to handle it by ourselves
    /// </summary>
    /// <param name="pageId"></param>
    /// <param name="portalId"></param>
    /// <returns></returns>
    private string GetSiteRoot(int pageId, int portalId)
    {
        try
        {
            // this is fallback
            if (pageId == UnknownPageId) return ServicesFramework.GetServiceFrameworkRoot();
            if (portalId == UnknownSiteId) portalId = PortalController.GetPortalDictionary()[pageId];

            //var cultureCode = LocaleController.Instance.GetCurrentLocale(portalId).Code;
            var cultureCode = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();

            // Get all aliases for the portal
            var aliases = PortalAliasController.Instance
                .GetPortalAliasesByPortalId(portalId)
                .ToList();

            // Figure out the correct alias based on the current URL and culture
            // Should also fall back to correct primary if the current one is not found
            var currentUrl = HttpContext.Current.Request.Url.ToString();
            var primaryPortalAlias = aliases
                                         .Where(
                                             a => currentUrl.IndexOf(a.HTTPAlias, OrdinalIgnoreCase) >= 0
                                         )
                                         .GetAliasByPortalIdAndSettings(
                                             portalId,
                                             result: null,
                                             cultureCode,
                                             settings: new(portalId)
                                         )
                                     // TODO: @STV THIS LOOKS WRONG - see task
                                     // IT SHOULD probably fallback to the first primary first
                                     // And only if this doesn't exist for random reasons, fallback to the FirstOrDefault
                                     ?? aliases.FirstOrDefault(); // fallback to first alias

            var siteRoot = primaryPortalAlias != null
                ? CleanLeadingPartSiteRoot(primaryPortalAlias.HTTPAlias)
                : ServicesFramework.GetServiceFrameworkRoot();
            if (string.IsNullOrEmpty(siteRoot)) siteRoot = "/";
            return siteRoot;
        }
        catch (Exception ex)
        {
            EnvLogger.LogException(ex);
            // if all breaks, falling back to a default value
            return ServicesFramework.GetServiceFrameworkRoot();
        }
    }

    private static string CleanLeadingPartSiteRoot(string path)
    {
        var index = path.IndexOf('/');
        return index <= 0 ? "/" : path.Substring(index).SuffixSlash();
    }
}