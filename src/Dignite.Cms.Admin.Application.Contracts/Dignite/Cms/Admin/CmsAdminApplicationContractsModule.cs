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
        typeof(AbpDynamicFormsFileExplorerModule),
        typeof(AbpDynamicFormsCkEditorModule)
        )]
    public class CmsAdminApplicationContractsModule : AbpModule
    {

    }
}
