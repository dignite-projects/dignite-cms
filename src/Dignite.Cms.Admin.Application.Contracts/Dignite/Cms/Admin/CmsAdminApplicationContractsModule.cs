using Dignite.Abp.DynamicForms.CkEditor;
using Dignite.Abp.DynamicForms.FileExplorer;
using Dignite.FileExplorer;
using Volo.Abp.Modularity;
using Volo.CmsKit.Admin;

namespace Dignite.Cms.Admin
{
    [DependsOn(
        typeof(CmsCommonApplicationContractsModule),
        typeof(FileExplorerApplicationContractsModule),
        typeof(CmsKitAdminApplicationContractsModule),
        typeof(DigniteAbpDynamicFormsFileExplorerModule),
        typeof(DigniteAbpDynamicFormsCkEditorModule)
        )]
    public class CmsAdminApplicationContractsModule : AbpModule
    {

    }
}
