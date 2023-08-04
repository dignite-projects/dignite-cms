// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using Dignite.Cms.Public.Sites;
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
namespace Dignite.Cms.Public.Sites;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(ISitePublicAppService), typeof(SitePublicClientProxy))]
public partial class SitePublicClientProxy : ClientProxyBase<ISitePublicAppService>, ISitePublicAppService
{
    public virtual async Task<SiteDto> FindByNameAsync(string name)
    {
        return await RequestAsync<SiteDto>(nameof(FindByNameAsync), new ClientProxyRequestTypeValue
        {
            { typeof(string), name }
        });
    }

    public virtual async Task<SiteDto> GetDefaultAsync()
    {
        return await RequestAsync<SiteDto>(nameof(GetDefaultAsync));
    }
}