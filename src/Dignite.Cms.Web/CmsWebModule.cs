using Dignite.Cms.Public.Web;
using Volo.Abp.Modularity;
using Volo.CmsKit.Web;

namespace Dignite.Cms.Web;

[DependsOn(
    typeof(CmsApplicationContractsModule),
    typeof(CmsPublicWebModule),
    typeof(CmsKitWebModule)
    )]
public class CmsWebModule : AbpModule
{
}
