﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ToSic.Eav.WebApi.Dto;
using ToSic.Eav.WebApi.Formats;
using ToSic.Eav.WebApi.PublicApi;
using ToSic.Eav.WebApi.Routing;
using ToSic.Sxc.WebApi.Cms;

namespace IntegrationSamples.SxcEdit01.Controllers
{
    //[AutoValidateAntiforgeryToken]
    [Route(IntegrationConstants.DefaultRouteRoot + AreaRoutes.Cms)]
    [ApiController]
    public class EditController: IntControllerBase<EditControllerReal>, IEditController
    {
        // IMPORTANT: Uses the Proxy/Real concept - see https://r.2sxc.org/proxy-controllers

        public EditController() : base("Edit") { }


        [HttpPost]
        [AllowAnonymous] // Anonymous is ok, security check happens internally
        public EditDto Load([FromBody] List<ItemIdentifier> items, int appId)
            => Real.Load(items, appId);

        [HttpPost]
        // todo #mvcSec [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        public Dictionary<Guid, int> Save([FromBody] EditDto package, int appId, bool partOfPage)
            => Real.Save(package, appId, partOfPage);

        [HttpGet]
        [HttpPost]
        [AllowAnonymous] // Anonymous is ok, security check happens internally
        public IEnumerable<EntityForPickerDto> EntityPicker(int appId, [FromBody] string[] items, string contentTypeName = null)
            => Real.EntityPicker(appId, items, contentTypeName);

        /// <inheritdoc />
        [HttpGet]
        //[DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        public LinkInfoDto LinkInfo(string link, int appId, string contentType = default, Guid guid = default, string field = default)
            => Real.LinkInfo(link, appId, contentType, guid, field);

    }
}
