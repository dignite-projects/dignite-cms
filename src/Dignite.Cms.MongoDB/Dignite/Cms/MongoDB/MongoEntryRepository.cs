using Dignite.Abp.Data;
using Dignite.Abp.DynamicForms;
using Dignite.Cms.Entries;
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
    public class MongoEntryRepository : MongoDbRepository<ICmsMongoDbContext, Entry, Guid>, IEntryRepository
    {
        private readonly IFormControlSelector _formControlSelector;
        private readonly IEnumerable<IFieldQuerying> _fieldQueryings;

        public MongoEntryRepository(
            IFormControlSelector formControlSelector,
            IEnumerable<IFieldQuerying> fieldQueryings, 
            IMongoDbContextProvider<ICmsMongoDbContext> dbContextProvider)
            : base(dbContextProvider)
        {
            _formControlSelector = formControlSelector;
            _fieldQueryings = fieldQueryings;
        }


        public virtual async Task<bool> SlugExistsAsync(string culture, Guid sectionId, string slug, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await (await GetMongoQueryableAsync(cancellationToken))
                       .AnyAsync(e => e.Culture == culture 
                           && e.SectionId == sectionId 
                           && e.Slug == slug && e.IsActivatedVersion, 
                       cancellationToken);
        }


        public virtual async Task<List<Entry>> GetListAsync(
            string culture,
            Guid sectionId,
            Guid? entryTypeId = null,
            Guid? creatorId = null,
            EntryStatus? status = null,
            string filter = null,
            DateTime? start = null,
            DateTime? end = null,
            IList<QueryingByField> queryingByCustomFields = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string sorting = null,
            CancellationToken cancellationToken = default)
        {
                cancellationToken = GetCancellationToken(cancellationToken);
            if (queryingByCustomFields == null || !queryingByCustomFields.Any())
            {
                return await (
                    await GetListQueryAsync(culture, sectionId, entryTypeId, creatorId, status, filter, start, end, cancellationToken))
                    .OrderBy(sorting.IsNullOrEmpty() ? $"{nameof(Entry.PublishTime)} desc" : sorting)
                    .As<IMongoQueryable<Entry>>()
                    .PageBy<Entry, IMongoQueryable<Entry>>(skipCount, maxResultCount)
                    .ToListAsync(cancellationToken);
            }
            else
            {
                var enumerable = (await GetListQueryAsync(culture, sectionId, entryTypeId, creatorId, status, filter, start, end, cancellationToken))
                    .OrderByDescending(e => e.PublishTime)
                    .AsEnumerable();
                enumerable = await QueryingByFields(enumerable, queryingByCustomFields, cancellationToken);

                return enumerable.Skip(skipCount).Take(maxResultCount).ToList();
            }

        }

        public virtual async Task<int> GetCountAsync(
            string culture,
            Guid sectionId,
            Guid? entryTypeId = null,
            Guid? creatorId = null,
            EntryStatus? status = null,
            string filter = null,
            DateTime? start = null,
            DateTime? end = null,
            IList<QueryingByField> queryingByCustomFields = null,
            CancellationToken cancellationToken = default
            )
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            if (queryingByCustomFields == null || !queryingByCustomFields.Any())
            {
                return await (await GetListQueryAsync(culture, sectionId, entryTypeId, creatorId, status, filter, start, end, cancellationToken))
                    .As<IMongoQueryable<Entry>>()
                    .CountAsync(GetCancellationToken(cancellationToken));
            }
            else
            {
                var entryEnumerable = (await GetListQueryAsync(culture, sectionId, entryTypeId, creatorId, status, filter, start, end, cancellationToken))
                    .AsEnumerable();
                entryEnumerable = await QueryingByFields(entryEnumerable, queryingByCustomFields);

                return entryEnumerable.Count();
            }
        }

        public virtual async Task<List<Entry>> GetListAsync(Guid sectionId, IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await (await GetMongoQueryableAsync(cancellationToken))
                .Where(e => e.SectionId == sectionId && ids.Contains(e.Id))
                .ToListAsync(cancellationToken);
        }


        /// <summary>
        /// Get a list of revisions
        /// </summary>
        /// <param name="initialVersionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<List<Entry>> GetVisionListAsync(Guid initialVersionId, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            return await (await GetMongoQueryableAsync(cancellationToken))
                .Where(e => e.InitialVersionId == initialVersionId || e.Id == initialVersionId)
                .OrderByDescending(e => e.CreationTime)
                .ToListAsync(cancellationToken);
        }


        public virtual async Task<Entry> FindBySlugAsync(string culture, Guid sectionId, string slug, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            return await FindAsync(e => e.SectionId == sectionId && e.Culture == culture && e.Status == EntryStatus.Published && e.Slug == slug && e.IsActivatedVersion,
                includeDetails,
                GetCancellationToken(cancellationToken));
        }

        public virtual async Task<Entry> FindPrevAsync(Guid id, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            var currentEntry = await FindAsync(e => e.Id == id, false, GetCancellationToken(cancellationToken));

            return await (await GetMongoQueryableAsync(cancellationToken))
                    .Where(e => e.SectionId == currentEntry.SectionId && e.Culture == currentEntry.Culture && e.PublishTime < currentEntry.PublishTime && e.Status == EntryStatus.Published && e.IsActivatedVersion)
                    .OrderByDescending(e => e.CreationTime)
                    .FirstOrDefaultAsync(cancellationToken);

        }

        public virtual async Task<Entry> FindNextAsync(Guid id, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);
            var currentEntry = await FindAsync(e => e.Id == id, false, GetCancellationToken(cancellationToken));

            return await (await GetMongoQueryableAsync(cancellationToken))
                    .Where(e => e.SectionId == currentEntry.SectionId && e.Culture == currentEntry.Culture && e.PublishTime > currentEntry.PublishTime && e.Status == EntryStatus.Published && e.IsActivatedVersion)
                    .OrderBy(e => e.CreationTime)
                    .FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<int> GetMaxOrderAsync(string culture, Guid sectionId, Guid? parentId, CancellationToken cancellationToken = default)
        {
            cancellationToken = GetCancellationToken(cancellationToken);

            return await (await GetMongoQueryableAsync(cancellationToken))
                .Where(e => e.SectionId == sectionId && e.Culture == culture && e.ParentId == parentId)
                .MaxAsync(e => (int?)e.Order, cancellationToken) ?? 0;
        }

        public virtual async Task<bool> CultureExistWithSingleSectionAsync(string culture, Guid sectionId, Guid entryTypeId)
        {
            return await (await GetListQueryAsync(culture, sectionId, entryTypeId))
                .As<IMongoQueryable<Entry>>()
                .AnyAsync();
        }


        protected virtual async Task<IQueryable<Entry>> GetListQueryAsync(
            string culture,
            Guid sectionId,
            Guid? entryTypeId = null,
            Guid? creatorId = null,
            EntryStatus? status = null,
            string filter = null,
            DateTime? start = null,
            DateTime? end = null,
            CancellationToken cancellationToken = default)
        {
            return (await GetMongoQueryableAsync(cancellationToken))
                .Where(e => e.Culture == culture && e.SectionId == sectionId && e.IsActivatedVersion)
                .WhereIf(entryTypeId.HasValue, e => e.EntryTypeId == entryTypeId.Value)
                .WhereIf(creatorId.HasValue, e => e.CreatorId == creatorId.Value)
                .WhereIf(status.HasValue, e => e.Status == status.Value)
                .WhereIf(!filter.IsNullOrEmpty(), e => e.Title.Contains(filter))
                .WhereIf(start.HasValue, e => e.PublishTime >= start.Value)
                .WhereIf(end.HasValue, e => e.PublishTime < end.Value);
        }


        protected virtual async Task<IEnumerable<Entry>> QueryingByFields(
            IEnumerable<Entry> source, 
            IList<QueryingByField> queryingByCustomFields,
            CancellationToken cancellationToken = default)
        {
            var fieldQueryable = await GetMongoQueryableAsync<Field>(cancellationToken);
            var fields = await fieldQueryable
                .Where(f => queryingByCustomFields.Select(p => p.Name).Contains(f.Name))
                .ToListAsync();

            foreach (var param in queryingByCustomFields)
            {
                foreach (var querying in _fieldQueryings)
                {
                    var field = fields.FirstOrDefault(f => f.Name == param.Name);
                    if (field == null)
                        continue;

                    var form = _formControlSelector.Get(field.FormControlName);
                    if (form.GetType() == querying.FormControlType)
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
