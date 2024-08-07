// This file is automatically generated by ABP framework to use MVC Controllers from CSharp
using Dignite.Cms.Public.Settings;
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
namespace Dignite.Cms.Public.Settings;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(ISiteSettingsPublicAppService), typeof(SiteSettingsPublicClientProxy))]
public partial class SiteSettingsPublicClientProxy : ClientProxyBase<ISiteSettingsPublicAppService>, ISiteSettingsPublicAppService
{
    public virtual async Task<string> GetDefaultLanguageAsync()
    {
        return await RequestAsync<string>(nameof(GetDefaultLanguageAsync));
    }

    public virtual async Task<IEnumerable<String>> GetAllLanguagesAsync()
    {
        return await RequestAsync<IEnumerable<String>>(nameof(GetAllLanguagesAsync));
    }

    public virtual async Task<BrandDto> GetBrandAsync()
    {
        return await RequestAsync<BrandDto>(nameof(GetBrandAsync));
    }
}
