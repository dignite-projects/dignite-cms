using Dignite.FileExplorer;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.CmsKit.Public;

namespace Dignite.Cms.Public;

[DependsOn(
    typeof(CmsDomainModule),
    typeof(CmsPublicApplicationContractsModule),
    typeof(AbpDddApplicationModule),
    typeof(AbpAutoMapperModule),
    typeof(FileExplorerApplicationModule),
    typeof(CmsKitPublicApplicationModule)
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
