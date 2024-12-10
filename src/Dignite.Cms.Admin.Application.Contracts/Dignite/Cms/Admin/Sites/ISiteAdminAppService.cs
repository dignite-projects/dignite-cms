using Dignite.Cms.Sites;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Dignite.Cms.Admin.Sites
{
    public interface ISiteAdminAppService : IApplicationService
    {
        Task<SiteDto> GetAsync();

        Task UpdateAsync(UpdateSiteInput input);
    }
}
