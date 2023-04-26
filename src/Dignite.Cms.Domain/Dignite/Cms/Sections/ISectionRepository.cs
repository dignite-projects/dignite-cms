using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Dignite.Cms.Sections
{
    public interface ISectionRepository : IBasicRepository<Section, Guid>
    {
        Task<bool> NameExistsAsync(Guid siteId, string name, Guid? ignoredId = null, CancellationToken cancellationToken = default);

        Task<Section> FindByNameAsync(Guid siteId, string name, bool includeDetails = true, CancellationToken cancellationToken = default);

        Task<Section> GetDefaultAsync(Guid siteId, bool includeDetails = true, CancellationToken cancellationToken = default);

        Task<List<Section>> GetListAsync(
            Guid? siteId,
            string filter = null,
            bool? isActive= null,
            bool includeDetails = true,
            CancellationToken cancellationToken = default);
    }
}
