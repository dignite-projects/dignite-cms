using Dignite.Abp.FieldCustomizing;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Dignite.Cms.Entries
{
    public interface IEntryRepository : IBasicRepository<Entry, Guid>
    {
        Task<bool> SlugExistsAsync( Guid sectionId,string language, string slug, Guid? ignoredId = null, CancellationToken cancellationToken = default);

        Task<bool> AnyAsync(Guid sectionId, string language, CancellationToken cancellationToken = default);

        Task<List<Entry>> GetListAsync(
            Guid sectionId, 
            string language,
            Guid? creatorId = null, 
            EntryStatus? auditedStatus = null,  
            string filter=null,
            DateTime? start=null,
            DateTime? end = null,
            IList<QueryingByFieldParameter> queryingByFieldParameters = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string sorting = null,
            CancellationToken cancellationToken = default);
        Task<int> GetCountAsync(
             Guid sectionId,
             string language,
             Guid? creatorId = null,
             EntryStatus? auditedStatus = null,
             string filter = null,
            DateTime? start = null,
            DateTime? end = null,
            IList<QueryingByFieldParameter> queryingByFieldParameters = null,
            CancellationToken cancellationToken = default
            );

        Task<List<Entry>> GetListAsync(Guid sectionId, IEnumerable<Guid> ids, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a list of revisions
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Entry>> GetRevisionListAsync(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="version"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Entry> FindByVersionAsync(Guid id, int version, bool includeDetails = true, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get entries using slug
        /// </summary>
        /// <param name="sectionId"></param>
        /// <param name="slug"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// This method gets the active revision
        /// </returns>
        Task<Entry> FindBySlugAsync(Guid sectionId, string language, string slug, bool includeDetails = true, CancellationToken cancellationToken = default);

        Task<Entry> FindPrevAsync(Guid id, bool includeDetails = false, CancellationToken cancellationToken = default);

        Task<Entry> FindNextAsync(Guid id, bool includeDetails = false, CancellationToken cancellationToken = default);

        Task<int> GetMaxOrderAsync(Guid sectionId, string language, Guid? parentId, CancellationToken cancellationToken = default);
    }
}
