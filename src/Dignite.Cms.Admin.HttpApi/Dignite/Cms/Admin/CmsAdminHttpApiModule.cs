using Localization.Resources.AbpUi;
using Dignite.Cms.Localization;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Microsoft.Extensions.DependencyInjection;
using Dignite.FileExplorer;
using Volo.CmsKit.Admin;

namespace Dignite.Cms.Admin
{
    [DependsOn(
        typeof(CmsAdminApplicationContractsModule),
        typeof(AbpAspNetCoreMvcModule),
        typeof(FileExplorerHttpApiModule),
        typeof(CmsKitAdminHttpApiModule))]
    public class CmsAdminHttpApiModule : AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            PreConfigure<IMvcBuilder>(mvcBuilder =>
            {
                mvcBuilder.AddApplicationPartIfNotExists(typeof(CmsAdminHttpApiModule).Assembly);
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
}
