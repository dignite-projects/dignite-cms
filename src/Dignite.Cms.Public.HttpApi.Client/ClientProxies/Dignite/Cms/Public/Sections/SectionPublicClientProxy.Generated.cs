// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using Dignite.Cms.Public.Sections;
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
namespace Dignite.Cms.Public.Sections;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(ISectionPublicAppService), typeof(SectionPublicClientProxy))]
public partial class SectionPublicClientProxy : ClientProxyBase<ISectionPublicAppService>, ISectionPublicAppService
{
    public virtual async Task<SectionDto> FindByNameAsync(Guid siteId, string name)
    {
        return await RequestAsync<SectionDto>(nameof(FindByNameAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), siteId },
            { typeof(string), name }
        });
    }

    public virtual async Task<SectionDto> FindByEntryPathAsync(Guid siteId, string entryPath)
    {
        return await RequestAsync<SectionDto>(nameof(FindByEntryPathAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), siteId },
            { typeof(string), entryPath }
        });
    }

    public virtual async Task<ListResultDto<SectionDto>> GetListAsync(GetSectionsInput input)
    {
        return await RequestAsync<ListResultDto<SectionDto>>(nameof(GetListAsync), new ClientProxyRequestTypeValue
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

    public virtual async Task<SectionDto> GetDefaultAsync(Guid siteId)
    {
        return await RequestAsync<SectionDto>(nameof(GetDefaultAsync), new ClientProxyRequestTypeValue
        {
            { typeof(Guid), siteId }
        });
    }
}
