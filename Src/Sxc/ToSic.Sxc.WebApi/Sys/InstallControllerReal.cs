﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToSic.Eav;
using ToSic.Eav.Apps;
using ToSic.Eav.Configuration;
using ToSic.Eav.Context;
using ToSic.Eav.Data;
using ToSic.Eav.Data.PropertyLookup;
using ToSic.Lib.DI;
using ToSic.Lib.Logging;
using ToSic.Eav.Persistence.Logging;
using ToSic.Eav.WebApi.ImportExport;
using ToSic.Eav.WebApi.Plumbing;
using ToSic.Eav.WebApi.Sys;
using ToSic.Sxc.Context;
using ToSic.Sxc.Run;
using ToSic.Sxc.WebApi.App;
using ServiceBase = ToSic.Lib.Services.ServiceBase;

namespace ToSic.Sxc.WebApi.Sys
{
    public class InstallControllerReal<THttpResponseType> : ServiceBase
    {
        public const string LogSuffix = "Install";

        #region System Installation

        public InstallControllerReal(
            LazySvc<IContextOfSite> context,
            LazySvc<IEnvironmentInstaller> envInstallerLazy, 
            LazySvc<ImportFromRemote> impFromRemoteLazy, 
            LazySvc<IUser> userLazy,
            ResponseMaker<THttpResponseType> responseMaker,

            LazySvc<AppsBackend> appsBackend,
            LazySvc<IAppStates> appStates,
            LazySvc<AppSettingsStack> appSettingsStack) : base($"{LogNames.WebApi}.{LogSuffix}Rl")
        {
            ConnectServices(
                _context = context,
                _envInstallerLazy = envInstallerLazy,
                _impFromRemoteLazy = impFromRemoteLazy,
                _userLazy = userLazy,
                _responseMaker = responseMaker,
                _appStates = appStates,
                _appSettingsStack = appSettingsStack,
                _appsBackendLazy = appsBackend
            );
        }

        private readonly LazySvc<IContextOfSite> _context;
        private readonly LazySvc<IEnvironmentInstaller> _envInstallerLazy;
        private readonly LazySvc<ImportFromRemote> _impFromRemoteLazy;
        private readonly LazySvc<IUser> _userLazy;
        private readonly ResponseMaker<THttpResponseType> _responseMaker;
        private readonly LazySvc<IAppStates> _appStates;
        private readonly LazySvc<AppSettingsStack> _appSettingsStack;
        private readonly LazySvc<AppsBackend> _appsBackendLazy;


        /// <summary>
        /// Finish system installation which had somehow been interrupted
        /// </summary>
        /// <returns></returns>
        public bool Resume() => _envInstallerLazy.Value.ResumeAbortedUpgrade();

        #endregion

        #region App / Content Package Installation

        public InstallAppsDto InstallSettings(bool isContentApp, IModule module)
        {
            // Get Remote Install URL
            var site = _context.Value.Site;
            var url = _envInstallerLazy.Value.Init(Log)
                .GetAutoInstallPackagesUiUrl(site, module, isContentApp);

            // Get list of already installed Apps
            var appsOfThisSite = _appsBackendLazy.Value.Apps()
                .Select(a => new AppDtoLight
                {
                    name = a.Name,
                    guid = a.Guid,
                    version = a.Version,
                })
                .ToList();

            // Get list of allow/forbid rules for the App installer
            var primaryApp = _appStates.Value.GetPrimaryApp(site.ZoneId, Log);
            var settingsSources = _appSettingsStack.Value.Init(primaryApp).GetStack(ConfigurationConstants.Settings);
            var stack = new PropertyStack().Init(ConfigurationConstants.RootNameSettings, settingsSources);

            var rules = stack.InternalGetPath(new PropReqSpecs("SiteSetup.AutoInstallApps", null, Log), null);
            var ruleEntities = rules.Result as IEnumerable<IEntity>;    // note: Result is null if nothing found...
            var rulesFinal = ruleEntities?
                .Select(e => new SiteSetupAutoInstallAppsRule(e).GetRuleDto())
                .ToList();

            return new InstallAppsDto
            {
                remoteUrl = url,
                installedApps = appsOfThisSite,
                rules = rulesFinal
            };
        }

        /// <summary>
        /// Before this was GET Installer/InstallPackage
        /// </summary>
        /// <param name="packageUrl"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public THttpResponseType RemotePackage(string packageUrl, IModule container)
        {
            var wrapLog = Log.Fn<THttpResponseType>();

            var isApp = !container.IsContent;

            Log.A("install package:" + packageUrl);

            var block = container.BlockIdentifier;
            var (success, messages) = _impFromRemoteLazy.Value.Init(_userLazy.Value, Log)
                .InstallPackage(block.ZoneId, block.AppId, isApp, packageUrl);

            Log.A("install completed with success:" + success);

            return success 
                ? wrapLog.ReturnAsOk(_responseMaker.Ok()) 
                : wrapLog.Return(_responseMaker.InternalServerError(MessageBuilder(messages)), "error");
        }

        private static string MessageBuilder(List<Message> messages)
        {
            var err = new StringBuilder();
            foreach (var m in messages) err.AppendFormat("{0}", m.Text);
            return err.ToString();
        }

        #endregion
    }
}
