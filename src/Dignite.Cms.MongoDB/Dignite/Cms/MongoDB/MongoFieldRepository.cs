using Dignite.Cms.Fields;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Dignite.Cms.MongoDB
{
    public class MongoFieldRepository : MongoDbRepository<ICmsMongoDbContext, Field, Guid>, IFieldRepository
    {
        public MongoFieldRepository(IMongoDbContextProvider<ICmsMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<Field> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await FindAsync(s => s.Name == name,false, cancellationToken);
        }

        public virtual async Task<int> GetCountAsync(Guid? groupId, string filter, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);

            return await (
                await GetListQueryAsync(
                    groupId, filter, cancellationToken
                ))
                .As<IMongoQueryable<Field>>().CountAsync(cancellationToken);
        }

        public virtual async Task<List<Field>> GetListAsync(Guid? groupId, string filter, int maxResultCount = int.MaxValue, int skipCount = 0, string sorting = null, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);

            return await (
                await GetListQueryAsync(
                    groupId, filter,cancellationToken
                ))
                .OrderBy(sorting.IsNullOrWhiteSpace() ? $"{nameof(Field.CreationTime)} asc" : sorting)
                .As<IMongoQueryable<Field>>()
                .PageBy<Field, IMongoQueryable<Field>>(skipCount, maxResultCount)
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<List<Field>> GetListAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await (await GetMongoQueryableAsync(cancellationToken))
                .Where(f => ids.Contains(f.Id))
                .ToListAsync(cancellationToken);
        }

        public virtual async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await (await GetMongoQueryableAsync(cancellationToken))
                .AnyAsync(f => f.Name == name, cancellationToken);
        }


        protected virtual async Task<IQueryable<Field>> GetListQueryAsync(
            Guid? groupId, string filter,
            CancellationToken cancellationToken = default)
        {
            return (await GetMongoQueryableAsync(cancellationToken))
                .WhereIf(groupId.HasValue, f => f.GroupId == groupId.Value)
                .WhereIf(!filter.IsNullOrEmpty(), f => f.Name.Contains(filter) || f.DisplayName.Contains(filter));
        }
    }
}
