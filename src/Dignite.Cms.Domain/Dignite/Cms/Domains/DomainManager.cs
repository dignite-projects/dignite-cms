using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;
using Volo.Abp.MultiTenancy;

namespace Dignite.Cms.Domains
{
    public class DomainManager : DomainService
    {
        private readonly IDomainRepository _domainRepository;
        private readonly IDataFilter _dataFilter;

        public DomainManager(IDomainRepository domainRepository, IDataFilter dataFilter)
        {
            _domainRepository = domainRepository;
            _dataFilter = dataFilter;
        }

        public async Task<Domain> CreateAsync(string domainName, Guid tenantId)
        {
            await CheckNameExistenceAsync(domainName);

            var domain = new Domain(GuidGenerator.Create(), domainName, tenantId);
            return await _domainRepository.InsertAsync(domain);
        }

        public async Task<Domain> UpdateAsync(Guid id, string domainName, string concurrencyStamp)
        {
            var entity = await _domainRepository.GetAsync(id, false);
            entity.SetConcurrencyStampIfNotNull(concurrencyStamp);
            if (!entity.DomainName.Equals(domainName, StringComparison.OrdinalIgnoreCase))
            {
                await CheckNameExistenceAsync(domainName);
            }

            //
            entity.DomainName = domainName;
            return await _domainRepository.UpdateAsync(entity);
        }

        protected virtual async Task CheckNameExistenceAsync(string name)
        {
            using (_dataFilter.Disable<IMultiTenant>())
            {
                if (await _domainRepository.NameExistsAsync(name))
                {
                    throw new DomainNameAlreadyExistException(name);
                }
            }
        }
    }
}
