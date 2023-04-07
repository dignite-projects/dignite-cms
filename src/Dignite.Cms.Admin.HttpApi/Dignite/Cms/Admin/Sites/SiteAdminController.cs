using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Dignite.Cms.Admin.Sites
{
    [RemoteService(Name = CmsAdminRemoteServiceConsts.RemoteServiceName)]
    [Area(CmsAdminRemoteServiceConsts.ModuleName)]
    [Authorize(Permissions.CmsAdminPermissions.Site.Default)]
    [Route("api/cms-admin/sites")]
    public class SiteAdminController : CmsAdminController, ISiteAdminAppService
    {
        private readonly ISiteAdminAppService _siteAppService;

        public SiteAdminController(ISiteAdminAppService siteAppService)
        {
            _siteAppService = siteAppService;
        }


        [HttpGet]
        [Route("{id:Guid}")]
        public virtual async Task<SiteDto> GetAsync(Guid id)
        {
            return await _siteAppService.GetAsync(id);
        }

        [HttpGet]
        public async Task<PagedResultDto<SiteDto>> GetListAsync(GetSitesInput input)
        {
            return await _siteAppService.GetListAsync(input);
        }



        [HttpPost]
        public async Task<SiteDto> CreateAsync(CreateSiteInput input)
        {
            return await _siteAppService.CreateAsync(input);
        }


        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<SiteDto> UpdateAsync(Guid id, UpdateSiteInput input)
        {
            return await _siteAppService.UpdateAsync(id,input);
        }


        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task DeleteAsync(Guid id)
        {
            await _siteAppService.DeleteAsync(id);
        }
    }
}
