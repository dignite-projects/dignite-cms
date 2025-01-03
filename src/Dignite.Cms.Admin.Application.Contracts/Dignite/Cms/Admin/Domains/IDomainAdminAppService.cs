﻿using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Dignite.Cms.Admin.Domains
{
    public interface IDomainAdminAppService: IApplicationService
    {
        Task<DomainDto> FindByNameAsync(string domainName);
        Task<bool> NameExistsAsync(string domainName);

        Task<DomainDto> GetBoundAsync();

        Task<DomainDto> UpdateAsync(UpdateDomainInput input);
    }
}
