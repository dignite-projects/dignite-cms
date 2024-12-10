using Dignite.CmsKit;
using Volo.Abp.Modularity;

namespace Dignite.Cms;

[DependsOn(
    typeof(CmsDomainSharedModule),
    typeof(DigniteCmsKitDomainModule)
)]
public class CmsDomainModule : AbpModule
{
}
