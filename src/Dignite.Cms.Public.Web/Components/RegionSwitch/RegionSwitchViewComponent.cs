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

namespace Dignite.Cms.Public.Web.Components.RegionSwitch;

public class RegionSwitchViewComponent : AbpViewComponent
{
    protected ISitePublicAppService _sitePublicAppService { get; }
    protected IOptions<AbpLocalizationOptions> _localizationOptions { get; }

    public RegionSwitchViewComponent(ISitePublicAppService sitePublicAppService,
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
            .Where(l=>site.Regions.Any(r=>r.Region.Equals(l.CultureName,System.StringComparison.OrdinalIgnoreCase)))
            .ToList();
        var culture = HttpContext.GetRouteValue(CultureRouteSegmentConstraint.RouteSegmentName)?.ToString();
        var currentRegion = culture==null? site.GetDefaultRegion():site.Regions.FirstOrDefault(r=>r.Region==culture)?.Region;
        currentRegion = currentRegion == null ? site.GetDefaultRegion() : currentRegion;

        var model = new RegionSwitchViewComponentModel
        {
            CurrentLanguage = languages.First(l=>l.CultureName.Equals(currentRegion,System.StringComparison.OrdinalIgnoreCase)),
            AllLanguages = languages
        };

        return View(model);
    }
}