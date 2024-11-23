using Asp.Versioning;
using Dignite.Cms.Entries;
using Dignite.Cms.Localization;
using Dignite.Cms.Public.Entries;
using Dignite.Cms.Public.Sections;
using Dignite.Cms.Public.Settings;
using Dignite.Cms.Public.Web.Models;
using Dignite.Cms.Public.Web.Routing;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RequestLocalization;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Localization;
using Volo.Abp.Text.Formatting;

namespace Dignite.Cms.Public.Web.Controllers
{
    [ControllerName(ControllerName)]
    public class CmsController : AbpController
    {
        public const string ControllerName = "Cms";

        private readonly ISiteSettingsPublicAppService _siteSettingsPublicAppService;
        private readonly ISectionPublicAppService _sectionPublicAppService;
        private readonly IEntryPublicAppService _entryPublicAppService;
        private readonly IOptions<AbpLocalizationOptions> _localizationOptions;


        public CmsController(ISiteSettingsPublicAppService siteSettingsPublicAppService, ISectionPublicAppService sectionPublicAppService, IEntryPublicAppService entryPublicAppService,
            IOptions<AbpLocalizationOptions> localizationOptions)
        {
            LocalizationResource = typeof(CmsResource);
            _sectionPublicAppService = sectionPublicAppService;
            _entryPublicAppService = entryPublicAppService;
            _siteSettingsPublicAppService = siteSettingsPublicAppService;
            _localizationOptions = localizationOptions;
        }

        public async Task<IActionResult> Default()
        {
            return await GetEntryActionResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public async Task<IActionResult> Entry(string route)
        {
            return await GetEntryActionResult(null, route);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="route">
        /// There are several formats:
        /// 1.{culture}
        /// 2.{culture}/{route}
        /// </param>
        /// <returns></returns>
        public async Task<IActionResult> CultureEntry(string culture, string route)
        {
            return await GetEntryActionResult(culture, route);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="route"></param>
        /// <returns></returns>
        protected async Task<IActionResult> GetEntryActionResult(string culture = null, string route = "/")
        {
            var section = await GetSection(route);
            if (section == null)
            {
                return NotFound();
            }

            //
            var defaultCulture = await _siteSettingsPublicAppService.GetDefaultLanguageAsync();
            if (culture.IsNullOrEmpty())
            {
                culture = defaultCulture;
            }
            else
            {
                /* Remove the default culture prefix and redirect to the new route.
                 * Example: the default culture is en, the current request route is /en/about, will jump to /about route
                 */
                if (culture.Equals(defaultCulture, StringComparison.OrdinalIgnoreCase) 
                    && Request.Path.Value.EnsureEndsWith('/').StartsWith($"/{culture}/"))
                {
                    return LocalRedirectPermanent(Request.GetEncodedPathAndQuery().RemovePreFix($"/{culture}").EnsureStartsWith('/').EnsureStartsWith('~'));
                }

                if (!culture.Equals(defaultCulture, StringComparison.OrdinalIgnoreCase) 
                    && !Request.RouteValues.Any(r=>r.Key.Equals( CultureRouteSegmentConstraint.RouteSegmentName,StringComparison.OrdinalIgnoreCase)))
                {
                    return Redirect(culture.ToLower());
                }
            }

            //Saving the currently requested cultural information to a cookie
            AbpRequestCultureCookieHelper.SetCultureCookie(
                HttpContext, 
                new RequestCulture(
                    culture, 
                    _localizationOptions.Value.Languages.First(l=>l.CultureName.Equals(culture,StringComparison.OrdinalIgnoreCase)).UiCultureName
                    )
                );

            //
            var entry = await GetEntry(culture, section, route);
            if (entry != null)
            {
                var viewModel = new EntryViewModel(entry, section);
                return View(section.Template, viewModel);
            }
            else
            {
                if (!culture.Equals(defaultCulture, StringComparison.OrdinalIgnoreCase))
                {
                    return Redirect(route.EnsureStartsWith('/'));
                }
                else
                {
                    return NotFound();
                }
            }
        }

        protected async Task<SectionDto> GetSection(string route)
        {
            if (route.IsNullOrEmpty() || route == "/")
            {
                return await _sectionPublicAppService.GetDefaultAsync();
            }
            else
            {
                return await _sectionPublicAppService.FindByRouteAsync(route);
            }
        }

        protected async Task<EntryDto> GetEntry(string culture, SectionDto section, string route)
        {
            var slug = ExtractSlug(section, route);
            if (slug.IsNullOrEmpty())
            {
                if (section.Type == Cms.Sections.SectionType.Single)
                    slug = EntryConsts.DefaultSlug;
                else
                    throw new Volo.Abp.AbpException($"The structure type section and channel type section route of the entry must contain {{slug}}");
            }

            EntryDto entry = await _entryPublicAppService.FindBySlugAsync(
                                                            new FindBySlugInput
                                                                {
                                                                    Culture = culture,
                                                                    SectionId = section.Id,
                                                                    Slug = slug
                                                                });
            return entry;
        }



        protected virtual string ExtractSlug(SectionDto section, string entryRoute)
        {
            string slug = null;
            //Extract Slug value from entryRoute
            var extractResult = FormattedStringValueExtracter.Extract(entryRoute.RemovePreFix("/").RemovePostFix("/"), section.Route.RemovePreFix("/").RemovePostFix("/"), ignoreCase: true);
            if (extractResult.IsMatch)
            {
                slug = extractResult.Matches.FirstOrDefault(m => m.Name.Equals(nameof(EntryDto.Slug), StringComparison.InvariantCultureIgnoreCase))?.Value;
            }

            return slug;
        }
    }
}
