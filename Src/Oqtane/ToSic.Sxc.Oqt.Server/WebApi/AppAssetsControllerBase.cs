using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Oqtane.Shared;
using System.IO;
using ToSic.Eav.Plumbing;
using ToSic.Eav.WebApi;
using ToSic.Sxc.Apps;
using ToSic.Sxc.Oqt.Server.Adam;
using ToSic.Sxc.Oqt.Server.Controllers;

namespace ToSic.Sxc.Oqt.Server.WebApi
{
    public abstract class AppAssetsControllerBase : OqtControllerBase<DummyControllerReal>
    {
        private string Route { get; }

        private readonly LazyInitLog<AppFolder> _appFolder;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly SiteState _siteState;

        protected AppAssetsControllerBase(AppAssetsDependencies dependencies, string route, string logSuffix): base(logSuffix)
        {
            _hostingEnvironment = dependencies.HostingEnvironment;
            _appFolder = dependencies.AppFolder.SetLog(Log);
            _siteState = dependencies.SiteState;
            Route = route;
        }

        [HttpGet("{*filePath}")]
        public IActionResult GetFile([FromRoute] string appName, [FromRoute] string filePath)
        {
            try
            {
                if (appName == WebApiConstants.Auto) appName = _appFolder.Ready.GetAppFolder();

                var alias = _siteState.Alias;
                var fullFilePath = ContentFileHelper.GetFilePath(_hostingEnvironment.ContentRootPath, alias, Route, appName, filePath);
                if (string.IsNullOrEmpty(fullFilePath)) return NotFound();

                var fileBytes = System.IO.File.ReadAllBytes(fullFilePath);
                var mimeType = ContentFileHelper.GetMimeType(fullFilePath);

                return mimeType.StartsWith("image") ? File(fileBytes, mimeType) :
                    new FileContentResult(fileBytes, mimeType) { FileDownloadName = Path.GetFileName(fullFilePath) };
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
