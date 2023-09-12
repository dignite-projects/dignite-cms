using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Volo.Abp.Localization;

namespace Dignite.Cms.Public.Web.Routing;

/// <summary>
/// This is a constraint class for the route parameter region. 
/// This is used when constraining the region route parameter in the configuration route pattern using the code {region:RegionalConstraint}.
/// </summary>
public class RegionRouteSegmentConstraint : IRouteConstraint
{
    public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        string region = values["region"]?.ToString();

        /*
         Using the IOptions<AbpLocalizationOptions> approach to get the multilingual list improves performance compared to using the IAbpRequestLocalizationOptionsProvider approach.
         The IAbpRequestLocalizationOptionsProvider way is getting it from the server side, which results in multiple http requests.
         Using the IOptions<AbpLocalizationOptions> approach has the prerequisite that the code to configure the language list needs to be placed in the Domain.Shared project.
         */
        var localizationOptions = httpContext.RequestServices.GetRequiredService<IOptions<AbpLocalizationOptions>>();
        var languages = localizationOptions.Value.Languages;

        Regex rgx = new Regex(@"^(" + languages.Select(l => l.CultureName).JoinAsString("|") + ")$", RegexOptions.IgnoreCase);
        if (rgx.IsMatch(region))
        {
            return true;
        }

        return false;
    }
}
