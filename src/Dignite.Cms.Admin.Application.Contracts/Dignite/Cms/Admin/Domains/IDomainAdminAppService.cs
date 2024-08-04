using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Dignite.Cms.Admin.Domains
{
    public interface IDomainAdminAppService: ICreateUpdateAppService<
        DomainDto,
        Guid,
        CreateDomainInput,
        UpdateDomainInput>
    {
        Task<bool> NameExistsAsync(string domainName);

        Task<DomainDto> GetBoundAsync();
    }
}
