using Dignite.Abp.RegionalizationManagement;
using Dignite.CmsKit.Admin;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Dignite.Cms.Admin
{
    [DependsOn(
        typeof(CmsAdminApplicationContractsModule),
        typeof(DigniteCmsKitAdminHttpApiClientModule),
        typeof(AbpRegionalizationManagementHttpApiClientModule))]
    public class CmsAdminHttpApiClientModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddHttpClientProxies(
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
