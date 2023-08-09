﻿using System;
using ToSic.Eav.Context;
using ToSic.Eav.Data;
using ToSic.Lib.Helpers;
using ToSic.Sxc.Adam;

namespace ToSic.Sxc.Data.AsConverter
{
    public partial class AsConverterService
    {
        public AdamManager AdamManager => _adamManager.Get(GetAdamManager);
        private readonly GetOnce<AdamManager> _adamManager = new GetOnce<AdamManager>();

        /// <summary>
        /// Special helper - if the DynamicCode is generated by the service or used in a WebApi there is no block, but we can figure out the context.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private AdamManager GetAdamManager()
        {
            // if this was initialized with an ADAM manager, use that
            // 2023-08-08 2dm - try to use the new GetOnce.Reset functionality
            //if (_adamManagerPrepared != null) return _adamManagerPrepared;

            // If we don't even have a _DynCodeRoot (eg. when exporting from a neutral WebAPI)
            if (_DynCodeRoot is null)
                throw new Exception($"Can't create App Context for {nameof(AdamManager)} in {nameof(AsConverterService)} - no block, no App");

            IContextOfApp contextOfApp = _DynCodeRoot.Block?.Context;
            // TODO: @2dm - find out / document why this could even be null
            if (contextOfApp == null)
            {
                if (_DynCodeRoot.App == null)
                    throw new Exception("Can't create App Context for ADAM - no block, no App");
                contextOfApp = _contextOfAppLazy.Value;
                contextOfApp.ResetApp(_DynCodeRoot.App);
            }

            return _adamManagerLazy.Value.Init(contextOfApp, this, CompatibilityLevel);
        }
        #region ADAM / Folder

        public IFolder Folder(ICanBeEntity item, string fieldName) => AdamManager.Folder(item.Entity, fieldName);

        #endregion

    }
}
