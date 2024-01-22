using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Dignite.Cms.Admin.Sites
{
    public interface ISiteAdminAppService
    : ICrudAppService<
        SiteDto,
        Guid,
        GetSitesInput,
        CreateSiteInput,
        UpdateSiteInput>
    {
        Task<bool> NameExistsAsync(string name);
        Task<bool> HostExistsAsync(string host);
    }
}
