using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;

namespace Dignite.Cms.Public.Sites
{
    [RemoteService(Name = CmsPublicRemoteServiceConsts.RemoteServiceName)]
    [Area(CmsPublicRemoteServiceConsts.ModuleName)]
    [Route("api/cms-public/sites")]
    public class SitePublicController : CmsPublicController, ISitePublicAppService
    {
        private readonly ISitePublicAppService _siteAppService;

        public SitePublicController(ISitePublicAppService siteAppService)
        {
            _siteAppService = siteAppService;
        }

        [HttpGet]
        [Route("find-by-name")]
        public async Task<SiteDto> FindByNameAsync(string name)
        {
            return await _siteAppService.FindByNameAsync(name);
        }

        [HttpGet]
        [Route("get-default")]
        public async Task<SiteDto> GetDefaultAsync()
        {
            return await _siteAppService.GetDefaultAsync();
        }
    }
}
