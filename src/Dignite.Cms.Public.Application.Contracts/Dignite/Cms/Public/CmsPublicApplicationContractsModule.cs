using Dignite.Abp.DynamicForms.FileExplorer;
using Dignite.CmsKit.Public;
using Volo.Abp.Modularity;

namespace Dignite.Cms.Public;

[DependsOn(
    typeof(CmsCommonApplicationContractsModule),
    typeof(DigniteAbpDynamicFormsFileExplorerModule),
    typeof(DigniteCmsKitPublicApplicationContractsModule)
    )]
public class CmsPublicApplicationContractsModule : AbpModule
{

}
