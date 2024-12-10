using Dignite.Cms.Admin;
using Dignite.Cms.Public;
using Dignite.CmsKit;
using Volo.Abp.Modularity;

namespace Dignite.Cms;

[DependsOn(
    typeof(CmsAdminApplicationContractsModule),
    typeof(CmsPublicApplicationContractsModule),
    typeof(DigniteCmsKitApplicationContractsModule)
    )]
public class CmsApplicationContractsModule : AbpModule
{

}
