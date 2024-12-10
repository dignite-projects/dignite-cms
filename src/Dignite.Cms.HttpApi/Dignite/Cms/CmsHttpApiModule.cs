using Dignite.Cms.Admin;
using Dignite.Cms.Public;
using Dignite.CmsKit;
using Volo.Abp.Modularity;

namespace Dignite.Cms;

[DependsOn(
    typeof(CmsApplicationContractsModule),
    typeof(CmsAdminHttpApiModule),
    typeof(CmsPublicHttpApiModule),
    typeof(DigniteCmsKitHttpApiModule))]
public class CmsHttpApiModule : AbpModule
{
}
