using Volo.Abp.Modularity;
using Volo.Abp.Localization;
using Dignite.Cms.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
using Dignite.Abp.FieldCustomizing;
using Volo.CmsKit;
using Volo.Abp.GlobalFeatures;
using Dignite.FileExplorer;

namespace Dignite.Cms;
[DependsOn(
    typeof(AbpValidationModule),
    typeof(AbpFieldCustomizingDomainSharedModule),
    typeof(FileExplorerDomainSharedModule),
    typeof(CmsKitDomainSharedModule)
)]
public class CmsDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CmsDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<CmsResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Dignite/Cms/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Cms", typeof(CmsResource));
        });

        GlobalFeatureManager.Instance.Modules.CmsKit(cmsKit =>
        {
            cmsKit.User.Enable();
        });
    }
}
