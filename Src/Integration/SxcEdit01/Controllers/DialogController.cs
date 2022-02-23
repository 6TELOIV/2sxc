﻿using Microsoft.AspNetCore.Mvc;
using ToSic.Eav.WebApi.Routing;
using ToSic.Sxc.WebApi.Admin;

namespace IntegrationSamples.SxcEdit01.Controllers
{
    [Route(IntegrationConstants.DefaultRouteRoot + AreaRoutes.Admin)]
    [ApiController]
    public class DialogController : IntControllerBase<DialogControllerReal>
    {
        // IMPORTANT: Uses the Proxy/Real concept - see https://r.2sxc.org/proxy-controllers

        public DialogController() :base("SysCnt") { }

        [HttpGet]
        public DialogContextStandaloneDto Settings(int appId) => Real.DialogSettings(appId);
    }
}
