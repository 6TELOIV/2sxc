﻿using System;
using System.Collections.Generic;
using System.Text;
using ToSic.Eav;
using ToSic.Eav.Context;
using ToSic.Eav.Logging;
using ToSic.Eav.Persistence.Logging;
using ToSic.Eav.Plumbing;
using ToSic.Eav.WebApi.ImportExport;
using ToSic.Eav.WebApi.Plumbing;
using ToSic.Sxc.Context;
using ToSic.Sxc.Run;

namespace ToSic.Sxc.WebApi.Sys
{
    public class InstallControllerReal<THttpResponseType> : HasLog<InstallControllerReal<THttpResponseType>>
    {
        public const string LogSuffix = "Install";

        #region System Installation

        public InstallControllerReal(
            LazyInitLog<IContextOfSite> context,
            Lazy<IEnvironmentInstaller> envInstallerLazy, 
            Lazy<ImportFromRemote> impFromRemoteLazy, 
            Lazy<IUser> userLazy,
            ResponseMaker<THttpResponseType> responseMaker
            ) : base($"{LogNames.WebApi}.{LogSuffix}Rl")
        {
            _context = context.SetLog(Log);
            _envInstallerLazy = envInstallerLazy;
            _impFromRemoteLazy = impFromRemoteLazy;
            _userLazy = userLazy;
            _responseMaker = responseMaker;
        }

        private readonly LazyInitLog<IContextOfSite> _context;
        private readonly Lazy<IEnvironmentInstaller> _envInstallerLazy;
        private readonly Lazy<ImportFromRemote> _impFromRemoteLazy;
        private readonly Lazy<IUser> _userLazy;
        private readonly ResponseMaker<THttpResponseType> _responseMaker;


        /// <summary>
        /// Finish system installation which had somehow been interrupted
        /// </summary>
        /// <returns></returns>
        public bool Resume() => _envInstallerLazy.Value.ResumeAbortedUpgrade();

        #endregion


        #region App / Content Package Installation

        /// <summary>
        /// Before this was GET Module/RemoteInstallDialogUrl
        /// </summary>
        /// <param name="isContentApp"></param>
        /// <returns></returns>
        public string RemoteWizardUrl(bool isContentApp, IModule module) 
            => _envInstallerLazy.Value.Init(Log)
                .GetAutoInstallPackagesUiUrl(
                    _context.Ready.Site,
                    module,
                    isContentApp);

        /// <summary>
        /// Before this was GET Installer/InstallPackage
        /// </summary>
        /// <param name="packageUrl"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public THttpResponseType RemotePackage(string packageUrl, IModule container)
        {
            var wrapLog = Log.Call<THttpResponseType>();

            var isApp = !container.IsContent;

            Log.Add("install package:" + packageUrl);

            var block = container.BlockIdentifier;
            var (success, messages) = _impFromRemoteLazy.Value.Init(_userLazy.Value, Log)
                .InstallPackage(block.ZoneId, block.AppId, isApp, packageUrl);

            Log.Add("install completed with success:" + success);

            return success ? wrapLog("Ok",_responseMaker.Ok()) : wrapLog("Error",_responseMaker.InternalServerError(MessageBuilder(messages)));
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
