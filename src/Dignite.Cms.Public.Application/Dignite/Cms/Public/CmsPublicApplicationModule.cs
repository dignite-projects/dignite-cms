using Dignite.CmsKit.Public;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Dignite.Cms.Public;

[DependsOn(
    typeof(CmsDomainModule),
    typeof(CmsPublicApplicationContractsModule),
    typeof(DigniteCmsKitPublicApplicationModule)
    )]
public class CmsPublicApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<CmsPublicApplicationModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<CmsPublicApplicationModule>(validate: true);
        });
    }
}
