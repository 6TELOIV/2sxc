﻿using System;
using System.IO;
using System.Web;
using System.Web.Caching;
using DotNetNuke.Common.Extensions;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Urls;
using DotNetNuke.Framework;
using Microsoft.Extensions.DependencyInjection;
using ToSic.Eav.Apps;
using ToSic.Eav.Configuration;
using ToSic.Eav.Data;
using ToSic.Eav.Data.PropertyLookup;
using ToSic.Eav.Helpers;
using ToSic.Eav.Plumbing;
using ToSic.Eav.Run;
using ToSic.Sxc.Dnn.Web;
using ToSic.Sxc.Web;

namespace ToSic.Sxc.Dnn.dist
{
    public class CachedPageBase : CDefault // HACK: inherits dnn default.aspx to preserve correct language cookie
    {
        protected string PageOutputCached(string virtualPath)
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
            var portalIdString = Request.QueryString[DnnJsApi.PortalIdParamName];
            var portalId = portalIdString.HasValue() ? Convert.ToInt32(portalIdString) : -1;
            var addOn = $"&{DnnJsApi.PortalIdParamName}={portalId}";

            // pageId should be provided in query string
            var pageIdString = Request.QueryString[HtmlDialog.PageIdInUrl];
            var pageId = pageIdString.HasValue() ? Convert.ToInt32(pageIdString) : -1;
            var siteRoot = GetSiteRoot(pageId, portalId);

            var content = DnnJsApi.GetJsApiJson(pageId, siteRoot);

            // EXPERIMENTAL 2DM
            var sp = HttpContext.Current.GetScope().ServiceProvider;
            var zoneMap = sp.GetService<IZoneMapper>();
            var zoneId = zoneMap.GetZoneId(portalId);
            var appsCache = sp.GetService<IAppStates>();
            var defId = appsCache.IdentityOfPrimary(zoneId);
            var appState = appsCache.Get(defId);
            var stackMaker = sp.GetService<AppSettingsStack>();

            var settingsSources = stackMaker.Init(appState).GetStack(ConfigurationConstants.Settings);
            var stack = new PropertyStack().Init(AppConstants.RootNameSettings, settingsSources);

            var req = new PropReqSpecs("SiteSetup.AutoInstallApps");
            var x = stack.InternalGetPath(req, null);

            // var customHeaders = appsCache.ToString() + "-" + stack.ToString() + "-" + x.Result?.ToString();
            var customHeaders = ""; 
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
        private static string GetSiteRoot(int pageId, int portalId)
        {
            // this is fallback
            if (pageId == -1) return ServicesFramework.GetServiceFrameworkRoot();
            if (portalId == -1) portalId = PortalController.GetPortalDictionary()[pageId];

            //var cultureCode = LocaleController.Instance.GetCurrentLocale(portalId).Code;
            var cultureCode = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
            var primaryPortalAlias = PortalAliasController.Instance.GetPortalAliasesByPortalId(portalId)
                .GetAliasByPortalIdAndSettings(portalId, result: null, cultureCode, settings: new FriendlyUrlSettings(portalId));
            var siteRoot = primaryPortalAlias != null ? CleanLeadingPartSiteRoot(primaryPortalAlias.HTTPAlias) : ServicesFramework.GetServiceFrameworkRoot();
            if (string.IsNullOrEmpty(siteRoot)) siteRoot = "/";
            return siteRoot;
        }

        private static string CleanLeadingPartSiteRoot(string path)
        {
            var index = path.IndexOf('/');
            return index <= 0 ? "/" : path.Substring(index).SuffixSlash();
        }
    }
}