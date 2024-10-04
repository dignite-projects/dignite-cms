using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Dignite.Cms.Domains
{
    public interface IDomainRepository : IBasicRepository<Domain, Guid>
    {
        Task<bool> NameExistsAsync(string domainName, CancellationToken cancellationToken = default);

        Task<Domain> FindByNameAsync(string domainName, CancellationToken cancellationToken = default);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<Domain> FindByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default);
    }
}
