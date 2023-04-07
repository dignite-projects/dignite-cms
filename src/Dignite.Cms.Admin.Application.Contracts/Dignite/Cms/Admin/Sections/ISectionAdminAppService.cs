using System;
using Volo.Abp.Application.Services;

namespace Dignite.Cms.Admin.Sections
{
    public interface ISectionAdminAppService
    : ICrudAppService<
        SectionDto,
        Guid,
        GetSectionsInput,
        CreateSectionInput,
        UpdateSectionInput>
    {
    }
}
