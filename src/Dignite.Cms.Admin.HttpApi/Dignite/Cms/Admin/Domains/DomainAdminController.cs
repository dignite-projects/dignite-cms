using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [HttpPost]
        public async Task<DomainDto> CreateAsync(CreateDomainInput input)
        {
            return await _entryAppService.CreateAsync(input);
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

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<DomainDto> UpdateAsync(Guid id, UpdateDomainInput input)
        {
            return await _entryAppService.UpdateAsync(id, input);   
        }
    }
}
