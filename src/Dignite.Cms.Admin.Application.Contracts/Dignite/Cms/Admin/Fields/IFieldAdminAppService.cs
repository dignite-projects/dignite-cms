using System;
using Volo.Abp.Application.Services;

namespace Dignite.Cms.Admin.Fields
{
    public interface IFieldAdminAppService
    : ICrudAppService<
        FieldDto,
        Guid,
        GetFieldsInput,
        CreateFieldInput,
        UpdateFieldInput>
    {
    }
}
