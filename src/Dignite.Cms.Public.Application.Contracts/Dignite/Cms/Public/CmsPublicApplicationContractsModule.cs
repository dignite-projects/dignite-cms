using Dignite.Abp.DynamicForms.FileExplorer;
using Dignite.FileExplorer;
using Volo.Abp.Modularity;
using Volo.CmsKit.Public;

namespace Dignite.Cms.Public;

[DependsOn(
    typeof(CmsCommonApplicationContractsModule),
    typeof(FileExplorerApplicationContractsModule),
    typeof(DigniteAbpDynamicFormsFileExplorerModule),
    typeof(CmsKitPublicApplicationContractsModule)
    )]
public class CmsPublicApplicationContractsModule : AbpModule
{

}
