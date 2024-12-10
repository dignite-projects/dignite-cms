using Dignite.Cms.Localization;
using Dignite.CmsKit.Public;
using Localization.Resources.AbpUi;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;

namespace Dignite.Cms.Public;

[DependsOn(
    typeof(CmsPublicApplicationContractsModule),
    typeof(DigniteCmsKitPublicHttpApiModule))]
public class CmsPublicHttpApiModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CmsPublicHttpApiModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Get<CmsResource>()
                .AddBaseTypes(typeof(AbpUiResource));
        });
    }
}
