using Dignite.Abp.DynamicForms.CkEditor;
using Dignite.Abp.DynamicForms.FileExplorer;
using Dignite.CmsKit.Admin;
using Volo.Abp.Modularity;

namespace Dignite.Cms.Admin
{
    [DependsOn(
        typeof(CmsCommonApplicationContractsModule),
        typeof(DigniteCmsKitAdminApplicationContractsModule),
        typeof(DigniteAbpDynamicFormsFileExplorerModule),
        typeof(DigniteAbpDynamicFormsCkEditorModule)
        )]
    public class CmsAdminApplicationContractsModule : AbpModule
    {

    }
}
