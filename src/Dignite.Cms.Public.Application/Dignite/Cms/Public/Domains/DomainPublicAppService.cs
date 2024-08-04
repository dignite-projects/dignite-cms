using Dignite.Cms.Domains;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Volo.Abp.Caching;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace Dignite.Cms.Public.Domains
{
    public class DomainPublicAppService : CmsPublicAppService, IDomainPublicAppService
    {
        private readonly IDomainRepository _domainRepository;
        private readonly IDataFilter _dataFilter;
        private readonly IDistributedCache<DomainDto> _domainCache;

        public DomainPublicAppService(IDomainRepository domainRepository, IDataFilter dataFilter, IDistributedCache<DomainDto> domainCache)
        {
            _domainRepository = domainRepository;
            _dataFilter = dataFilter;
            _domainCache = domainCache;
        }

        public async Task<DomainDto> FindByNameAsync(string domainName)
        {
            return await _domainCache.GetOrAddAsync(
                domainName, //Cache key
                async () => await GetDomainFromDatabaseAsync(domainName),
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(1)
                }
            );
        }
        private async Task<DomainDto> GetDomainFromDatabaseAsync(string domainName)
        {
            using (_dataFilter.Disable<IMultiTenant>())
            {
                var domain = await _domainRepository.FindByNameAsync(domainName);

                return ObjectMapper.Map<Domain, DomainDto>(domain);
            }
        }
    }
}
