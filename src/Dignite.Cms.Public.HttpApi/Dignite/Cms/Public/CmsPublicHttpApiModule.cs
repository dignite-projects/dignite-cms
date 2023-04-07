using Localization.Resources.AbpUi;
using Dignite.Cms.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Dignite.FileExplorer;
using Volo.CmsKit.Public;

namespace Dignite.Cms.Public;

[DependsOn(
    typeof(CmsPublicApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(FileExplorerHttpApiModule),
    typeof(CmsKitPublicHttpApiModule))]
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
