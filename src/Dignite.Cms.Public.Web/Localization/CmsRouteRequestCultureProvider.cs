using Dignite.Cms.Public.Sites;
using Dignite.Cms.Public.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace Dignite.Cms.Public.Web.Localization;

/// <summary>
/// </summary>
public class CmsRouteRequestCultureProvider : RouteDataRequestCultureProvider
{
    /// <inheritdoc />
    public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
    {
        var providerResultCulture = await base.DetermineProviderCultureResult (httpContext);
        var controller = httpContext.GetRouteValue("Controller")?.ToString();
        string culture;
        if (providerResultCulture == NullProviderCultureResult.Result)
        {
            //Skip if it is not a CMS route
            if (controller == null || controller != EntryController.ControllerName)
            {
                // No values specified for either so no match
                return NullProviderCultureResult.Result;
            }
            else
            {
                //Getting the default Culture from the Cms site
                var _siteAppService = httpContext.RequestServices.GetRequiredService<ISitePublicAppService>();
                var host = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}";
                var site = await _siteAppService.FindByHostAsync(host);
                culture = site.GetDefaultLanguage().CultureName;
            }
        }
        else
        {
            culture = providerResultCulture.Cultures.First().Value;
        }


        /*
         Using the IOptions<AbpLocalizationOptions> approach to get the multilingual list improves performance compared to using the IAbpRequestLocalizationOptionsProvider approach.
         The IAbpRequestLocalizationOptionsProvider way is getting it from the server side, which results in multiple http requests.
         Using the IOptions<AbpLocalizationOptions> approach has the prerequisite that the code to configure the language list needs to be placed in the Domain.Shared project.
         */
        var localizationOptions = httpContext.RequestServices.GetRequiredService<IOptions<AbpLocalizationOptions>>();
        var languages = localizationOptions.Value.Languages;
        var uiCulture = languages.FirstOrDefault(l => l.CultureName.Equals(culture, StringComparison.OrdinalIgnoreCase))?.UiCultureName ?? culture;

        return new ProviderCultureResult(culture, uiCulture);
    }
}
