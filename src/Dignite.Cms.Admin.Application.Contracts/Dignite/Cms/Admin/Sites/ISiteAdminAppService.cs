using System;
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
    }
}
