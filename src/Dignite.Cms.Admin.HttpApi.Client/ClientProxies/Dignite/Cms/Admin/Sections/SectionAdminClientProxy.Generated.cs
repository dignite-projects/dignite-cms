// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using Dignite.Cms.Admin.Sections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Http.Client.ClientProxying;
using Volo.Abp.Http.Modeling;

// ReSharper disable once CheckNamespace
namespace Dignite.Cms.Admin.Sections;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(ISectionAdminAppService), typeof(SectionAdminClientProxy))]
public partial class SectionAdminClientProxy : ClientProxyBase<ISectionAdminAppService>, ISectionAdminAppService
{
    public virtual async Task<SectionDto> CreateAsync(CreateSectionInput input)
    {
        return await RequestAsync<SectionDto>(nameof(CreateAsync), new ClientProxyRequestTypeValue
        {
            { typeof(CreateSectionInput), input }
        });
    }

    public virtual async Task<SectionDto> UpdateAsync(Guid id, UpdateSectionInput input)
    {
        return await RequestAsync<SectionDto>(nameof(UpdateAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), id },
            { typeof(UpdateSectionInput), input }
        });
    }

    public virtual async Task DeleteAsync(Guid id)
    {
        await RequestAsync(nameof(DeleteAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), id }
        });
    }

    public virtual async Task<PagedResultDto<SectionDto>> GetListAsync(GetSectionsInput input)
    {
        return await RequestAsync<PagedResultDto<SectionDto>>(nameof(GetListAsync), new ClientProxyRequestTypeValue
        {
            { typeof(GetSectionsInput), input }
        });
    }

    public virtual async Task<SectionDto> GetAsync(Guid id)
    {
        return await RequestAsync<SectionDto>(nameof(GetAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), id }
        });
    }

    public virtual async Task<bool> NameExistsAsync(Guid siteId, string name)
    {
        return await RequestAsync<bool>(nameof(NameExistsAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), siteId },
            { typeof(string), name }
        });
    }
}
