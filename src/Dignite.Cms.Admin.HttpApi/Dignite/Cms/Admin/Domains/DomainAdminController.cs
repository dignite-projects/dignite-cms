using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;

namespace Dignite.Cms.Admin.Domains
{
    [RemoteService(Name = CmsAdminRemoteServiceConsts.RemoteServiceName)]
    [Area(CmsAdminRemoteServiceConsts.ModuleName)]
    [Authorize(Permissions.CmsAdminPermissions.Entry.Default)]
    [Route("api/cms-admin/domains")]
    public class DomainAdminController : CmsAdminController, IDomainAdminAppService
    {
        private readonly IDomainAdminAppService _entryAppService;

        public DomainAdminController(IDomainAdminAppService entryAppService)
        {
            _entryAppService = entryAppService;
        }

        [HttpGet]
        [Route("bound")]
        public async Task<DomainDto> GetBoundAsync()
        {
            return await (_entryAppService.GetBoundAsync());
        }

        [HttpGet]
        [Route("name-exists")]
        public async Task<bool> NameExistsAsync(string domainName)
        {
            return await _entryAppService.NameExistsAsync(domainName);
        }

        [HttpPost]
        public async Task<DomainDto> UpdateAsync(UpdateDomainInput input)
        {
            return await _entryAppService.UpdateAsync(input);   
        }
    }
}
