using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Dignite.Cms.Public.Sites
{
    public interface ISitePublicAppService : IApplicationService
    {
        Task<SiteDto> FindByNameAsync(string name);
        Task<SiteDto> FindByHostAsync(string host);
    }
}
