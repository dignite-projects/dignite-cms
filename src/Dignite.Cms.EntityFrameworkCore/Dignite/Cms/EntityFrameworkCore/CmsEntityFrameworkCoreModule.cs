using Dignite.CmsKit.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Dignite.Cms.EntityFrameworkCore;

[DependsOn(
    typeof(CmsDomainModule),
    typeof(DigniteCmsKitEntityFrameworkCoreModule)
)]
public class CmsEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<CmsDbContext>(options =>
        {
            options.AddRepository<Sections.Section, Sections.EfCoreSectionRepository>();
            options.AddRepository<Sections.EntryType, Sections.EfCoreEntryTypeRepository>();
            options.AddRepository<Fields.FieldGroup, Fields.EfCoreFieldGroupRepository>();
            options.AddRepository<Fields.Field, Fields.EfCoreFieldRepository>();
            options.AddRepository<Entries.Entry, Entries.EfCoreEntryRepository>();
        });
    }
}
