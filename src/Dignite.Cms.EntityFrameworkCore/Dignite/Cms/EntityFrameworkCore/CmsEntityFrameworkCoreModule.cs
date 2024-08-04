using Dignite.FileExplorer.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;
using Volo.CmsKit.EntityFrameworkCore;

namespace Dignite.Cms.EntityFrameworkCore;

[DependsOn(
    typeof(CmsDomainModule),
    typeof(AbpEntityFrameworkCoreModule),
    typeof(FileExplorerEntityFrameworkCoreModule),
    typeof(CmsKitEntityFrameworkCoreModule)
)]
public class CmsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<CmsDbContext>(options =>
        {
            options.AddRepository<Domains.Domain, Domains.EfCoreDomainRepository>();
            options.AddRepository<Sites.Site, Sites.EfCoreSiteRepository>();
            options.AddRepository<Sections.Section, Sections.EfCoreSectionRepository>();
            options.AddRepository<Sections.EntryType, Sections.EfCoreEntryTypeRepository>();
            options.AddRepository<Fields.FieldGroup, Fields.EfCoreFieldGroupRepository>();
            options.AddRepository<Fields.Field, Fields.EfCoreFieldRepository>();
            options.AddRepository<Entries.Entry, Entries.EfCoreEntryRepository>();
        });
    }
}
