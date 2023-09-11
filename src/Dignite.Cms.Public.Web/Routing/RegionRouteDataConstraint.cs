using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Volo.Abp.Localization;
using Volo.Abp.Threading;

namespace Dignite.Cms.Public.Web.Routing;

/// <summary>
/// 
/// </summary>
public class RegionRouteDataConstraint : IRouteConstraint
{
    public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
    {
        string region = values["region"]?.ToString();
        var languageProvider = httpContext.RequestServices.GetRequiredService<ILanguageProvider>();
        var languages = AsyncHelper.RunSync(languageProvider.GetLanguagesAsync);

        Regex rgx = new Regex(@"^(" + languages.Select(l => l.CultureName).JoinAsString("|") + ")$", RegexOptions.IgnoreCase);
        if (rgx.IsMatch(region))
        {
            return true;
        }

        return false;
    }
}
