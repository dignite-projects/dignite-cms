using Dignite.Cms.Entries;
using Dignite.Cms.Fields;
using Dignite.Cms.Sections;
using Dignite.Cms.Sites;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.CmsKit.MongoDB;

namespace Dignite.Cms.MongoDB;

[ConnectionStringName(CmsDbProperties.ConnectionStringName)]
public class CmsMongoDbContext : AbpMongoDbContext, ICmsMongoDbContext
{
    public IMongoCollection<Site> Sites => Collection<Site>();
    public IMongoCollection<Section> Sections => Collection<Section>();
    public IMongoCollection<EntryType> EntryTypes => Collection<EntryType>();
    public IMongoCollection<FieldGroup> FieldGroups => Collection<FieldGroup>();
    public IMongoCollection<Field> Fields => Collection<Field>();
    public IMongoCollection<Entry> Entries => Collection<Entry>();

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureCmsKit();
        modelBuilder.ConfigureCms();
    }
}
