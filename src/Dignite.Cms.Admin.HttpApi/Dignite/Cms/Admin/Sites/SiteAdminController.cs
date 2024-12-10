using Dignite.Cms.Sites;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;

namespace Dignite.Cms.Admin.Sites
{
    [RemoteService(Name = CmsAdminRemoteServiceConsts.RemoteServiceName)]
    [Area(CmsAdminRemoteServiceConsts.ModuleName)]
    [Route("api/cms-admin/sites")]
    public class SiteAdminController : CmsAdminController, ISiteAdminAppService
    {
        private readonly ISiteAdminAppService _siteAdminAppService;

        public SiteAdminController(ISiteAdminAppService siteAdminAppService)
        {
            _siteAdminAppService = siteAdminAppService;
        }

        [HttpGet]
        public async Task<SiteDto> GetAsync()
        {
            return await _siteAdminAppService.GetAsync();
        }

        [HttpPut]
        public async Task UpdateAsync(UpdateSiteInput input)
        {
            await _siteAdminAppService.UpdateAsync(input);
        }
    }
}
