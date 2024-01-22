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
        var host = $"{Request.Scheme}://{Request.Host.Value}";
        var site = await _sitePublicAppService.FindByHostAsync(host);
        var languages = _localizationOptions.Value.Languages
            .Where(l=>site.Languages.Any(r=>r.CultureName.Equals(l.CultureName,System.StringComparison.OrdinalIgnoreCase)))
            .ToList();
        var culture = HttpContext.GetRouteValue(CultureRouteSegmentConstraint.RouteSegmentName)?.ToString();
        var currentCulture = culture==null? 
            site.GetDefaultLanguage().CultureName:
            site.Languages.FirstOrDefault(r=>r.CultureName.Equals(culture,System.StringComparison.OrdinalIgnoreCase))?.CultureName;
        currentCulture = currentCulture == null ? site.GetDefaultLanguage().CultureName : currentCulture;

        var model = new CultureSwitchViewComponentModel
        {
            CurrentLanguage = languages.First(l=>l.CultureName.Equals(currentCulture,System.StringComparison.OrdinalIgnoreCase)),
            AllLanguages = languages
        };

        return View(model);
    }
}