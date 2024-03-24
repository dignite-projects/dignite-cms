using Dignite.Cms.Entries;
using Dignite.Cms.Fields;
using Dignite.Cms.Sections;
using Dignite.Cms.Sites;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using Volo.CmsKit.MongoDB;

namespace Dignite.Cms.MongoDB;

[DependsOn(
    typeof(CmsDomainModule),
    typeof(AbpMongoDbModule),
    typeof(CmsKitMongoDbModule)
    )]
public class CmsMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<CmsMongoDbContext>(options =>
        {
            options.AddRepository<Site, MongoSiteRepository>();
            options.AddRepository<Section, MongoSectionRepository>();
            options.AddRepository<EntryType, MongoEntryTypeRepository>();
            options.AddRepository<FieldGroup, MongoFieldGroupRepository>();
            options.AddRepository<Field, MongoFieldRepository>();
            options.AddRepository<Entry, MongoEntryRepository>();
        });
    }
}
