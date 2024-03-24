using Dignite.Cms.Fields;
using Xunit;

namespace Dignite.Cms.MongoDB.Fields;

[Collection(MongoTestCollection.Name)]
public class FieldRepository_Tests : FieldRepository_Tests<CmsMongoDbTestModule>
{
    /* Don't write custom repository tests here, instead write to
     * the base class.
     * One exception can be some specific tests related to MongoDB.
     */
}
