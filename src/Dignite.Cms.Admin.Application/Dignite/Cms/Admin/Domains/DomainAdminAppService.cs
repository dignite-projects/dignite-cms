using Dignite.Cms.Domains;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace Dignite.Cms.Admin.Domains
{
    [Authorize(Permissions.CmsAdminPermissions.Domain.Default)]
    public class DomainAdminAppService : CmsAdminAppServiceBase, IDomainAdminAppService
    {
        private readonly IDomainRepository _domainRepository;
        private readonly DomainManager _domainManager;
        private readonly IDataFilter _dataFilter;

        public DomainAdminAppService(IDomainRepository domainRepository, DomainManager domainManager, IDataFilter dataFilter)
        {
            _domainRepository = domainRepository;
            _domainManager = domainManager;
            _dataFilter = dataFilter;
        }

        [AllowAnonymous]
        public async Task<DomainDto> FindByNameAsync(string domainName)
        {
            using (_dataFilter.Disable<IMultiTenant>())
            {
                var domain = await _domainRepository.FindByNameAsync(domainName);

                return ObjectMapper.Map<Domain, DomainDto>(domain);
            }
        }

        public async Task<DomainDto> GetBoundAsync()
        {
            var boundDomain = await _domainRepository.FindByTenantIdAsync(CurrentTenant.Id.Value);
            return ObjectMapper.Map<Domain, DomainDto>(boundDomain);
        }

        public async Task<bool> NameExistsAsync(string domainName)
        {
            var boundDomain = await _domainRepository.FindByTenantIdAsync(CurrentTenant.Id.Value);
            if (boundDomain == null)
            {
                using (_dataFilter.Disable<IMultiTenant>())
                {
                    return await _domainRepository.NameExistsAsync(domainName);
                }
            }
            else
            {
                if (boundDomain.DomainName.Equals(domainName, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
                else
                {
                    using (_dataFilter.Disable<IMultiTenant>())
                    {
                        return await _domainRepository.NameExistsAsync(domainName);
                    }
                }
            }
        }


        [Authorize(Permissions.CmsAdminPermissions.Domain.Update)]
        public async Task<DomainDto> UpdateAsync(UpdateDomainInput input)
        {
            if (!CurrentTenant.Id.HasValue)
            {
                throw new BusinessException(message: "The primary site does not need to be configured with a domain name!");
            }

            var boundDomain = await _domainRepository.FindByTenantIdAsync(CurrentTenant.Id.Value);
            if (boundDomain == null)
            {
                boundDomain = await _domainManager.CreateAsync(input.DomainName, CurrentTenant.Id.Value);
            }
            else
            {
                if (!boundDomain.DomainName.Equals(input.DomainName, StringComparison.OrdinalIgnoreCase))
                {
                    boundDomain = await _domainManager.UpdateAsync(boundDomain.Id, input.DomainName, boundDomain.ConcurrencyStamp);
                }
            }
            return ObjectMapper.Map<Domain, DomainDto>(boundDomain); 
        }
    }
}
