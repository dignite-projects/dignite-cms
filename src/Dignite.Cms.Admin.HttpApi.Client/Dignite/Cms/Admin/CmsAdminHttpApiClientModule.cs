using Dignite.FileExplorer;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;
using Volo.CmsKit.Admin;

namespace Dignite.Cms.Admin
{
    [DependsOn(
        typeof(CmsAdminApplicationContractsModule),
        typeof(AbpHttpClientModule),
        typeof(FileExplorerHttpApiClientModule),
        typeof(CmsKitAdminHttpApiClientModule))]
    public class CmsAdminHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddStaticHttpClientProxies(
                typeof(CmsAdminApplicationContractsModule).Assembly,
                CmsAdminRemoteServiceConsts.RemoteServiceName
            );

            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<CmsAdminHttpApiClientModule>();
            });

        }
    }
}
