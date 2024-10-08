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
    public class EntryController : AbpController
    {
        public const string ControllerName = "Entry";

        private readonly ISiteSettingsPublicAppService _siteSettingsPublicAppService;
        private readonly ISectionPublicAppService _sectionPublicAppService;
        private readonly IEntryPublicAppService _entryPublicAppService;
        private readonly IOptions<AbpLocalizationOptions> _localizationOptions;


        public EntryController(ISiteSettingsPublicAppService siteSettingsPublicAppService, ISectionPublicAppService sectionPublicAppService, IEntryPublicAppService entryPublicAppService,
            IOptions<AbpLocalizationOptions> localizationOptions)
        {
            LocalizationResource = typeof(CmsResource);
            _sectionPublicAppService = sectionPublicAppService;
            _entryPublicAppService = entryPublicAppService;
            _siteSettingsPublicAppService = siteSettingsPublicAppService;
            _localizationOptions = localizationOptions;
        }

        public async Task<IActionResult> HomePage()
        {
            return await EntryResult();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entryPath"></param>
        /// <returns></returns>
        public async Task<IActionResult> EntryPage(string entryPath)
        {
            return await EntryResult(null, entryPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="entryPath">
        /// There are several formats:
        /// 1.{culture}
        /// 2.{culture}/{entryPath}
        /// </param>
        /// <returns></returns>
        public async Task<IActionResult> EntryPageWithCulture(string culture, string entryPath)
        {
            return await EntryResult(culture, entryPath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        protected async Task<IActionResult> EntryResult(string culture = null, string path = "/")
        {
            var section = await GetSection(path);
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
                /* Remove the default culture prefix and redirect to the new path.
                 * Example: the default culture is en, the current request path is /en/about, will jump to /about path
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
            var entry = await GetEntry(culture, section, path);
            if (entry != null)
            {
                var viewModel = new EntryViewModel(entry, section);
                return View(section.Template, viewModel);
            }
            else
            {
                if (!culture.Equals(defaultCulture, StringComparison.OrdinalIgnoreCase))
                {
                    return Redirect(path.EnsureStartsWith('/'));
                }
                else
                {
                    return NotFound();
                }
            }
        }

        protected async Task<SectionDto> GetSection(string path)
        {
            if (path.IsNullOrEmpty() || path == "/")
            {
                return await _sectionPublicAppService.GetDefaultAsync();
            }
            else
            {
                return await _sectionPublicAppService.FindByEntryPathAsync(path);
            }
        }

        protected async Task<EntryDto> GetEntry(string culture, SectionDto section, string path)
        {
            var slug = ExtractSlug(section.Route, path);
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



        protected virtual string ExtractSlug(string route, string path)
        {
            string slug = null;
            //Extract Slug value from path
            var extractResult = FormattedStringValueExtracter.Extract(path.RemovePreFix("/").RemovePostFix("/"), route.RemovePreFix("/").RemovePostFix("/"), ignoreCase: true);
            if (extractResult.IsMatch)
            {
                slug = extractResult.Matches.FirstOrDefault(m => m.Name.Equals(nameof(EntryDto.Slug), StringComparison.InvariantCultureIgnoreCase))?.Value;
            }

            return slug;
        }
    }
}
