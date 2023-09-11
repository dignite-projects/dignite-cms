using Dignite.Abp.DynamicForms;
using Dignite.Abp.FieldCustomizing;
using Dignite.Abp.FieldCustomizing.EntityFrameworkCore.QueryingByFields;
using Dignite.Cms.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Dignite.Cms.Entries
{
    public class EfCoreEntryRepository : EfCoreRepository<ICmsDbContext, Entry,Guid>, IEntryRepository
    {
        private readonly IFormSelector _formSelector;
        private readonly IEnumerable<IFieldQuerying> _fieldQueryings;

        public EfCoreEntryRepository(
            IFormSelector formSelector,
            IEnumerable<IFieldQuerying> fieldQueryings,
            IDbContextProvider<ICmsDbContext> dbContextProvider
            )
            : base(dbContextProvider)
        {
            _formSelector = formSelector;
            _fieldQueryings = fieldQueryings;
        }


        public async Task<bool> SlugExistsAsync(Guid sectionId, string region, string slug, Guid? ignoredId = null, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                       .WhereIf(ignoredId != null, ct => ct.Id != ignoredId)
                       .AnyAsync(e => e.SectionId== sectionId && e.Region==region && e.Slug == slug && e.Revision.IsActive, GetCancellationToken(cancellationToken));
        }

        public async Task<bool> AnyAsync(Guid sectionId, string region, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                       .AnyAsync(e => e.SectionId == sectionId && e.Region == region, GetCancellationToken(cancellationToken));
        }


        public async Task<List<Entry>> GetListAsync(
            Guid sectionId,
            string region,
            Guid? creatorId = null,
            EntryStatus? status = null,
            string filter = null,
            DateTime? start = null,
            DateTime? end = null,
            IList<QueryingByFieldParameter> queryingByFieldParameters = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string sorting = null,
            CancellationToken cancellationToken = default)
        {
            if (queryingByFieldParameters == null || !queryingByFieldParameters.Any())
            {
                return await (await GetQueryableAsync(sectionId, region, creatorId, status, filter, start, end))
                    .OrderBy(sorting.IsNullOrEmpty() ? $"{nameof(Entry.PublishTime)} desc" : sorting)
                    .PageBy(skipCount, maxResultCount)
                    .ToListAsync(GetCancellationToken(cancellationToken));
            }
            else
            {                
                var enumerable = (await GetQueryableAsync(sectionId, region, creatorId, status, filter, start, end))
                    .OrderByDescending(e => e.PublishTime).AsEnumerable<Entry>();
                enumerable = await QueryingByFieldParameters(enumerable, queryingByFieldParameters);

                return enumerable.Skip(skipCount).Take(maxResultCount).ToList();
            }

        }

        public async Task<int> GetCountAsync(
            Guid sectionId,
            string region,
            Guid? creatorId = null,
            EntryStatus? status = null,
            string filter = null,
            DateTime? start = null,
            DateTime? end = null,
            IList<QueryingByFieldParameter> queryingByFieldParameters = null,
            CancellationToken cancellationToken = default
            )
        {
            if (queryingByFieldParameters == null || !queryingByFieldParameters.Any())
            {
                return await (await GetQueryableAsync(sectionId, region, creatorId, status, filter, start, end))
                .CountAsync(GetCancellationToken(cancellationToken));
            }
            else
            {
                var enumerable = (await GetQueryableAsync(sectionId, region, creatorId, status, filter, start, end))
                    .AsEnumerable<Entry>();
                enumerable = await QueryingByFieldParameters(enumerable, queryingByFieldParameters);

                return enumerable.Count();
            }
        }

        public async Task<List<Entry>> GetListAsync(Guid sectionId, IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(e => e.SectionId == sectionId && ids.Contains(e.Id))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }


        /// <summary>
        /// Get a list of revisions
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<Entry>> GetRevisionListAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .Where(e => e.Revision.InitialId==id)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="version"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Entry> FindByVersionAsync(Guid id, int version, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .IncludeDetails(includeDetails)
                .Where(e => e.Revision.InitialId == id && e.Revision.Version==version)
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<Entry> FindBySlugAsync(Guid sectionId, string region, string slug, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .FirstOrDefaultAsync(e => e.SectionId == sectionId && e.Region==region && e.Status== EntryStatus.Published && e.Slug == slug && e.Revision.IsActive, GetCancellationToken(cancellationToken));
        }

        public async Task<Entry> FindPrevAsync(Guid id, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            var currentEntry = await dbSet.FirstAsync(e => e.Id == id, GetCancellationToken(cancellationToken));
            return await dbSet
                    .Where(e => e.SectionId == currentEntry.SectionId && e.Region==currentEntry.Region && e.PublishTime < currentEntry.PublishTime && e.Status == EntryStatus.Published && e.Revision.IsActive)
                    .OrderByDescending(e => e.CreationTime)
                    .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));           

        }

        public async Task<Entry> FindNextAsync(Guid id, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            var dbSet = await GetDbSetAsync();
            var currentEntry = await dbSet.FirstAsync(e => e.Id == id, GetCancellationToken(cancellationToken));
            return await dbSet
                    .Where(e => e.SectionId == currentEntry.SectionId && e.Region == currentEntry.Region && e.PublishTime > currentEntry.PublishTime && e.Status == EntryStatus.Published && e.Revision.IsActive)
                    .OrderBy(e => e.CreationTime)
                    .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<int> GetMaxOrderAsync(Guid sectionId, string region, Guid? parentId, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).Where(e => e.SectionId == sectionId && e.Region == region && e.ParentId==parentId)
                .MaxAsync(e=>(int?)e.Order, GetCancellationToken(cancellationToken))??0;
        }

        public override async Task<IQueryable<Entry>> WithDetailsAsync()
        {
            return (await GetQueryableAsync()).IncludeDetails();
        }


        protected virtual async Task<IQueryable<Entry>> GetQueryableAsync(
             Guid sectionId,
             string region,
             Guid? creatorId = null,
             EntryStatus? status = null,
             string filter = null,
            DateTime? start = null,
            DateTime? end = null)
        {
            return (await GetDbSetAsync()).Where(e => e.SectionId == sectionId && e.Region == region && e.Revision.IsActive)
                .WhereIf(creatorId.HasValue, e => e.CreatorId == creatorId.Value)
                .WhereIf(status.HasValue, e => e.Status == status.Value)
                .WhereIf(!filter.IsNullOrEmpty(), e => e.Title.Contains(filter))
                .WhereIf(start.HasValue, e => e.PublishTime>=start.Value)
                .WhereIf(end.HasValue, e => e.PublishTime<end.Value);
        }

        protected virtual async Task<IEnumerable<Entry>> QueryingByFieldParameters(IEnumerable<Entry> source, IList<QueryingByFieldParameter> queryingByFieldParameters)
        {
            var dbContext = await GetDbContextAsync();
            var fields = await dbContext.Fields
                .Where(f => queryingByFieldParameters.Select(p => p.FieldName).Contains(f.Name))
                .ToListAsync();

            foreach (var param in queryingByFieldParameters)
            {
                foreach (var querying in _fieldQueryings)
                {
                    var field = fields.FirstOrDefault(f => f.Name == param.FieldName);
                    if (field == null)
                        continue;

                    var form = _formSelector.Get(field.FormName);
                    if (form.GetType() == querying.FormType)
                    {
                        source = querying.Query(source, param);
                        continue;
                    }
                }
            }

            return source;
        }
    }
}
