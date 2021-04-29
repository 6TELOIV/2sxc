﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Oqtane.Shared;
using ToSic.Eav.WebApi.PublicApi;
using ToSic.Sxc.Oqt.Shared;

namespace ToSic.Sxc.Oqt.Server.Controllers.Admin
{
    /// <inheritdoc cref="IMetadataController" />
    //[ValidateAntiForgeryToken]
    [Authorize(Roles = RoleNames.Admin)]

    // Release routes
    [Route(WebApiConstants.ApiRoot + "/admin/[controller]/[action]")]
    [Route(WebApiConstants.ApiRoot2 + "/admin/[controller]/[action]")]
    [Route(WebApiConstants.ApiRoot3 + "/admin/[controller]/[action]")]

    // Beta routes
    [Route(WebApiConstants.WebApiStateRoot + "/admin/[controller]/[action]")]
    public class MetadataController : OqtStatefulControllerBase, IMetadataController
    {
        protected override string HistoryLogName => "Api.Metadata";

        public MetadataController(StatefulControllerDependencies dependencies) : base(dependencies)
        { }

        [HttpGet]
        public IEnumerable<Dictionary<string, object>> Get(int appId, int targetType, string keyType, string key, string contentType)
            => Eav.WebApi.MetadataApi.Get(ServiceProvider, appId, targetType, keyType, key, contentType);

    }
}