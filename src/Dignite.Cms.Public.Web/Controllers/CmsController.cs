using Dignite.Cms.Localization;
using Dignite.Cms.Public.Entries;
using Dignite.Cms.Public.Sections;
using Dignite.Cms.Public.Sites;
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

        private readonly ISitePublicAppService _sitePublicAppService;
        private readonly ISectionPublicAppService _sectionPublicAppService;
        private readonly IEntryPublicAppService _entryPublicAppService;
        private readonly IOptions<AbpLocalizationOptions> _localizationOptions;


        public CmsController(ISitePublicAppService sitePublicAppService, ISectionPublicAppService sectionPublicAppService, IEntryPublicAppService entryPublicAppService,
            IOptions<AbpLocalizationOptions> localizationOptions)
        {
            LocalizationResource = typeof(CmsResource);
            _sectionPublicAppService = sectionPublicAppService;
            _entryPublicAppService = entryPublicAppService;
            _sitePublicAppService = sitePublicAppService;
            _localizationOptions = localizationOptions;
        }

        public async Task<IActionResult> Index()
        {
            return await EntryViewResult("/", null);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="url">
        /// There are several formats:
        /// 1.{culture}
        /// 2.{culture}/{url}
        /// </param>
        /// <returns></returns>
        public async Task<IActionResult> EntryByCulture(string culture, string url="/")
        {
            return await EntryViewResult(url, culture);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<IActionResult> Entry(string url="/")
        {
            return await EntryViewResult(url, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        protected async Task<IActionResult> EntryViewResult(string url, string culture = null)
        {
            var section = await GetSection(url);
            if (section == null)
            {
                return NotFound();
            }

            //
            var defaultCulture = section.Site.GetDefaultCulture();
            if (culture.IsNullOrEmpty())
            {
                culture = defaultCulture;
            }
            else
            {
                /* Remove the default culture prefix and redirect to the new Url.
                 * Example: the default culture is en, the current request path is /en/about, will jump to /about Url
                 */
                if (culture.Equals(defaultCulture, StringComparison.OrdinalIgnoreCase) 
                    && Request.Path.Value.EnsureEndsWith('/').StartsWith($"/{culture}/"))
                {
                    return RedirectPermanent("~"+Request.GetEncodedPathAndQuery().EnsureEndsWith('/').RemovePreFix($"/{culture}/").EnsureStartsWith('/'));
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
            var entry = await GetEntry(section, url, culture);
            if (entry != null)
            {
                var viewModel = new EntryViewModel(entry, section);
                return View(section.Template, viewModel);
            }
            else
            {
                if (!culture.Equals(defaultCulture, StringComparison.OrdinalIgnoreCase))
                {
                    return Redirect(url.EnsureStartsWith('/'));
                }
                else
                {
                    return NotFound();
                }
            }
        }

        protected async Task<SectionDto> GetSection(string url = null)
        {
            var hostUrl= $"{Request.Scheme}://{Request.Host.Value}";
            var site = await _sitePublicAppService.FindByHostUrlAsync(hostUrl);

            if (site == null)
                return null;

            if (url.IsNullOrEmpty() || url == "/")
            {
                return await _sectionPublicAppService.GetDefaultAsync(site.Id);
            }
            else
            {
                return await _sectionPublicAppService.FindByUrlAsync(site.Id,url);
            }
        }

        protected async Task<EntryDto> GetEntry(SectionDto section, string url, string culture)
        {
            EntryDto entry = null;
            // If the section type is single, then the slug value of the entry is the name of the section
            if (section.Type == Cms.Sections.SectionType.Single) 
            {
                var result = await _entryPublicAppService.GetListAsync(new GetEntriesInput { 
                    SectionId = section.Id,
                    SkipCount=0,
                    MaxResultCount=1,
                    Culture = culture
                });
                entry = result.Items.Any() ? result.Items[0] : null;
            }
            else
            {
                string slug = null;
                //Extract Slug value from URL
                var extractResult = FormattedStringValueExtracter.Extract(url.RemovePreFix("/").RemovePostFix("/"), section.Route.RemovePreFix("/").RemovePostFix("/"), ignoreCase: true);
                if (extractResult.IsMatch)
                {
                    slug = extractResult.Matches.First(m => m.Name.Equals(nameof(EntryDto.Slug), StringComparison.OrdinalIgnoreCase)).Value;
                    //
                    entry = await _entryPublicAppService.FindBySlugAsync(new FindBySlugInput
                    {
                        Culture = culture,
                        SectionId = section.Id,
                        Slug = slug
                    });
                }
                else
                {
                    throw new Volo.Abp.AbpException($"The structure type section and channel type section route of the entry must contain {{slug}}");
                }
            }


            if (entry != null)
            {
                SetEntryUrl(entry);
            }

            return entry;
        }


        protected void SetEntryUrl(EntryDto entry)
        {
            var hostAddress = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host;
            if (entry.Url.StartsWith(hostAddress, StringComparison.OrdinalIgnoreCase))
            {
                entry.Url = entry.Url.RemovePreFix(StringComparison.OrdinalIgnoreCase, hostAddress);
            }
        }
    }
}
