using Dignite.Cms.Public.Settings;
using Dignite.Cms.Settings;
using Microsoft.Extensions.Caching.Distributed;
using System;
using Volo.Abp.Caching;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;
using Volo.Abp.Ui.Branding;

namespace Dignite.Cms;

[Dependency(ReplaceServices = true)]
public class CmsBrandingProvider : DefaultBrandingProvider
{
    private readonly ISiteSettingsPublicAppService _siteSettingsPublicAppService;
    private readonly IDistributedCache<BrandDto> _domainCache;

    public CmsBrandingProvider(ISiteSettingsPublicAppService siteSettingsPublicAppService, IDistributedCache<BrandDto> domainCache)
    {
        _siteSettingsPublicAppService = siteSettingsPublicAppService;
        _domainCache = domainCache;
    }

    public override string AppName => GetBrand().AppName;

    private BrandDto GetBrand()
    {
        var brand = AsyncHelper.RunSync(() => _domainCache.GetOrAddAsync(
                "brand", //Cache key
                async () => await _siteSettingsPublicAppService.GetBrandAsync(),
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1)
                }
            ));

        return brand;
    }
}
