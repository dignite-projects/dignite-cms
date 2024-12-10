using Dignite.Cms.Sites;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;

namespace Dignite.Cms.Public.Sites
{
    [RemoteService(Name = CmsPublicRemoteServiceConsts.RemoteServiceName)]
    [Area(CmsPublicRemoteServiceConsts.ModuleName)]
    [Route("api/cms-public/site")]
    public class SitePublicController : CmsPublicController, ISitePublicAppService
    {
        private readonly ISitePublicAppService _sitePublicAppService;

        public SitePublicController(ISitePublicAppService sitePublicAppService)
        {
            _sitePublicAppService = sitePublicAppService;
        }

        [HttpGet]
        public async Task<SiteDto> GetAsync()
        {
            return await _sitePublicAppService.GetAsync();
        }
    }
}
