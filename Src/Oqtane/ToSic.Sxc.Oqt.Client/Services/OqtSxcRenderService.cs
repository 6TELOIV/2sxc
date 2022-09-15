using Oqtane.Modules;
using Oqtane.Services;
using Oqtane.Shared;
using System.Net.Http;
using System.Threading.Tasks;
using ToSic.Sxc.Oqt.Shared.Models;

namespace ToSic.Sxc.Oqt.Client.Services
{
    public class OqtSxcRenderService : ServiceBase, IOqtSxcRenderService, IService
    {
        public OqtSxcRenderService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string ApiUrl => CreateApiUrl("OqtSxcRender");

        public async Task<OqtViewResultsDto> PrepareAsync(int aliasId, int pageId, int moduleId, string culture,
            string query, bool preRender)
        {
            // TODO: @STV - GET pre-render info to the server as well and include in insights log
            // So we can see if the render was server or client-side
            var url = CreateAuthorizationPolicyUrl($"{ApiUrl}/{aliasId}/{pageId}/{moduleId}/{culture}/Prepare{query}", EntityNames.Module, moduleId);
            return await GetJsonAsync<OqtViewResultsDto>(url);
        }
    }
}
