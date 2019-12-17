﻿using System;
using System.Threading;
using System.Web;
using ToSic.Eav.Apps;
using ToSic.Eav.Documentation;
using ToSic.Eav.Logging;
using ToSic.Eav.Run;
using ToSic.Sxc.Data;

namespace ToSic.Sxc.Apps
{
    /// <summary>
    /// A <em>single-use</em> app-object providing quick simple api to access
    /// name, folder, data, metadata etc.
    /// </summary>
    [PublicApi]
    public class App : Eav.Apps.App, IApp
    {
        #region Dynamic Properties: Configuration, Settings, Resources
        /// <inheritdoc />
        public AppConfiguration Configuration => _appConfig
                                                 // Create config object. Note that AppConfiguration could be null, then it would use default values
                                                 ?? (_appConfig = new AppConfiguration(AppConfiguration, Log));

        private AppConfiguration _appConfig;

        [PrivateApi("obsolete, use the typed accessor instead, only included for old-compatibility")]
        [Obsolete("use the new, typed accessor instead")]
        dynamic SexyContent.Interfaces.IApp.Configuration
        {
            get
            {
                var c = Configuration;
                return c?.Entity != null ? new DynamicEntity(c.Entity, new[] {Thread.CurrentThread.CurrentCulture.Name}, null) : null;
            }
        }

        /// <inheritdoc />
        public dynamic Settings
        {
            get
            {
                if(!_settingsLoaded && AppSettings != null)
                    _settings = new DynamicEntity(AppSettings, new[] {Thread.CurrentThread.CurrentCulture.Name}, null);
                _settingsLoaded = true;
                return _settings;
            }
        }
        private bool _settingsLoaded;
        private dynamic _settings;

        /// <inheritdoc />
        public dynamic Resources
        {
            get
            {
                if(!_resLoaded && AppResources!= null)
                    _res = new DynamicEntity(AppResources, new[] {Thread.CurrentThread.CurrentCulture.Name}, null);
                _resLoaded = true;
                return _res;
            }
        }
        private bool _resLoaded;
        private dynamic _res;

        #endregion


        /// <summary>
        /// New constructor which auto-configures the app-data
        /// </summary>
        [PrivateApi]
        public App(ITenant tenant, 
            int zoneId, 
            int appId, 
            Func<Eav.Apps.App, IAppDataConfiguration> buildConfig, 
            bool allowSideEffects, 
            ILog parentLog = null)
            : base(tenant, zoneId, appId, allowSideEffects, buildConfig, parentLog) { }

        #region Paths
        /// <inheritdoc />
        public string Path => VirtualPathUtility.ToAbsolute(GetRootPath());
        /// <inheritdoc />
        public string Thumbnail => System.IO.File.Exists(PhysicalPath + IconFile) ? Path + IconFile : null;

        #endregion

        
    }
}