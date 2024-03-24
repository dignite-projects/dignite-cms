using Dignite.Cms.Sites;
using Xunit;

namespace Dignite.Cms.MongoDB.Sites;

[Collection(MongoTestCollection.Name)]
public class SiteRepository_Tests : SiteRepository_Tests<CmsMongoDbTestModule>
{
    /* Don't write custom repository tests here, instead write to
     * the base class.
     * One exception can be some specific tests related to MongoDB.
     */
}
