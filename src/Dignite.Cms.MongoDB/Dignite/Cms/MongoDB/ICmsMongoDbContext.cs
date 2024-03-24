using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Dignite.Cms.MongoDB;

[ConnectionStringName(CmsDbProperties.ConnectionStringName)]
public interface ICmsMongoDbContext : IAbpMongoDbContext
{
    IMongoCollection<Sites.Site> Sites { get; }
    IMongoCollection<Sections.Section> Sections { get; }
    IMongoCollection<Sections.EntryType> EntryTypes { get; }
    IMongoCollection<Fields.FieldGroup> FieldGroups { get; }
    IMongoCollection<Fields.Field> Fields { get; }
    IMongoCollection<Entries.Entry> Entries { get; }
}
