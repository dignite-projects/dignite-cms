using Dignite.Cms.Entries;
using Xunit;

namespace Dignite.Cms.MongoDB.Entries;

[Collection(MongoTestCollection.Name)]
public class EntryRepository_Tests : EntryRepository_Tests<CmsMongoDbTestModule>
{
    /* Don't write custom repository tests here, instead write to
     * the base class.
     * One exception can be some specific tests related to MongoDB.
     */
}
