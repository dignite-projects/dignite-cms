using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Dignite.Cms.Sites
{
    public interface ISiteRepository : IBasicRepository<Site, Guid>
    {
        Task<bool> NameExistsAsync(string name, Guid? ignoredId = null, CancellationToken cancellationToken = default);

        Task<Site> FindByNameAsync(string name, CancellationToken cancellationToken = default);


        Task<List<Site>> GetListAsync(
            string filter = null,
             bool? isActive = null,
            CancellationToken cancellationToken = default);
    }
}
