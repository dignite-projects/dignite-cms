using Dignite.Cms.Localization;
using Dignite.Cms.Public.Entries;
using Dignite.Cms.Public.Sections;
using Dignite.Cms.Public.Sites;
using Dignite.Cms.Public.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Text.Formatting;

namespace Dignite.Cms.Public.Web.Controllers
{
    public class CmsController : AbpController
    {
        private readonly ISitePublicAppService _sitePublicAppService;
        private readonly ISectionPublicAppService _sectionPublicAppService;
        private readonly IEntryPublicAppService _entryPublicAppService;


        public CmsController(ISitePublicAppService sitePublicAppService, ISectionPublicAppService sectionPublicAppService, IEntryPublicAppService entryPublicAppService)
        {
            LocalizationResource = typeof(CmsResource);
            _sectionPublicAppService = sectionPublicAppService;
            _entryPublicAppService = entryPublicAppService;
            _sitePublicAppService = sitePublicAppService;
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
        public async Task<IActionResult> CultureEntry(string culture, string url=null)
        {
            return await EntryViewResult(url, culture);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<IActionResult> Entry(string url=null)
        {
            return await EntryViewResult(url, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        protected async Task<IActionResult> EntryViewResult(string url = null, string language=null)
        {
            var section = await GetSection(url);
            var defaultLanguage = section.Site.GetDefaultLanguage();
            if (section == null)
            {
                return NotFound();
            }


            if (language.IsNullOrEmpty())
            {
                language = defaultLanguage;
            }

            var entry = await GetEntry(section, url, language);
            if (entry != null)
            {
                var viewModel = new EntryViewModel(entry, section);
                return View(section.Template, viewModel);
            }
            else
            {
                if (!language.Equals(defaultLanguage, StringComparison.OrdinalIgnoreCase))
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
            var site = await _sitePublicAppService.FindByHostAsync(hostUrl);

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

        protected async Task<EntryDto> GetEntry(SectionDto section, string url, string language)
        {
            EntryDto entry = null;
            // If the section type is single, then the slug value of the entry is the name of the section
            if (section.Type == Cms.Sections.SectionType.Single) 
            {
                var result = await _entryPublicAppService.GetListAsync(new GetEntriesInput { 
                    SectionId = section.Id,
                    SkipCount=0,
                    MaxResultCount=1,
                    Language = language
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
                        Language = language,
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
