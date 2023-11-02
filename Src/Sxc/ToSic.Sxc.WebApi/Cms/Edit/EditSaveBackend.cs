﻿using System;
using System.Collections.Generic;
using System.Linq;
using ToSic.Eav.Apps;
using ToSic.Eav.Apps.Work;
using ToSic.Eav.Context;
using ToSic.Eav.Data;
using ToSic.Eav.Data.Build;
using ToSic.Eav.ImportExport.Json;
using ToSic.Eav.ImportExport.Serialization;
using ToSic.Eav.Security.Permissions;
using ToSic.Eav.WebApi.Dto;
using ToSic.Eav.WebApi.Errors;
using ToSic.Eav.WebApi.Formats;
using ToSic.Eav.WebApi.SaveHelpers;
using ToSic.Lib.DI;
using ToSic.Lib.Logging;
using ToSic.Lib.Services;
using ToSic.Sxc.Context;
using ToSic.Sxc.WebApi.Save;

namespace ToSic.Sxc.WebApi.Cms
{
    public class EditSaveBackend : ServiceBase
    {
        private readonly AppWork _appWork;
        private readonly DataBuilder _dataBuilder;
        private readonly SaveEntities _saveBackendHelper;
        private readonly SaveSecurity _saveSecurity;
        private readonly JsonSerializer _jsonSerializer;

        #region DI Constructor and Init

        public EditSaveBackend(
            SxcPagePublishing pagePublishing, 
            LazySvc<AppManager> appManagerLazy,
            AppWork appWork,
            Sxc.Context.IContextResolver ctxResolver,
            JsonSerializer jsonSerializer,
            SaveSecurity saveSecurity,
            SaveEntities saveBackendHelper,
            DataBuilder dataBuilder
            ) : base("Cms.SaveBk")
        {
            ConnectServices(
                _pagePublishing = pagePublishing,
                _appManagerLazy = appManagerLazy,
                _appWork = appWork,
                _ctxResolver = ctxResolver,
                _jsonSerializer = jsonSerializer,
                _saveSecurity = saveSecurity,
                _saveBackendHelper = saveBackendHelper,
                _dataBuilder = dataBuilder
            );
        }
        private readonly SxcPagePublishing _pagePublishing;
        private readonly LazySvc<AppManager> _appManagerLazy;
        private readonly Sxc.Context.IContextResolver _ctxResolver;

        public EditSaveBackend Init(int appId)
        {
            _appId = appId;
            // The context should be from the block if there is one, because it affects saving/publishing
            // Basically it can result in things being saved draft or titles being updated
            _context = _ctxResolver.GetBlockOrSetApp(appId);
            _pagePublishing.Init(_context);
            return this;
        }

        private IContextOfApp _context;
        private int _appId;
        #endregion

        public Dictionary<Guid, int> Save(EditDto package, bool partOfPage)
        {
            Log.A($"save started with a#{_appId}, i⋮{package.Items.Count}, partOfPage:{partOfPage}");

            var validator = new SaveDataValidator(package, Log);
            // perform some basic validation checks
            var containsOnlyExpectedNodesException = validator.ContainsOnlyExpectedNodes();
            if (containsOnlyExpectedNodesException != null)
                throw containsOnlyExpectedNodesException;

            // todo: unsure about this - thought I should check contentblockappid in group-header, because this is where it should be saved!
            //var contextAppId = appId;
            //var targetAppId = package.Items.First().Header.Group.ContentBlockAppId;
            //if (targetAppId != 0)
            //{
            //    Log.A($"detected content-block app to use: {targetAppId}; in context of app {contextAppId}");
            //    appId = targetAppId;
            //}

            // new API WIP
            var appCtx = _appWork.Context(_appId);
            var appEntities = _appWork.Entities(appState: appCtx.AppState);

            var ser = _jsonSerializer.SetApp(appCtx.AppState);
            // Since we're importing directly into this app, we would prefer local content-types
            ser.PreferLocalAppTypes = true;
            validator.PrepareForEntityChecks(appEntities);

            #region check if it's an update, and do more security checks then - shared with EntitiesController.Save
            // basic permission checks
            var permCheck = _saveSecurity.Init(_context).DoPreSaveSecurityCheck(_appId, package.Items);

            var foundItems = package.Items.Where(i => i.Entity.Id != 0 || i.Entity.Guid != Guid.Empty)
                .Select(i => i.Entity.Guid != Guid.Empty
                    ? appEntities.Get(i.Entity.Guid) // prefer guid access if available
                    : appEntities.Get(i.Entity.Id) // otherwise id
                );
            if (foundItems.Any(i => i != null) && !permCheck.EnsureAll(GrantSets.UpdateSomething, out var error))
                throw HttpException.PermissionDenied(error);
            #endregion


            var items = package.Items.Where(i => !i.Header.IsEmpty).Select(i =>
            {
                var ent = ser.Deserialize(i.Entity, false, false);

                var index = package.Items.IndexOf(i); // index is helpful in case of errors
                var isOkException = validator.EntityIsOk(index, ent);
                if (isOkException != null)
                    throw isOkException;

                var resultValidator = validator.IfUpdateValidateAndCorrectIds(index, ent);

                if (resultValidator.Exception != null)
                    throw resultValidator.Exception;

                ent = _dataBuilder.Entity.CreateFrom(ent, id: resultValidator.ResetId, isPublished: package.IsPublished,
                    placeDraftInBranch: package.DraftShouldBranch);

                //ent.ResetEntityId(resultValidator.ResetId ?? 0); //AjaxPreviewHelperWIP!
                //ent.IsPublished = package.IsPublished;
                //ent.PlaceDraftInBranch = package.DraftShouldBranch;

                // new in 11.01
                if (i.Header.Parent != null)
                {
                    // Check if Add was true, and fix if it had already been saved (EntityId != 0)
                    // the entityId is reset by the validator if it turns out to be an update
                    // todo: verify use - maybe it's to set before we save, as maybe afterwards it's always != 0?
                    var add = i.Header.AddSafe;
                    i.Header.Add = add;
                    if (ent.EntityId > 0 && add) i.Header.Add = false;
                }

                return new BundleWithHeader<IEntity>
                {
                    Header = i.Header,
                    Entity = ent
                };
            })
            .ToList();

            Log.A("items to save generated, all data tests passed");

            return _pagePublishing.SaveInPagePublishing(_ctxResolver.BlockOrNull(), _appId, items, partOfPage,
                    forceSaveAsDraft => DoSave(appEntities, appCtx, items, forceSaveAsDraft),
                    permCheck);
        }


        private Dictionary<Guid, int> DoSave(WorkEntities workEntities, IAppWorkCtx appCtx, List<BundleWithHeader<IEntity>> items, bool forceSaveAsDraft)
        {
            // only save entities that are
            // a) not in a group
            // b) in a group where the slot isn't marked as empty
            var entitiesToSave = items
                .Where(e => !e.Header.IsContentBlockMode || !e.Header.IsEmpty)
                .ToList();

            _saveBackendHelper.UpdateGuidAndPublishedAndSaveMany(workEntities.AppWorkCtx, entitiesToSave, forceSaveAsDraft);
            return _saveBackendHelper.GenerateIdList(workEntities, items);
        }
    }
}
