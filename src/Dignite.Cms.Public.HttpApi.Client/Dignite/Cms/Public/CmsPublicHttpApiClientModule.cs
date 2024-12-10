using Dignite.CmsKit.Public;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Dignite.Cms.Public;

[DependsOn(
    typeof(CmsPublicApplicationContractsModule),
    typeof(DigniteCmsKitPublicHttpApiClientModule))]
public class CmsPublicHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(CmsPublicApplicationContractsModule).Assembly,
            CmsPublicRemoteServiceConsts.RemoteServiceName
        );

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CmsPublicHttpApiClientModule>();
        });
    }
}
