using Dignite.CmsKit.Brand;
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
    private readonly IBrandAppService _brandAppService;
    private readonly IDistributedCache<BrandDto> _domainCache;

    public CmsBrandingProvider(IBrandAppService brandAppService, IDistributedCache<BrandDto> domainCache)
    {
        _brandAppService = brandAppService;
        _domainCache = domainCache;
    }

    public override string AppName => GetBrand().Name;

    private BrandDto GetBrand()
    {
        var brand = AsyncHelper.RunSync(() => _domainCache.GetOrAddAsync(
                "brand", //Cache key
                async () => await _brandAppService.GetAsync(),
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1)
                }
            ));

        return brand;
    }
}
