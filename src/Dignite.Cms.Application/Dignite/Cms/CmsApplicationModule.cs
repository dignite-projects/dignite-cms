using Dignite.Cms.Admin;
using Dignite.Cms.Public;
using Volo.Abp.Modularity;
using Volo.CmsKit;

namespace Dignite.Cms;

[DependsOn(
    typeof(CmsApplicationContractsModule),
    typeof(CmsPublicApplicationModule),
    typeof(CmsAdminApplicationModule),
    typeof(CmsKitApplicationModule)
    )]
public class CmsApplicationModule : AbpModule
{
}
