using Dignite.Abp.DynamicForms.CkEditor;
using Dignite.Abp.DynamicForms.FileExplorer;
using Dignite.Abp.RegionalizationManagement;
using Dignite.CmsKit.Admin;
using Volo.Abp.Modularity;

namespace Dignite.Cms.Admin
{
    [DependsOn(
        typeof(CmsCommonApplicationContractsModule),
        typeof(DigniteCmsKitAdminApplicationContractsModule),
        typeof(RegionalizationManagementApplicationContractsModule),
        typeof(DigniteAbpDynamicFormsFileExplorerModule),
        typeof(DigniteAbpDynamicFormsCkEditorModule)
        )]
    public class CmsAdminApplicationContractsModule : AbpModule
    {

    }
}
