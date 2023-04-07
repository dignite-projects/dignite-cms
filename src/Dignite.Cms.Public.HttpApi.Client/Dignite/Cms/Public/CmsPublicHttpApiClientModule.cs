using Dignite.FileExplorer;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit.Public;

namespace Dignite.Cms.Public;

[DependsOn(
    typeof(CmsPublicApplicationContractsModule),
    typeof(AbpHttpClientModule),
    typeof(FileExplorerHttpApiClientModule),
    typeof(CmsKitPublicHttpApiClientModule))]
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
