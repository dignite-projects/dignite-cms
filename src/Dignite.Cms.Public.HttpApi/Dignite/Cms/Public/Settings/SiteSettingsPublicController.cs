using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp;

namespace Dignite.Cms.Public.Settings
{
    [RemoteService(Name = CmsPublicRemoteServiceConsts.RemoteServiceName)]
    [Area(CmsPublicRemoteServiceConsts.ModuleName)]
    [Route("api/cms-public/site-settings")]
    public class SiteSettingsPublicController : CmsPublicController, ISiteSettingsPublicAppService
    {
        private readonly ISiteSettingsPublicAppService _siteSettingsPublicAppService;

        public SiteSettingsPublicController(ISiteSettingsPublicAppService siteSettingsPublicAppService)
        {
            _siteSettingsPublicAppService = siteSettingsPublicAppService;
        }

        [HttpGet]
        [Route("default-language")]
        public async Task<string> GetDefaultLanguageAsync()
        {
            return await _siteSettingsPublicAppService.GetDefaultLanguageAsync();
        }

        [HttpGet]
        [Route("all-languages")]
        public async Task<IEnumerable<string>> GetAllLanguagesAsync()
        {
            return await _siteSettingsPublicAppService.GetAllLanguagesAsync();
        }

        [HttpGet]
        [Route("brand")]
        public async Task<BrandDto> GetBrandAsync()
        {
            return await _siteSettingsPublicAppService.GetBrandAsync();
        }
    }
}
