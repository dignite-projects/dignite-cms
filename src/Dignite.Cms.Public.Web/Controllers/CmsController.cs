using Dignite.Cms.Localization;
using Dignite.Cms.Public.Entries;
using Dignite.Cms.Public.Sections;
using Dignite.Cms.Public.Sites;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

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
        public IActionResult Index()
        {
            return RedirectToAction("Entry", new
            {
                culture = CultureInfo.CurrentUICulture.TextInfo.CultureName
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="url">
        /// There are several formats:
        /// 1.{section}
        /// 2.{section}/{slug}
        /// 3.{section}/{query-conditions}
        /// </param>
        /// <returns></returns>
        [HttpGet("/{culture:regex(zh-hans|en)}/{*url}")]
        public async Task<IActionResult> Entry(string culture, string url)
        {
            if (culture is null)
            {
                throw new ArgumentNullException(nameof(culture));
            }

            url = url ?? "/";
            var section=await GetSectionByUrl(url);
            var entry = await GetEntryByUrl(culture, section, url);
            ViewBag.Section=section;
            return View(section.EntryPage.Template, entry);
        }

        protected async Task<SectionDto> GetSectionByUrl(string url)
        {
            var site = await _sitePublicAppService.GetDefaultAsync();
            if (url.IsNullOrEmpty() || url == "/")
            {
                return await _sectionPublicAppService.GetDefaultAsync(site.Id);
            }
            else
            {
                return await _sectionPublicAppService.FindByUrlAsync(site.Id,url);
            }
        }

        protected async Task<EntryDto> GetEntryByUrl(string language, SectionDto section, string url)
        {
            if (section.Type == Cms.Sections.SectionType.Single)
            {
                return await _entryPublicAppService.FindBySlugAsync(new FindBySlugInput
                {
                    Language = language,
                    SectionId = section.Id,
                    Slug = section.Name
                });
            }
            else
            { 
                var urlNodes = url.Split('/');
                return await _entryPublicAppService.FindBySlugAsync(new FindBySlugInput
                {
                    Language = language,
                    SectionId = section.Id,
                    Slug = urlNodes[1]
                }); 
            }
        }
    }
}
