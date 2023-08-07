using Dignite.Abp.FieldCustomizing;
using Dignite.Cms.Entries;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Dignite.Cms.Sections
{
    public interface ISectionRepository : IBasicRepository<Section, Guid>
    {
        Task<bool> NameExistsAsync(Guid siteId, string name, CancellationToken cancellationToken = default);
        Task<bool> RouteExistsAsync(Guid siteId, string route, CancellationToken cancellationToken = default);

        Task<Section> FindByNameAsync(Guid siteId, string name, bool includeDetails = true, CancellationToken cancellationToken = default);

        Task<Section> GetDefaultAsync(Guid siteId, bool includeDetails = true, CancellationToken cancellationToken = default);

        Task<int> GetCountAsync(
            Guid? siteId,
            string filter = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default
            );
        Task<List<Section>> GetListAsync(
            Guid? siteId,
            string filter = null,
            bool? isActive= null,
            bool includeDetails = true,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string sorting = null,
            CancellationToken cancellationToken = default);
    }
}
