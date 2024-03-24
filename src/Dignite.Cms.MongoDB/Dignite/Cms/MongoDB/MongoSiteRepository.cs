using Dignite.Cms.Sites;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.MongoDB;
using Volo.Abp.MongoDB;

namespace Dignite.Cms.MongoDB
{
    public class MongoSiteRepository : MongoDbRepository<ICmsMongoDbContext, Site, Guid>, ISiteRepository
    {
        public MongoSiteRepository(IMongoDbContextProvider<ICmsMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }

        public virtual async Task<Site> FindByHostAsync(string host, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await (await GetMongoQueryableAsync(cancellationToken))
                .FirstOrDefaultAsync(s => s.Host == host, cancellationToken);
        }

        public virtual async Task<Site> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await (await GetMongoQueryableAsync(cancellationToken))
                .FirstOrDefaultAsync(s => s.Name == name, cancellationToken);
        }

        public virtual async Task<List<Site>> GetListAsync(string filter = null, bool? isActive = null, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await (await GetMongoQueryableAsync(cancellationToken))
                    .WhereIf(!filter.IsNullOrWhiteSpace(), s => s.DisplayName.Contains(filter) || s.Name.Contains(filter))
                    .WhereIf(isActive.HasValue, s => s.IsActive == isActive)
                    .As<IMongoQueryable<Site>>()
                    .OrderByDescending(s => s.IsActive)
                    .ThenBy(s => s.CreationTime)
                    .ToListAsync(cancellationToken);
        }

        public virtual async Task<bool> HostExistsAsync(string host, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await (await GetMongoQueryableAsync(cancellationToken))
                       .AnyAsync(s => s.Host == host, cancellationToken);
        }

        public virtual async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await (await GetMongoQueryableAsync(cancellationToken))
                       .AnyAsync(s => s.Name == name, cancellationToken);
        }
    }
}
