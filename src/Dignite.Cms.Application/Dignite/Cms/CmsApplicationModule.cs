using Dignite.Cms.Admin;
using Dignite.Cms.Public;
using Dignite.CmsKit;
using Volo.Abp.Modularity;

namespace Dignite.Cms;

[DependsOn(
    typeof(CmsApplicationContractsModule),
    typeof(CmsPublicApplicationModule),
    typeof(CmsAdminApplicationModule),
    typeof(DigniteCmsKitApplicationModule)
    )]
public class CmsApplicationModule : AbpModule
{
}
