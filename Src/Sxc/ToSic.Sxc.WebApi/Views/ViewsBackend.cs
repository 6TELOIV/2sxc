﻿using System.Collections.Generic;
using System.Linq;
using ToSic.Eav.Apps;
using ToSic.Eav.Context;
using ToSic.Eav.Data;
using ToSic.Eav.DataFormats.EavLight;
using ToSic.Lib.Logging;
using ToSic.Eav.Serialization;
using ToSic.Eav.WebApi.Dto;
using ToSic.Eav.WebApi.Security;
using ToSic.Lib.DI;
using ToSic.Lib.Services;
using ToSic.Sxc.Apps;
using ToSic.Sxc.WebApi.ImportExport;
using ToSic.Eav.Apps.Work;
using ToSic.Sxc.Apps.Work;

namespace ToSic.Sxc.WebApi.Views
{
    public class ViewsBackend: ServiceBase
    {
        private readonly AppWorkContextService _appWorkCtxSvc;
        private readonly LazySvc<WorkViewsMod> _workViewsMod;
        private readonly AppWorkSxc _appWorkSxc;
        private readonly LazySvc<IConvertToEavLight> _convertToEavLight;
        private readonly Generator<ImpExpHelpers> _impExpHelpers;
        private readonly ISite _site;
        private readonly IUser _user;

        public ViewsBackend(
            AppWorkContextService appWorkCtxSvc,
            AppWorkSxc appWorkSxc,
            LazySvc<WorkViewsMod> workViewsMod,
            IContextOfSite context,
            LazySvc<IConvertToEavLight> convertToEavLight,
            Generator<ImpExpHelpers> impExpHelpers
        ) : base("Bck.Views")
        {
            _appWorkCtxSvc = appWorkCtxSvc;
            ConnectServices(
                _workViewsMod = workViewsMod,
                _appWorkSxc = appWorkSxc,
                _convertToEavLight = convertToEavLight,
                _impExpHelpers = impExpHelpers,
                _site = context.Site,
                _user = context.User
            );
        }

        public IEnumerable<ViewDetailsDto> GetAll(int appId)
        {
            var l = Log.Fn<IEnumerable<ViewDetailsDto>>($"get all a#{appId}");

            var appSysCtx = _appWorkSxc.AppWork.ContextPlus(appId);
            var contentTypes = appSysCtx.AppState.ContentTypes.OfScope(Scopes.Default).ToList();

            var viewList = _appWorkSxc.AppViews(appSysCtx).GetAll().ToList();
            Log.A($"attribute list count:{contentTypes.Count}, template count:{viewList.Count}");
            var ser = _convertToEavLight.Value as ConvertToEavLight;
            var views = viewList.Select(view => new ViewDetailsDto
            {
                Id = view.Id, Name = view.Name, ContentType = TypeSpecs(contentTypes, view.ContentType, view.ContentItem),
                PresentationType = TypeSpecs(contentTypes, view.PresentationType, view.PresentationItem),
                ListContentType = TypeSpecs(contentTypes, view.HeaderType, view.HeaderItem),
                ListPresentationType = TypeSpecs(contentTypes, view.HeaderPresentationType, view.HeaderPresentationItem),
                TemplatePath = view.Path,
                IsHidden = view.IsHidden,
                ViewNameInUrl = view.UrlIdentifier,
                Guid = view.Guid,
                List = view.UseForList,
                HasQuery = view.QueryRaw != null,
                Used = view.Entity.Parents().Count,
                IsShared = view.IsShared,
                EditInfo = new EditInfoDto(view.Entity),
                Metadata = ser?.CreateListOfSubEntities(view.Metadata, SubEntitySerialization.AllTrue()),
                Permissions = new HasPermissionsDto {Count = view.Entity.Metadata.Permissions.Count()},
            }).ToList();
            return l.Return(views, $"{views.Count}");
        }


        /// <summary>
        /// Helper to prepare a quick-info about 1 content type
        /// </summary>
        /// <param name="allCTs"></param>
        /// <param name="staticName"></param>
        /// <param name="maybeEntity"></param>
        /// <returns></returns>
        private static ViewContentTypeDto TypeSpecs(IEnumerable<IContentType> allCTs, string staticName, IEntity maybeEntity)
        {
            var found = allCTs.FirstOrDefault(ct => ct.NameId == staticName);
            return new ViewContentTypeDto
            {
                StaticName = staticName, Id = found?.Id ?? 0, Name = found == null ? "no content type" : found.Name,
                DemoId = maybeEntity?.EntityId ?? 0,
                DemoTitle = maybeEntity?.GetBestTitle() ?? ""
            };
        }

        public bool Delete(int appId, int id)
        {
            // todo: extra security to only allow zone change if host user
            Log.A($"delete a{appId}, t:{id}");
            var app = _impExpHelpers.New().GetAppAndCheckZoneSwitchPermissions(_site.ZoneId, appId, _user, _site.ZoneId);
            //var cms = _cmsManagerLazy.Value.Init(app);
            //cms.Views.DeleteView(id);
            _workViewsMod.Value.InitContext(_appWorkCtxSvc.Context(app)).DeleteView(id);
            return true;
        }
    }
}
