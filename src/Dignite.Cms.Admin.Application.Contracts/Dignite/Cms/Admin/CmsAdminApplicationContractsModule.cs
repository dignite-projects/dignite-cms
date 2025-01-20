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
        typeof(AbpRegionalizationManagementApplicationContractsModule),
        typeof(AbpDynamicFormsFileExplorerModule),
        typeof(AbpDynamicFormsCkEditorModule)
        )]
    public class CmsAdminApplicationContractsModule : AbpModule
    {

    }
}
