using Dignite.Cms.Public.Sites;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace Dignite.Cms.Public.Web.Routing
{
    public class CmsRouteRequestCultureProvider : RouteDataRequestCultureProvider
    {
        /// <inheritdoc />
        public override async Task<ProviderCultureResult> DetermineProviderCultureResult(HttpContext httpContext)
        {
            var providerResultCulture = await base.DetermineProviderCultureResult(httpContext);
            string culture;
            if (providerResultCulture == NullProviderCultureResult.Result)
            {
                if (!ShouldProcessCulture(httpContext))
                {
                    // No values specified for either so no match
                    return NullProviderCultureResult.Result;
                }
                else
                {
                    var cultureRouteHelper = httpContext.RequestServices.GetRequiredService<CultureRouteHelper>();
                    if (cultureRouteHelper.TryMatchRoute(httpContext))
                    {
                        //Getting the default Culture from the Cms site
                        var _siteAppService = httpContext.RequestServices.GetRequiredService<ISitePublicAppService>();
                        var site = await _siteAppService.GetAsync();
                        culture = site.DefaultLanguage;
                    }
                    else
                    {
                        // No values specified for either so no match
                        return NullProviderCultureResult.Result;
                    }
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

            //
            AbpRequestCultureCookieHelper.SetCultureCookie(
                httpContext,
                new RequestCulture(culture, uiCulture)
            );

            return new ProviderCultureResult(culture, uiCulture);
        }

        private bool ShouldProcessCulture(HttpContext httpContext)
        {
            var endpoint = httpContext.GetEndpoint();
            if (endpoint == null)
            {
                return false;
            }

            // 检查是否为 Razor Page
            var pageActionDescriptor = endpoint.Metadata.GetMetadata<CompiledPageActionDescriptor>();
            if (pageActionDescriptor != null)
            {
                return typeof(ICmsCultureRouteable).IsAssignableFrom(pageActionDescriptor.DeclaredModelTypeInfo);
            }

            // 检查是否为 MVC Controller
            var controllerActionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
            if (controllerActionDescriptor != null)
            {
                var controllerType = controllerActionDescriptor.ControllerTypeInfo;
                return typeof(ICmsCultureRouteable).IsAssignableFrom(controllerType);
            }

            return false;
        }
    }
}
