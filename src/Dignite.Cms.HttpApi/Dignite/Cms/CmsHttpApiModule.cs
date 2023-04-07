using Dignite.Cms.Admin;
using Dignite.Cms.Public;
using Volo.Abp.Modularity;
using Volo.CmsKit;

namespace Dignite.Cms;

[DependsOn(
    typeof(CmsApplicationContractsModule),
    typeof(CmsAdminHttpApiModule),
    typeof(CmsPublicHttpApiModule),
    typeof(CmsKitHttpApiModule))]
public class CmsHttpApiModule : AbpModule
{
}
