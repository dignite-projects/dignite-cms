using Dignite.Cms.Admin;
using Dignite.Cms.Public;
using Volo.Abp.Modularity;
using Volo.CmsKit;

namespace Dignite.Cms;

[DependsOn(
    typeof(CmsApplicationContractsModule),
    typeof(CmsPublicHttpApiClientModule),
    typeof(CmsAdminHttpApiClientModule),
    typeof(CmsKitHttpApiClientModule))]
public class CmsHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
    }
}
