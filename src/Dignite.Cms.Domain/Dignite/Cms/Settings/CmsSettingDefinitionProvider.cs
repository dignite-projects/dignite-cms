using Dignite.Cms.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace Dignite.Cms.Settings;

public class CmsSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        context.Add(
            new SettingDefinition(CmsSettings.Site.Languages,
                "en,zh-Hant,ja",
                L("DisplayName:Cms.Site.Languages"),
                isVisibleToClients:true
                ),
            new SettingDefinition(CmsSettings.Site.Name,
                "My Dignite",
                L("DisplayName:Cms.Site.Name"),
                isVisibleToClients: true
                ),
            new SettingDefinition(CmsSettings.Site.LogoUrl,
                null,
                L("DisplayName:Cms.Site.LogoUrl"),
                isVisibleToClients: true
                ),
            new SettingDefinition(CmsSettings.Site.LogoReverseUrl,
                null,
                L("DisplayName:Cms.Site.LogoReverseUrl"),
                isVisibleToClients: true
                )
        );
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CmsResource>(name);
    }
}
