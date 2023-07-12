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
        public async Task<IActionResult> CultureEntry(string culture, string url)
        {
            return await EntryViewResult(url, culture);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
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
            var viewModel = ObjectMapper.Map<EntryDto, EntryViewModel>(entry);
            viewModel.Section=section;
            return View(section.EntryPage.Template, viewModel);
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
            if (language.IsNullOrEmpty())
            {
                language = section.Site.Languages.OrderByDescending(l => l.IsDefault).First().Language;
            }

            // If the section type is single, then the slug value of the entry is the name of the section
            if (section.Type == Cms.Sections.SectionType.Single) 
            {
                var result = await _entryPublicAppService.GetListAsync(new GetEntriesInput { 
                    SectionId = section.Id,
                    SkipCount=0,
                    MaxResultCount=1,
                    Language = language
                });
                return result.Items.Any() ? result.Items[0] : null;
            }
            else
            {
                string slug = null;
                //Extract Slug value from URL
                var extractResult = FormattedStringValueExtracter.Extract(url.RemovePreFix("/").RemovePostFix("/"), section.EntryPage.Route.RemovePreFix("/").RemovePostFix("/"), ignoreCase: true);
                if (extractResult.IsMatch)
                {
                    slug = extractResult.Matches.First(m => m.Name.Equals(nameof(EntryDto.Slug), StringComparison.OrdinalIgnoreCase)).Value;
                    //
                    return await _entryPublicAppService.FindBySlugAsync(new FindBySlugInput
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
        }
    }
}
