using Dignite.Cms.Sections;
using Xunit;

namespace Dignite.Cms.MongoDB.Sections;

[Collection(MongoTestCollection.Name)]
public class SectionRepository_Tests : SectionRepository_Tests<CmsMongoDbTestModule>
{
    /* Don't write custom repository tests here, instead write to
     * the base class.
     * One exception can be some specific tests related to MongoDB.
     */
}
