using Volo.Abp.Data;
using Volo.Abp.MongoDB;
using Volo.CmsKit.MongoDB;

namespace Dignite.Cms.MongoDB;

[ConnectionStringName(CmsDbProperties.ConnectionStringName)]
public class CmsMongoDbContext : AbpMongoDbContext, ICmsMongoDbContext
{
    /* Add mongo collections here. Example:
     * public IMongoCollection<Question> Questions => Collection<Question>();
     */

    protected override void CreateModel(IMongoModelBuilder modelBuilder)
    {
        base.CreateModel(modelBuilder);

        modelBuilder.ConfigureCmsKit();
        modelBuilder.ConfigureCms();
    }
}
