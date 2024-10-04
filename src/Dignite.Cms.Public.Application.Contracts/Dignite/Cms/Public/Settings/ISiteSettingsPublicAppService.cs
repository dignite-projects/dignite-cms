using Dignite.Cms.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Dignite.Cms.Public.Settings
{
    public interface ISiteSettingsPublicAppService: IApplicationService
    {
        /// <summary>
        /// Get the default language of the site
        /// </summary>
        /// <returns></returns>
        Task<string> GetDefaultLanguageAsync();

        Task<IEnumerable<string>> GetAllLanguagesAsync();

        Task<BrandDto> GetBrandAsync();
    }
}
