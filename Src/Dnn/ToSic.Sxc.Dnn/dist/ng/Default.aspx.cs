﻿using DotNetNuke.Framework;
using System;
using System.IO;
using System.Web;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using ToSic.Sxc.Services;

namespace ToSic.Sxc.Dnn.dist.ng
{
    public partial class Default : CachedPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Clear();
            Response.AppendHeader("test-dev", "2sxc");

            //var sp = DnnStaticDi.GetPageScopedServiceProvider();
            //var pageService = sp.GetRequiredService<IPageService>();
            //pageService.AddCsp("test-csp", "2sxc");

            PageOutputCached("~/DesktopModules/ToSIC_SexyContent/dist/ng/ui.html");
        }
    }
}