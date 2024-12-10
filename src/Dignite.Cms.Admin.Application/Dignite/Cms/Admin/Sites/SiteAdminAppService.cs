using Dignite.Cms.Settings;
using Dignite.Cms.Sites;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Localization;
using Volo.Abp.SettingManagement;

namespace Dignite.Cms.Admin.Sites
{
    public class SiteAdminAppService : CmsAdminAppServiceBase, ISiteAdminAppService
    {
        private readonly ISettingManager _settingManager;

        public SiteAdminAppService(ISettingManager settingManager)
        {
            _settingManager = settingManager;
        }

        public async Task<SiteDto> GetAsync()
        {
            var site = new SiteDto();
            site.DefaultLanguage = await SettingProvider.GetOrNullAsync(LocalizationSettingNames.DefaultLanguage);
            site.AllLanguages = (await SettingProvider.GetOrNullAsync(CmsSettings.Site.Languages)).Split(',');

            return site;
        }

        public async Task UpdateAsync(UpdateSiteInput input)
        {
            await _settingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, LocalizationSettingNames.DefaultLanguage, input.DefaultLanguage);
            await _settingManager.SetForTenantOrGlobalAsync(CurrentTenant.Id, CmsSettings.Site.Languages, input.AllLanguages.JoinAsString(","));
        }
    }
}
