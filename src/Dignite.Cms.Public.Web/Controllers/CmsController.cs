using Dignite.Cms.Localization;
using Dignite.Cms.Public.Entries;
using Dignite.Cms.Public.Sections;
using Dignite.Cms.Public.Sites;
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

        [HttpGet("/")]
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
        [HttpGet("/{culture:culture}/{*url}")]
        public async Task<IActionResult> Entry(string culture, string url)
        {
            return await EntryViewResult(url, culture);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [HttpGet("/{*url}")]
        public async Task<IActionResult> Entry(string url)
        {
            return await EntryViewResult(url, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        protected async Task<IActionResult> EntryViewResult( string url, string language)
        {
            var section = await GetSection(url);
            var entry = await GetEntry(section, url, language);
            ViewBag.Section = section;
            return View(section.EntryPage.Template, entry);
        }

        protected async Task<SectionDto> GetSection(string url)
        {
            if (url.IsNullOrEmpty() || url == "/")
            {
                var site = await _sitePublicAppService.GetDefaultAsync();
                return await _sectionPublicAppService.GetDefaultAsync(site.Id);
            }
            else
            {
                return await _sectionPublicAppService.FindByUrlAsync(url);
            }
        }

        protected async Task<EntryDto> GetEntry(SectionDto section, string url, string language = null)
        {
            string slug = null;
            if (language.IsNullOrEmpty())
            {
                language = section.Site.Languages.OrderByDescending(l => l.IsDefault).First().Language;
            }

            if (section.Type == Cms.Sections.SectionType.Single) // If the section type is single, then the slug value of the entry is the name of the section
            {
                slug = section.Name;
            }
            else
            {
                //Extract Slug value from URL
                var extractResult = FormattedStringValueExtracter.Extract(url.RemovePreFix("/"), section.EntryPage.Route.RemovePreFix("/"), ignoreCase: true);
                if (extractResult.IsMatch)
                {
                    slug = extractResult.Matches.First(m => m.Name.Equals(nameof(EntryDto.Slug), StringComparison.OrdinalIgnoreCase)).Value;
                }
                else
                {
                    throw new Volo.Abp.AbpException($"The structure type section and channel type section route of the entry must contain {{slug}}");
                }
            }

            //
            return await _entryPublicAppService.FindBySlugAsync(new FindBySlugInput
            {
                Language = language,
                SectionId = section.Id,
                Slug = slug
            });
        }
    }
}
