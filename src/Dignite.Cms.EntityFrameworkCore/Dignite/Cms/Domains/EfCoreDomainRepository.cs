using Dignite.Cms.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Dignite.Cms.Domains
{
    public class EfCoreDomainRepository : EfCoreRepository<ICmsDbContext, Domain, Guid>, IDomainRepository
    {
        public EfCoreDomainRepository(
            IDbContextProvider<ICmsDbContext> dbContextProvider
            )
            : base(dbContextProvider)
        {
        }

        public async Task<Domain> FindByNameAsync(string domainName, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).FirstOrDefaultAsync(s => s.DomainName == domainName, GetCancellationToken(cancellationToken));
        }

        public async Task<Domain> FindByTenantIdAsync(Guid tenantId, CancellationToken cancellationToken = default)
        {
            return await(await GetDbSetAsync()).FirstOrDefaultAsync(s => s.TenantId == tenantId, GetCancellationToken(cancellationToken));
        }

        public async Task<bool> NameExistsAsync(string domainName, CancellationToken cancellationToken = default)
        {
            return await(await GetDbSetAsync()).AnyAsync(s => s.DomainName == domainName, GetCancellationToken(cancellationToken));
        }
    }
}
