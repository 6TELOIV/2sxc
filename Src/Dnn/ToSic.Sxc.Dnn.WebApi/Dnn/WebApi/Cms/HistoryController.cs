﻿using System.Collections.Generic;
using System.Web.Http;
using DotNetNuke.Security;
using DotNetNuke.Web.Api;
using ToSic.Eav.Apps;
using ToSic.Eav.Persistence.Versions;
using ToSic.Eav.WebApi.Formats;
using ToSic.Eav.WebApi.PublicApi;
using ToSic.Sxc.WebApi;
using ToSic.Sxc.WebApi.Cms;

namespace ToSic.Sxc.Dnn.WebApi.Cms
{
    [SupportedModules("2sxc,2sxc-app")]
    [ValidateAntiForgeryToken]
    public class HistoryController : SxcApiControllerBase, IHistoryController
    {
        protected override string HistoryLogName => "Api.History";

        /// <inheritdoc />
        [HttpPost]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        public List<ItemHistory> Get(int appId, [FromBody] ItemIdentifier item) 
            => _build<AppManager>().Init(appId, Log).Entities
                .VersionHistory(_build<IdentifierHelper>().Init(Log).ResolveItemIdOfGroup(appId, item, Log).EntityId);

        /// <inheritdoc />
        [HttpPost]
        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.Edit)]
        public bool Restore(int appId, int changeId, [FromBody] ItemIdentifier item)
        {
            _build<AppManager>().Init(appId, Log).Entities
                .VersionRestore(_build<IdentifierHelper>().Init(Log).ResolveItemIdOfGroup(appId, item, Log).EntityId, changeId);
            return true;
        }

    }
}
