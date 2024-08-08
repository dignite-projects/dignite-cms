using Dignite.Cms.Settings;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace Dignite.Cms.Admin.Settings
{
    [RemoteService(Name = CmsAdminRemoteServiceConsts.RemoteServiceName)]
    [Area(CmsAdminRemoteServiceConsts.ModuleName)]
    [Route("api/cms-admin/site-settings")]
    public class SiteSettingsAdminController : CmsAdminController, ISiteSettingsAdminAppService
    {
        private readonly ISiteSettingsAdminAppService _siteSettingsAdminAppService;

        public SiteSettingsAdminController(ISiteSettingsAdminAppService siteSettingsAdminAppService)
        {
            _siteSettingsAdminAppService = siteSettingsAdminAppService;
        }

        [HttpGet]
        [Route("default-language")]
        public async Task<string> GetDefaultLanguageAsync()
        {
            return await _siteSettingsAdminAppService.GetDefaultLanguageAsync();
        }

        [HttpGet]
        [Route("all-languages")]
        public async Task<IEnumerable<string>> GetAllLanguagesAsync()
        {
            return await _siteSettingsAdminAppService.GetAllLanguagesAsync();
        }

        [HttpGet]
        [Route("brand")]
        public async Task<BrandDto> GetBrandAsync()
        {
            return await _siteSettingsAdminAppService.GetBrandAsync();
        }
    }
}
