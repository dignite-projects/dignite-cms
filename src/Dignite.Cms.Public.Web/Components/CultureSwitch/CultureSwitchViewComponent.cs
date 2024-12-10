using Dignite.Cms.Public.Sites;
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
    protected ISitePublicAppService _sitePublicAppService { get; }
    protected IOptions<AbpLocalizationOptions> _localizationOptions { get; }

    public CultureSwitchViewComponent(ISitePublicAppService sitePublicAppService,
        IOptions<AbpLocalizationOptions> localizationOptions)
    {
        _sitePublicAppService = sitePublicAppService;
        _localizationOptions = localizationOptions;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var site = await _sitePublicAppService.GetAsync();
        var languages = _localizationOptions.Value.Languages
            .Where(l=> site.AllLanguages.Any(r=>r.Equals(l.CultureName,System.StringComparison.OrdinalIgnoreCase)))
            .ToList();
        var culture = HttpContext.GetRouteValue(CultureRouteSegmentConstraint.RouteSegmentName)?.ToString();
        var currentCulture = culture==null?
            site.DefaultLanguage :
            site.AllLanguages.FirstOrDefault(r=>r.Equals(culture,System.StringComparison.OrdinalIgnoreCase));
        currentCulture = currentCulture == null ? site.DefaultLanguage : currentCulture;

        var model = new CultureSwitchViewComponentModel
        {
            Default = languages.First(l => l.CultureName.Equals(site.DefaultLanguage, System.StringComparison.OrdinalIgnoreCase)),
            CurrentLanguage = languages.First(l=>l.CultureName.Equals(currentCulture,System.StringComparison.OrdinalIgnoreCase)),
            AllLanguages = languages
        };

        return View(model);
    }
}