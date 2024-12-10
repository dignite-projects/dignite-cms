using Dignite.Cms.Sites;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Dignite.Cms.Public.Sites
{
    public interface ISitePublicAppService: IApplicationService
    {
        Task<SiteDto> GetAsync();
    }
}
