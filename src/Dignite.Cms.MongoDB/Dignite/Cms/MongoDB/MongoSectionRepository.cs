using Dignite.Cms.Sections;
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
    public class MongoSectionRepository : MongoDbRepository<ICmsMongoDbContext, Section, Guid>, ISectionRepository
    {
        public MongoSectionRepository(IMongoDbContextProvider<ICmsMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
        }


        public virtual async Task<Section> GetDefaultAsync(Guid siteId, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await FindAsync(s => s.SiteId == siteId && s.IsActive && s.IsDefault && s.Type == SectionType.Single, includeDetails, cancellationToken);
        }

        public virtual async Task<bool> NameExistsAsync(Guid siteId, string name, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await (await GetMongoQueryableAsync(cancellationToken))
                .Where(s => s.SiteId == siteId)
                .AnyAsync(s => s.SiteId == siteId && s.Name == name, cancellationToken);
        }

        public virtual async Task<bool> RouteExistsAsync(Guid siteId, string route, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await (await GetMongoQueryableAsync(cancellationToken))
                .Where(s => s.SiteId == siteId)
                .AnyAsync(s => s.Route == route, cancellationToken);
        }

        public virtual async Task<Section> FindByNameAsync(Guid siteId, string name, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await FindAsync(s => s.SiteId == siteId && s.Name == name, includeDetails, cancellationToken);
        }

        public virtual async Task<int> GetCountAsync(
            Guid siteId,
            string filter = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default
            )
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            var query = await GetListQueryAsync(
                siteId, filter, isActive,
                cancellationToken);

            return await query.As<IMongoQueryable<Section>>().CountAsync(cancellationToken);
        }

        public virtual async Task<List<Section>> GetListAsync(
            Guid siteId,
            string filter = null,
            bool? isActive = null,
            bool includeDetails = true,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string sorting = null,
            CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);

            var query = await GetListQueryAsync(
                siteId, filter, isActive,
                cancellationToken);

            return await query.OrderBy(sorting.IsNullOrWhiteSpace() ? $"{nameof(Section.CreationTime)} asc" : sorting)
                      .As<IMongoQueryable<Section>>()
                      .PageBy<Section, IMongoQueryable<Section>>(skipCount, maxResultCount)
                      .ToListAsync(cancellationToken);
        }


        protected virtual async Task<IQueryable<Section>> GetListQueryAsync(
            Guid siteId,
            string filter = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            return (await GetMongoQueryableAsync(cancellationToken))
                .Where(s => s.SiteId == siteId)
                .WhereIf(!filter.IsNullOrEmpty(), et => et.DisplayName.Contains(filter))
                .WhereIf(isActive.HasValue, s => s.IsActive == isActive); 
        }
    }
}
