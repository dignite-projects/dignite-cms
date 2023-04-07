using Dignite.Cms.Admin;
using Dignite.Cms.Public;
using Volo.Abp.Modularity;
using Volo.CmsKit;

namespace Dignite.Cms;

[DependsOn(
    typeof(CmsAdminApplicationContractsModule),
    typeof(CmsPublicApplicationContractsModule),
    typeof(CmsKitApplicationContractsModule)
    )]
public class CmsApplicationContractsModule : AbpModule
{

}
