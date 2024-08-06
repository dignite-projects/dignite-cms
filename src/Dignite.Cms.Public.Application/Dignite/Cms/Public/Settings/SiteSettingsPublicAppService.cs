using Dignite.Cms.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace Dignite.Cms.Public.Settings
{
    public class SiteSettingsPublicAppService : CmsPublicAppService, ISiteSettingsPublicAppService
    {
        public SiteSettingsPublicAppService()
        {
        }

        public async Task<IEnumerable<string>> GetAllLanguagesAsync()
        {
            var languages = await SettingProvider.GetOrNullAsync(CmsSettings.Site.Languages);
            return languages.Split(';');
        }

        public async Task<string> GetDefaultLanguageAsync()
        {
            return await SettingProvider.GetOrNullAsync(LocalizationSettingNames.DefaultLanguage);
        }
    }
}
