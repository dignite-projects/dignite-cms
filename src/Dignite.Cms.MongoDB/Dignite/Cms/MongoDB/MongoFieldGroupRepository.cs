using Dignite.Cms.Fields;
using System;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Dignite.Cms.MongoDB
{
    public class MongoFieldGroupRepository : MongoDbRepository<ICmsMongoDbContext, FieldGroup, Guid>, IFieldGroupRepository
    {
        public MongoFieldGroupRepository(IMongoDbContextProvider<ICmsMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }
    }
}
