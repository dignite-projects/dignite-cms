using Dignite.Cms.Entries;
using Dignite.Cms.Fields;
using Dignite.Cms.Sections;
using Dignite.Cms.Sites;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Dignite.Cms.MongoDB;

public static class CmsMongoDbContextExtensions
{
    public static void ConfigureCms(this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        builder.Entity<Site>(b =>
        {
            b.CollectionName = CmsDbProperties.DbTablePrefix + "Sites";
        });

        builder.Entity<Section>(b =>
        {
            b.CollectionName = CmsDbProperties.DbTablePrefix + "Sections";
        });

        builder.Entity<EntryType>(b =>
        {
            b.CollectionName = CmsDbProperties.DbTablePrefix + "EntryTypes";
        });

        builder.Entity<FieldGroup>(b =>
        {
            b.CollectionName = CmsDbProperties.DbTablePrefix + "FieldGroups";
        });

        builder.Entity<Field>(b =>
        {
            b.CollectionName = CmsDbProperties.DbTablePrefix + "Fields";
        });

        builder.Entity<Entry>(b =>
        {
            b.CollectionName = CmsDbProperties.DbTablePrefix + "Entries";
        });
    }
}
