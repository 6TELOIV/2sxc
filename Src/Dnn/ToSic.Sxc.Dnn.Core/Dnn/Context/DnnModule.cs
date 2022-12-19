﻿using System;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using ToSic.Eav.Apps;
using ToSic.Eav.Apps.Run;
using ToSic.Eav.Context;
using ToSic.Eav.DI;
using ToSic.Lib.Documentation;
using ToSic.Lib.Logging;
using ToSic.Sxc.Context;

namespace ToSic.Sxc.Dnn.Context
{
    /// <summary>
    /// The DNN implementation of a Block Container (a Module).
    /// </summary>
    [PrivateApi("this is just internal, external users don't really have anything to do with this")]
    public class DnnModule: Module<ModuleInfo>
    {
        #region Constructors and DI
        
        public DnnModule(IAppStates appStates, LazyInitLog<AppFinder> appFinderLazy, ISite site): base("Dnn.Contnr")
        {
            _appStates = appStates;
            _appFinderLazy = appFinderLazy.SetLog(Log);
            _site = site;
        }

        private readonly IAppStates _appStates;
        private readonly LazyInitLog<AppFinder> _appFinderLazy;
        private readonly ISite _site;

        /// <summary>
        /// We don't use a Constructor because of DI
        /// So you must always call Init
        /// </summary>
        public new DnnModule Init(ModuleInfo item, ILog parentLog)
        {
            base.Init(item, parentLog);
            var warpLog = Log.Fn<DnnModule>($"{item?.ModuleID}");
            return warpLog.Return(this);
        }

        /// <summary>
        /// We don't use a Constructor because of DI
        /// So you must always call Init
        /// </summary>
        public override IModule Init(int moduleId, ILog parentLog)
        {
            var mod = ModuleController.Instance.GetModule(moduleId, Null.NullInteger, false);
            Init(mod, parentLog);
            var wrapLog = Log.Fn<IModule>($"{moduleId}");
            return wrapLog.Return(this);
        }

        #endregion


        /// <inheritdoc />
        public override int Id => UnwrappedContents?.ModuleID ?? Eav.Constants.NullId;


        /// <inheritdoc />
        public override bool IsContent => (UnwrappedContents?.DesktopModule.ModuleName ?? "2sxc") == "2sxc";


        /// <inheritdoc />
        public override IBlockIdentifier BlockIdentifier
        {
            get
            {
                if (_blockIdentifier != null) return _blockIdentifier;
                if (UnwrappedContents == null) return null;

                // find ZoneId, AppId and prepare settings for next values
                // note: this is the correct zone, even if the module is shared from another portal, because the Site is prepared correctly
                var zoneId = _site.ZoneId;
                var (appId, appNameId) = GetInstanceAppIdAndName(zoneId);
                var settings = UnwrappedContents.ModuleSettings;

                // find block identifier
                Guid.TryParse(settings[Settings.ModuleSettingContentGroup]?.ToString(), out var blockGuid);

                // Check if we have preview-view identifier - for blocks which don't exist yet
                var previewTemplateString = settings[Settings.ModuleSettingsPreview]?.ToString();
                var overrideView = !string.IsNullOrEmpty(previewTemplateString)
                    ? Guid.Parse(previewTemplateString)
                    : new Guid();

                // Create identifier
                return _blockIdentifier = new BlockIdentifier(zoneId, appId, appNameId, blockGuid, overrideView);
            }
        }
        private IBlockIdentifier _blockIdentifier;

        private (int AppId, string AppNameId) GetInstanceAppIdAndName(int zoneId)
        {
            var wrapLog = Log.Fn<(int, string)>($"{zoneId}");

            var module = UnwrappedContents ?? throw new Exception("instance is not ModuleInfo");

            var msg = $"get appid from instance for Z:{zoneId} Mod:{module.ModuleID}";
            if (IsContent)
            {
                var appId = _appStates.DefaultAppId(zoneId);
                return wrapLog.Return((appId, "Content"), $"{msg} - use Default app: {appId}");
            }

            if (module.ModuleSettings.ContainsKey(Settings.ModuleSettingApp))
            {
                var guid = module.ModuleSettings[Settings.ModuleSettingApp].ToString();
                var appId = _appFinderLazy.Value.FindAppId(zoneId, guid);
                return wrapLog.Return((appId, guid), $"{msg} AppG:{guid} = app:{appId}");
            }

            Log.A($"{msg} not found = null");
            return wrapLog.Return((Eav.Constants.AppIdEmpty, Eav.Constants.AppNameIdEmpty), "not found");
        }
    }
}
