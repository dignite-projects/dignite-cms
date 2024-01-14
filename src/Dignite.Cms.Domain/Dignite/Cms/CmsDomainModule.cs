using Dignite.FileExplorer;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AutoMapper;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;
using Volo.CmsKit;

namespace Dignite.Cms;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(CmsDomainSharedModule),
    typeof(FileExplorerDomainModule),
    typeof(AbpAutoMapperModule),
    typeof(CmsKitDomainModule)
)]
public class CmsDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAutoMapperObjectMapper<CmsDomainModule>();

        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddProfile<CmsDomainMappingProfile>(validate: true);
        });

        Configure<AbpDistributedEntityEventOptions>(options =>
        {
            options.EtoMappings.Add<Entries.Entry, Entries.EntryEto>(typeof(CmsDomainModule));

            options.AutoEventSelectors.Add<Entries.Entry>();
        });

    }
}
