using Dignite.Cms.Public.Sites;
using Dignite.Cms.Public.Web.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;

namespace Dignite.Cms.Public.Web.Components.CultureSwitch;

public class CultureSwitchViewComponent : AbpViewComponent
{
    protected ISitePublicAppService _sitePublicAppService { get; }
    protected IOptions<AbpLocalizationOptions> _localizationOptions { get; }
    protected CultureRouteHelper _cultureRouteHelper { get; }

    public CultureSwitchViewComponent(
        ISitePublicAppService sitePublicAppService,
        IOptions<AbpLocalizationOptions> localizationOptions,
        CultureRouteHelper cultureRouteHelper)
    {
        _sitePublicAppService = sitePublicAppService;
        _localizationOptions = localizationOptions;
        _cultureRouteHelper = cultureRouteHelper;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var site = await _sitePublicAppService.GetAsync();
        var siteLanguages = _localizationOptions.Value.Languages
            .Where(l => site.AllLanguages.Any(r => r.Equals(l.CultureName, System.StringComparison.OrdinalIgnoreCase)))
            .ToList();
        var routeCultureName = HttpContext.GetRouteValue(CultureRouteSegmentConstraint.RouteSegmentName)?.ToString();
        var isMatchingRoute = _cultureRouteHelper.TryMatchRoute(HttpContext,out string routePattern); //判断本页url是否匹配带有Culture路由参数的路由
        var currentCultureName = routeCultureName == null ?
            (isMatchingRoute ? site.DefaultLanguage : CultureInfo.CurrentCulture.Name) :
            site.AllLanguages.FirstOrDefault(r => r.Equals(routeCultureName, System.StringComparison.OrdinalIgnoreCase));

        var model = new CultureSwitchViewComponentModel
        (
            site.DefaultLanguage,
            currentCultureName,
            siteLanguages,
            isMatchingRoute,
            routePattern
        );

        return View(model);
    }
}