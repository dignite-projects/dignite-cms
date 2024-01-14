using Dignite.Abp.DynamicForms;
using Dignite.Cms.Localization;
using Dignite.FileExplorer;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit;

namespace Dignite.Cms;
[DependsOn(
    typeof(AbpValidationModule),
    typeof(DigniteAbpDynamicFormsModule),
    typeof(FileExplorerDomainSharedModule),
    typeof(CmsKitDomainSharedModule)
)]
public class CmsDomainSharedModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        CmsGlobalFeatureConfigurator.Configure();
    }

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
    }
}
