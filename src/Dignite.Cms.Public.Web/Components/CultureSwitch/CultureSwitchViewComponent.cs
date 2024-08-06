using Dignite.Cms.Public.Settings;
using Dignite.Cms.Public.Web.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;

namespace Dignite.Cms.Public.Web.Components.CultureSwitch;

public class CultureSwitchViewComponent : AbpViewComponent
{
    protected ISiteSettingsPublicAppService _siteSettingsPublicAppService { get; }
    protected IOptions<AbpLocalizationOptions> _localizationOptions { get; }

    public CultureSwitchViewComponent(ISiteSettingsPublicAppService siteSettingsPublicAppService,
        IOptions<AbpLocalizationOptions> localizationOptions)
    {
        _siteSettingsPublicAppService = siteSettingsPublicAppService;
        _localizationOptions = localizationOptions;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var siteDefaultLanguage = await _siteSettingsPublicAppService.GetDefaultLanguageAsync();
        var siteAllLanguages = await _siteSettingsPublicAppService.GetAllLanguagesAsync();
        var languages = _localizationOptions.Value.Languages
            .Where(l=> siteAllLanguages.Any(r=>r.Equals(l.CultureName,System.StringComparison.OrdinalIgnoreCase)))
            .ToList();
        var culture = HttpContext.GetRouteValue(CultureRouteSegmentConstraint.RouteSegmentName)?.ToString();
        var currentCulture = culture==null?
            siteDefaultLanguage :
            siteAllLanguages.FirstOrDefault(r=>r.Equals(culture,System.StringComparison.OrdinalIgnoreCase));
        currentCulture = currentCulture == null ? siteDefaultLanguage : currentCulture;

        var model = new CultureSwitchViewComponentModel
        {
            Default = languages.First(l => l.CultureName.Equals(siteDefaultLanguage, System.StringComparison.OrdinalIgnoreCase)),
            CurrentLanguage = languages.First(l=>l.CultureName.Equals(currentCulture,System.StringComparison.OrdinalIgnoreCase)),
            AllLanguages = languages
        };

        return View(model);
    }
}