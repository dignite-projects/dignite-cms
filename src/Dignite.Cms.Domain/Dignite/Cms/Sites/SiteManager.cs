using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;

namespace Dignite.Cms.Sites
{
    public class SiteManager:DomainService
    {
        protected readonly ISiteRepository _siteRepository;

        public SiteManager(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }

        public async Task<Site> CreateAsync(string displayName, string name, string host, bool isActive, List<SiteLanguage> languages, Guid? tenantId=null)
        {
            await CheckNameExistenceAsync(name);
            await CheckHostExistenceAsync(host);

            var entity = new Site(
                GuidGenerator.Create(),
                displayName,
                name,
                host,
                isActive,
                tenantId);
            languages.ForEach(entity.AddLanguage);

            return await _siteRepository.InsertAsync(entity);
        }

        public async Task<Site> UpdateAsync(Guid id, string displayName, string name, string host, bool isActive, List<SiteLanguage> languages,string concurrencyStamp)
        {
            var entity = await _siteRepository.GetAsync(id, false);
            entity.SetConcurrencyStampIfNotNull(concurrencyStamp);
            if (!entity.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                await CheckNameExistenceAsync(name);
            }
            if (!entity.Host.Equals(host, StringComparison.OrdinalIgnoreCase))
            {
                await CheckHostExistenceAsync(host);
            }

            //
            entity.SetDisplayName(displayName);
            entity.SetName(name);
            entity.SetHost(host);
            entity.SetActive(isActive);
            entity.Languages.Clear();
            languages.ForEach(entity.AddLanguage);
            
            return await _siteRepository.UpdateAsync(entity);
        }

        protected virtual async Task CheckNameExistenceAsync(string name)
        {
            if (await _siteRepository.NameExistsAsync( name))
            {
                throw new SiteNameAlreadyExistException(name);
            }
        }
        protected virtual async Task CheckHostExistenceAsync(string host)
        {
            if (await _siteRepository.HostExistsAsync(host))
            {
                throw new SiteHostAlreadyExistException(host);
            }
        }
    }
}
