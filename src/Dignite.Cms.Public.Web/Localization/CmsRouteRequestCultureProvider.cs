using Dignite.Cms.Public.Sites;
using Dignite.Cms.Public.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace Dignite.Cms.Public.Web.Localization;

/// <summary>
/// Reading <see cref="RouteDataStringKey"/> data from CMS routes and converting to <see cref="ProviderCultureResult"/>
/// </summary>
public class CmsRouteRequestCultureProvider : RequestCultureProvider
{
    /// <summary>
    /// The key that contains the region name.
    /// Defaults to "region".
    /// </summary>
    public string RouteDataStringKey { get; set; } = "region";

    /// <inheritdoc />
    public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
    {
        if (httpContext == null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        string culture = null;
        string uiCulture = null;
        string controller = null;

        controller = httpContext.GetRouteValue("Controller")?.ToString();
        culture = httpContext.GetRouteValue(RouteDataStringKey)?.ToString();

        //Skip if it is not a CMS route
        if (controller == null || controller != CmsController.ControllerName)
        {
            // No values specified for either so no match
            return NullProviderCultureResult.Result;
        }
        else
        {
            if (culture == null)
            {
                //Getting the default region from the Cms site
                var _siteAppService = httpContext.RequestServices.GetRequiredService<ISitePublicAppService>();
                var hostUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}";
                var site = await _siteAppService.FindByHostUrlAsync(hostUrl);
                culture = site.GetDefaultRegion();
            }
        }


        /*
         Using the IOptions<AbpLocalizationOptions> approach to get the multilingual list improves performance compared to using the IAbpRequestLocalizationOptionsProvider approach.
         The IAbpRequestLocalizationOptionsProvider way is getting it from the server side, which results in multiple http requests.
         Using the IOptions<AbpLocalizationOptions> approach has the prerequisite that the code to configure the language list needs to be placed in the Domain.Shared project.
         */
        var localizationOptions = httpContext.RequestServices.GetRequiredService<IOptions<AbpLocalizationOptions>>();
        var languages = localizationOptions.Value.Languages;
        uiCulture = languages.FirstOrDefault(l => l.CultureName.Equals(culture, StringComparison.OrdinalIgnoreCase))?.UiCultureName ?? culture;

        var providerResultCulture = new ProviderCultureResult(culture, uiCulture);

        return providerResultCulture;
    }
}
