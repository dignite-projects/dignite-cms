using Dignite.Cms.Settings;
using Dignite.Cms.Sites;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace Dignite.Cms.Public.Sites
{
    public class SitePublicAppService : CmsPublicAppService, ISitePublicAppService
    {
        public async Task<SiteDto> GetAsync()
        {
            var site = new SiteDto();
            site.DefaultLanguage = await SettingProvider.GetOrNullAsync(LocalizationSettingNames.DefaultLanguage);
            site.AllLanguages = (await SettingProvider.GetOrNullAsync(CmsSettings.Site.Languages)).Split(',');

            return site;
        }
    }
}
