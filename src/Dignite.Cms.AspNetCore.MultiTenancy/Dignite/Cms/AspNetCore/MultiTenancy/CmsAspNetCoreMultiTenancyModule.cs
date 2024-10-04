using Dignite.Cms.Public;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.Modularity;

namespace Dignite.Cms.AspNetCore.MultiTenancy
{
    [DependsOn(
        typeof(CmsPublicApplicationContractsModule),
        typeof(AbpAspNetCoreMultiTenancyModule)
        )]
    public class CmsAspNetCoreMultiTenancyModule : AbpModule
    {
    }
}
