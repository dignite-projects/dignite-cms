// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using Dignite.Cms.Admin.Domains;
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
namespace Dignite.Cms.Admin.Domains;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(IDomainAdminAppService), typeof(DomainAdminClientProxy))]
public partial class DomainAdminClientProxy : ClientProxyBase<IDomainAdminAppService>, IDomainAdminAppService
{
    public virtual async Task<DomainDto> FindByNameAsync(string domainName)
    {
        return await RequestAsync<DomainDto>(nameof(FindByNameAsync), new ClientProxyRequestTypeValue
        {
            { typeof(string), domainName }
        });
    }

    public virtual async Task<DomainDto> GetBoundAsync()
    {
        return await RequestAsync<DomainDto>(nameof(GetBoundAsync));
    }

    public virtual async Task<bool> NameExistsAsync(string domainName)
    {
        return await RequestAsync<bool>(nameof(NameExistsAsync), new ClientProxyRequestTypeValue
        {
            { typeof(string), domainName }
        });
    }

    public virtual async Task<DomainDto> UpdateAsync(UpdateDomainInput input)
    {
        return await RequestAsync<DomainDto>(nameof(UpdateAsync), new ClientProxyRequestTypeValue
        {
            { typeof(UpdateDomainInput), input }
        });
    }
}
