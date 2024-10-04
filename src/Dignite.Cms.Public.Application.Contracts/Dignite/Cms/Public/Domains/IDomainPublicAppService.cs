using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Dignite.Cms.Public.Domains
{
    public interface IDomainPublicAppService: IApplicationService
    {
        Task<DomainDto> FindByNameAsync(string domainName);
    }
}
