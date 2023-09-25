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
        var hostUrl = $"{Request.Scheme}://{Request.Host.Value}";
        var site = await _sitePublicAppService.FindByHostUrlAsync(hostUrl);
        var languages = _localizationOptions.Value.Languages
            .Where(l=>site.Cultures.Any(r=>r.CultureName.Equals(l.CultureName,System.StringComparison.OrdinalIgnoreCase)))
            .ToList();
        var culture = HttpContext.GetRouteValue(CultureRouteSegmentConstraint.RouteSegmentName)?.ToString();
        var currentCulture = culture==null? 
            site.GetDefaultCulture():
            site.Cultures.FirstOrDefault(r=>r.CultureName.Equals(culture,System.StringComparison.OrdinalIgnoreCase))?.CultureName;
        currentCulture = currentCulture == null ? site.GetDefaultCulture() : currentCulture;

        var model = new CultureSwitchViewComponentModel
        {
            CurrentLanguage = languages.First(l=>l.CultureName.Equals(currentCulture,System.StringComparison.OrdinalIgnoreCase)),
            AllLanguages = languages
        };

        return View(model);
    }
}