using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Dignite.Cms.MongoDB;

public static class CmsMongoDbContextExtensions
{
    public static void ConfigureCms(
        this IMongoModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));
    }
}
