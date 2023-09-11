using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using Volo.Abp.Localization;
using System.Linq;

namespace Dignite.Cms.Public.Web.Routing;

/// <summary>
/// TODO: Careful consideration of class names
/// </summary>
public class RegionRouteRequestCultureProvider : RequestCultureProvider
{
    public RegionRouteRequestCultureProvider(List<LanguageInfo> languages)
    {
        Languages = languages;
    }

    private List<LanguageInfo> Languages { get; }

    /// <summary>
    /// The key that contains the region name.
    /// Defaults to "region".
    /// </summary>
    public string RouteDataStringKey { get; set; } = "region";

    /// <inheritdoc />
    public override Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
    {
        if (httpContext == null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        string culture = null;
        string uiCulture = null;

        if (!string.IsNullOrEmpty(RouteDataStringKey))
        {
            culture = httpContext.GetRouteValue(RouteDataStringKey)?.ToString();
        }

        if (culture == null)
        {
            // No values specified for either so no match
            return NullProviderCultureResult;
        }

        uiCulture = Languages.FirstOrDefault(l => l.CultureName.Equals(culture, StringComparison.OrdinalIgnoreCase))?.UiCultureName ?? culture;

        var providerResultCulture = new ProviderCultureResult(culture, uiCulture);

        return Task.FromResult<ProviderCultureResult>(providerResultCulture);
    }
}
