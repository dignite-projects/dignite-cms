using Dignite.Abp.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Dignite.Cms.Entries
{
    public interface IEntryRepository : IBasicRepository<Entry, Guid>
    {
        Task<bool> SlugExistsAsync( Guid sectionId,string culture, string slug, Guid? ignoredId = null, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(Guid sectionId, string culture, CancellationToken cancellationToken = default);

        Task<List<Entry>> GetListAsync(
            Guid sectionId, 
            string culture,
            Guid? creatorId = null, 
            EntryStatus? auditedStatus = null,  
            string filter=null,
            DateTime? start=null,
            DateTime? end = null,
            IList<QueryingByCustomField> queryingByCustomFields = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string sorting = null,
            CancellationToken cancellationToken = default);
        Task<int> GetCountAsync(
             Guid sectionId,
             string culture,
             Guid? creatorId = null,
             EntryStatus? auditedStatus = null,
             string filter = null,
            DateTime? start = null,
            DateTime? end = null,
            IList<QueryingByCustomField> queryingByCustomFields = null,
            CancellationToken cancellationToken = default
            );

        Task<List<Entry>> GetListAsync(Guid sectionId, IEnumerable<Guid> ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of revisions
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Entry>> GetAllVisionListAsync(Entry entry, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get entries using slug
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// This method gets the active revision
        /// </returns>
        Task<Entry> FindBySlugAsync(Guid sectionId, string culture, string slug, bool includeDetails = true, CancellationToken cancellationToken = default);

        Task<Entry> FindPrevAsync(Guid id, bool includeDetails = false, CancellationToken cancellationToken = default);

        Task<Entry> FindNextAsync(Guid id, bool includeDetails = false, CancellationToken cancellationToken = default);

        Task<int> GetMaxOrderAsync(Guid sectionId, string culture, Guid? parentId, CancellationToken cancellationToken = default);
    }
}
