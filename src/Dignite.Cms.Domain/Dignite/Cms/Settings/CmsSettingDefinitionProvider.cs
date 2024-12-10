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
                )
        );
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CmsResource>(name);
    }
}
