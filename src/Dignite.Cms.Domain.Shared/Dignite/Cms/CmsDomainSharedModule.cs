using Dignite.Abp.DynamicForms;
using Dignite.Abp.DynamicForms.Localization;
using Dignite.Cms.Localization;
using Dignite.CmsKit;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Dignite.Cms;
[DependsOn(
    typeof(AbpDynamicFormsModule),
    typeof(DigniteCmsKitDomainSharedModule)
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
                .AddBaseTypes(typeof(AbpDynamicFormsResource))
                .AddVirtualJson("/Dignite/Cms/Localization/Resources");
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace("Cms", typeof(CmsResource));
        });
    }
}
