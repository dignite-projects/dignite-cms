using Dignite.Cms.Admin.Sites;
using Dignite.Cms.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dignite.Cms.Admin.Pages
{
    public class SiteAdminAppService : CmsAdminAppServiceBase, ISiteAdminAppService
    {
        private readonly ISiteRepository _siteRepository;

        public SiteAdminAppService(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }

        public async Task<SiteDto> CreateAsync(CreateSiteInput input)
        {
            var entity = new Site(
                GuidGenerator.Create(), 
                input.DisplayName, 
                input.Name,
                input.Cultures.Select(l=>new SiteCulture(l.IsDefault,l.CultureName)).ToList(),
                input.HostUrl,
                input.IsActive, 
                CurrentTenant.Id);


            await _siteRepository.InsertAsync(entity);

            var dto = ObjectMapper.Map<Site, SiteDto>(entity);

            return dto;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _siteRepository.DeleteAsync(id);
        }

        public async Task<SiteDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Site, SiteDto>(
                await _siteRepository.GetAsync(id)
            );
        }

        public async Task<PagedResultDto<SiteDto>> GetListAsync(GetSitesInput input)
        {
            var result = await _siteRepository.GetListAsync( input.Filter, input.IsActive);
            var dto =
                ObjectMapper.Map<List<Site>, List<SiteDto>>(
                    result
                    );

            return new PagedResultDto<SiteDto>(dto.Count, dto);
        }

        public async Task<SiteDto> UpdateAsync(Guid id, UpdateSiteInput input)
        {
            var entity = await _siteRepository.GetAsync(id, false);



            //
            entity.SetDisplayName(input.DisplayName);
            entity.SetName(input.Name);
            entity.SetHostUrl(input.HostUrl);
            entity.SetActive(input.IsActive);
            entity.SetCultures(input.Cultures.Select(l => new SiteCulture(l.IsDefault, l.CultureName)).ToList());
            await _siteRepository.UpdateAsync(entity);

            var dto =
                ObjectMapper.Map<Site, SiteDto>(
                    entity
                    );

            return dto;
        }
    }
}