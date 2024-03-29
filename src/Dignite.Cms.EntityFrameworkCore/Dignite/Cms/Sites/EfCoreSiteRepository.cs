﻿using Dignite.Cms.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Dignite.Cms.Sites
{
    public class EfCoreSiteRepository : EfCoreRepository<ICmsDbContext, Site,Guid>, ISiteRepository
    {
        public EfCoreSiteRepository(
            IDbContextProvider<ICmsDbContext> dbContextProvider
            )
            : base(dbContextProvider)
        {
        }


        public async Task<bool> NameExistsAsync(string name, CancellationToken cancellationToken = default)
        {
            return await(await GetDbSetAsync())
                       .AnyAsync(s => s.Name == name, GetCancellationToken(cancellationToken));
        }
        public async Task<bool> HostExistsAsync(string host, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                       .AnyAsync(s => s.Host == host, GetCancellationToken(cancellationToken));
        }

        public async Task<Site> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return await(await GetDbSetAsync()).FirstOrDefaultAsync(s => s.Name == name, GetCancellationToken(cancellationToken));
        }
        public async Task<Site> FindByHostAsync(string host, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync()).FirstOrDefaultAsync(s => s.Host == host, GetCancellationToken(cancellationToken));
        }

        public async Task<List<Site>> GetListAsync(string filter = null, bool? isActive = null, CancellationToken cancellationToken = default)
        {
            return await (await GetDbSetAsync())
                .WhereIf(!filter.IsNullOrWhiteSpace(), s => s.DisplayName.Contains(filter) || s.Name.Contains(filter))
                .WhereIf(isActive.HasValue, s => s.IsActive == isActive)
                .OrderByDescending(s => s.IsActive)
                .ThenBy(s => s.CreationTime)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }
    }
}
