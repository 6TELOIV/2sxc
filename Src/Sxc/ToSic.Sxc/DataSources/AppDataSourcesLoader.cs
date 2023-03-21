﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using ToSic.Eav;
using ToSic.Eav.Apps;
using ToSic.Eav.Apps.Paths;
using ToSic.Eav.Caching.CachingMonitors;
using ToSic.Eav.Context;
using ToSic.Eav.DataSources;
using ToSic.Eav.DataSources.Catalog;
using ToSic.Lib.DI;
using ToSic.Lib.Logging;
using ToSic.Lib.Services;
using ToSic.Sxc.Code;

namespace ToSic.Sxc.DataSources
{
    public class AppDataSourcesLoader : ServiceBase, IAppDataSourcesLoader
    {
        private const string DataSourcesFolder = "DataSources";

        public AppDataSourcesLoader(ILogStore logStore, ISite site, IAppStates appStates, LazySvc<AppPaths> appPathsLazy, LazySvc<CodeCompiler> codeCompilerLazy) : base("Eav.AppDtaSrcLoad")
        {
            ConnectServices(
                _logStore = logStore,
                _site = site,
                _appStates = appStates,
                _appPathsLazy = appPathsLazy,
                _codeCompilerLazy = codeCompilerLazy
            );
        }
        private readonly ILogStore _logStore;
        private readonly ISite _site;
        private readonly IAppStates _appStates;
        private readonly LazySvc<AppPaths> _appPathsLazy;
        private readonly LazySvc<CodeCompiler> _codeCompilerLazy;

        /// <summary>
        /// A cache of all DataSource Types - initialized upon first access ever, then static cache.
        /// </summary>
        private static MemoryCache AppCache => MemoryCache.Default;

        private static string AppCacheKey(int appId) => $"{appId}";

        public List<DataSourceInfo> Get(int appId) => AppCache[AppCacheKey(appId)] as List<DataSourceInfo> ?? CreateAndReturnAppCache(appId);

        private List<DataSourceInfo> CreateAndReturnAppCache(int appId)
        {
            try
            {
                var expiration = new TimeSpan(1, 0, 0);
                var policy = new CacheItemPolicy { SlidingExpiration = expiration };

                var (physicalPath, virtualPath) = GetAppDataSourceFolderPaths(appId);
                if (Directory.Exists(physicalPath))
                    policy.ChangeMonitors.Add(new FolderChangeMonitor(new List<string> { physicalPath }));

                var data = (LoadAppDataSources(appId) ?? new List<Type>())
                    .Select(t => new DataSourceInfo(t, false))
                    .ToList();

                // add configuration types
                var appState = _appStates.Get(appId);
                data.ForEach(dsi =>
                {
                    dsi.VisualQuery.ConfigurationType = string.IsNullOrEmpty(dsi.VisualQuery.ConfigurationType) 
                        ? appState.GetContentType($"{dsi.TypeName}Configuration")?.NameId
                        : dsi.VisualQuery.ConfigurationType;
                });

                AppCache.Set(new CacheItem(AppCacheKey(appId), data), policy);

                return data;
            }
            catch
            {
                /* ignore for now */
            }
            return null;
        }

        private (string physicalPath, string virtualPath) GetAppDataSourceFolderPaths(int appId)
        {
            var appState = _appStates.Get(appId);
            var appPaths = _appPathsLazy.Value.Init(_site, appState);
            var physicalPath = Path.Combine(appPaths.PhysicalPath, DataSourcesFolder);
            var virtualPath = Path.Combine(appPaths.Path, DataSourcesFolder);
            return (physicalPath, virtualPath);
        }

        private IEnumerable<Type> LoadAppDataSources(int appId) => Log.Func($"a:{appId}", l =>
        {
            _logStore.Add(EavLogs.LogStoreAppDataSourcesLoader, Log);

            var (physicalPath, virtualPath) = GetAppDataSourceFolderPaths(appId);

            if (!Directory.Exists(physicalPath))
                return (null, $"no folder {physicalPath}");

            var compiler = _codeCompilerLazy.Value;
            var types = new List<Type>();
            var errors = new List<string>();

            foreach (var dataSourceFile in Directory.GetFiles(physicalPath, "*.cs", SearchOption.TopDirectoryOnly))
            {
                try
                {
                    var (type, errorMessages) = compiler.GetTypeOrErrorMessages(
                        virtualPath: Path.Combine(virtualPath, Path.GetFileName(dataSourceFile)), 
                        className: Path.GetFileNameWithoutExtension(dataSourceFile), 
                        throwOnError: false);

                    if (type != null) types.Add(type);
                    if (!string.IsNullOrEmpty(errorMessages)) errors.Add(errorMessages);
                }
                catch (Exception ex)
                {
                    errors.Add(ex.Message);
                    l.Ex(ex);
                }
            }

            if (errors.Any()) l.A($"Errors: {string.Join(",", errors)}");

            return types.Any() 
                ? (types, $"OK, DataSources:{types.Count} ({string.Join(";", types.Select(t => t.FullName))}), path:{virtualPath}")
                : (null, $"OK, no working DataSources found, path:{virtualPath}") ;
        });
    }
}
