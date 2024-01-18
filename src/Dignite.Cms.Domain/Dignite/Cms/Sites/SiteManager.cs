using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<Site> CreateAsync(string displayName, string name, ICollection<SiteCulture> cultures, string host, bool isActive, Guid? tenantId)
        {
            await CheckNameExistenceAsync(name);
            var entity = new Site(
                GuidGenerator.Create(),
                displayName,
                name,
                cultures.Select(l => new SiteCulture(l.IsDefault, l.CultureName)).ToList(),
                host,
                isActive,
                tenantId);


            return await _siteRepository.InsertAsync(entity);
        }

        public async Task<Site> UpdateAsync(Guid id, string displayName, string name, ICollection<SiteCulture> cultures, string host, bool isActive)
        {
            var entity = await _siteRepository.GetAsync(id, false);
            if (!entity.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                await CheckNameExistenceAsync(name);
            }

            //
            entity.SetDisplayName(displayName);
            entity.SetName(name);
            entity.SetHost(host);
            entity.SetActive(isActive);
            entity.SetCultures(cultures.Select(l => new SiteCulture(l.IsDefault, l.CultureName)).ToList());
            
            return await _siteRepository.UpdateAsync(entity);
        }

        protected virtual async Task CheckNameExistenceAsync( string name)
        {
            if (await _siteRepository.NameExistsAsync( name))
            {
                throw new SiteNameAlreadyExistException(name);
            }
        }
    }
}
