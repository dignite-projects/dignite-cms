using Dignite.Cms.Sections;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Dignite.Cms.MongoDB
{
    public class MongoEntryTypeRepository : MongoDbRepository<ICmsMongoDbContext, EntryType, Guid>, IEntryTypeRepository
    {
        public MongoEntryTypeRepository(IMongoDbContextProvider<ICmsMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<bool> NameExistsAsync(Guid sectionId, string name, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await (await GetMongoQueryableAsync(cancellationToken))
                .Where(s => s.SectionId == sectionId)
                .AnyAsync(s => s.Name == name, cancellationToken);
        }
    }
}
