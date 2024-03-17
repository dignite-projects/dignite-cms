using Dignite.Cms.Admin.Sites;
using Dignite.Cms.Sites;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace Dignite.Cms.Admin.Pages
{
    [Authorize(Permissions.CmsAdminPermissions.Site.Default)]
    public class SiteAdminAppService : CmsAdminAppServiceBase, ISiteAdminAppService
    {
        private readonly ISiteRepository _siteRepository;
        private readonly SiteManager _siteManager;
        private readonly IDataFilter _dataFilter;

        public SiteAdminAppService(ISiteRepository siteRepository, SiteManager siteManager, IDataFilter dataFilter)
        {
            _siteRepository = siteRepository;
            _siteManager = siteManager;
            _dataFilter = dataFilter;
        }

        [Authorize(Permissions.CmsAdminPermissions.Site.Create)]
        public async Task<SiteDto> CreateAsync(CreateSiteInput input)
        {
            if (!input.Languages.Any(m => m.IsDefault))
            {
                input.Languages.First().IsDefault = true;
            }

            var entity = await _siteManager.CreateAsync(
                input.DisplayName,
                input.Name,
                input.Host,
                input.IsActive,
                input.Languages.Select(
                    l => new SiteLanguage(
                        l.IsDefault,
                        l.CultureName)
                    ).ToList(),
                CurrentTenant.Id);

            var dto = ObjectMapper.Map<Site, SiteDto>(entity);

            return dto;
        }

        [Authorize(Permissions.CmsAdminPermissions.Site.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _siteRepository.DeleteAsync(id);
        }

        [Authorize(Permissions.CmsAdminPermissions.Site.Default)]
        public async Task<bool> NameExistsAsync(string name)
        {
            return await _siteRepository.NameExistsAsync(name);
        }

        [Authorize(Permissions.CmsAdminPermissions.Site.Default)]
        public async Task<bool> HostExistsAsync(string host)
        {
            using (_dataFilter.Disable<IMultiTenant>())
            {
                return await _siteRepository.HostExistsAsync(host);
            }
        }

        [Authorize(Permissions.CmsAdminPermissions.Site.Default)]
        public async Task<SiteDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Site, SiteDto>(
                await _siteRepository.GetAsync(id)
            );
        }

        [Authorize(Permissions.CmsAdminPermissions.Site.Default)]
        public async Task<PagedResultDto<SiteDto>> GetListAsync(GetSitesInput input)
        {
            var result = await _siteRepository.GetListAsync( input.Filter, input.IsActive);
            var dto =
                ObjectMapper.Map<List<Site>, List<SiteDto>>(
                    result
                    );

            return new PagedResultDto<SiteDto>(dto.Count, dto);
        }

        [Authorize(Permissions.CmsAdminPermissions.Site.Update)]
        public async Task<SiteDto> UpdateAsync(Guid id, UpdateSiteInput input)
        {
            if (!input.Languages.Any(m => m.IsDefault))
            {
                input.Languages.First().IsDefault = true;
            }

            var entity = await _siteManager.UpdateAsync(
                id, 
                input.DisplayName, 
                input.Name, 
                input.Host, 
                input.IsActive, 
                input.Languages.Select(
                    l => new SiteLanguage(
                        l.IsDefault, 
                        l.CultureName)
                    ).ToList(),
                input.ConcurrencyStamp);

            var dto =
                ObjectMapper.Map<Site, SiteDto>(
                    entity
                    );

            return dto;
        }
    }
}