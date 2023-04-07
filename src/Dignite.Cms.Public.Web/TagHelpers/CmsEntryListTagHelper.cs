using Dignite.Cms.Public.Entries;
using Dignite.Cms.Public.Sections;
using Dignite.Cms.Public.Sites;
using Dignite.Cms.Public.Web.Razor;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dignite.Cms.Public.Web.TagHelpers
{
    public class CmsEntryListTagHelper: TagHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public SectionDto Section { get; set; }

        /// <summary>
        /// If no <see cref="Section"/> is specified, entries can be queried by <see cref="SectionName"/>
        /// </summary>

        public Guid? SiteId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string SiteName { get; set; }

        /// <summary>
        /// If no <see cref="Section"/> is specified, entries can be queried by <see cref="SectionName"/>
        /// </summary>
        public string SectionName{ get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string QueryingByFieldParameters { get; set; }

        /// <summary>
        /// The name or path of the view that is rendered to the response.
        /// </summary>
        public string PartialName { get; set; }


        /// <summary>
        /// number of returned results;
        /// When the <see cref="Section"/> type is SectionType.Channel, the number of returned results must be designed
        /// </summary>
        public int? ResultCount { get; set; }

        /// <summary>
        /// number of data paged
        /// default value : 1
        /// </summary>
        public int CurrentPage { get; set; }


        private readonly IRazorPartialRenderer _renderer;
        private readonly IEntryPublicAppService _entryAppService;
        private readonly ISectionPublicAppService _sectionAppService;
        private readonly ISitePublicAppService _siteAppService;

        public CmsEntryListTagHelper(
            IRazorPartialRenderer renderer,
            IEntryPublicAppService entryAppService,
            ISectionPublicAppService sectionAppService,
            ISitePublicAppService siteAppService
            )
        {
            _renderer = renderer;
            _entryAppService = entryAppService;
            _sectionAppService = sectionAppService;
            _siteAppService = siteAppService;
            CurrentPage = 1;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Section == null)
            {
                if ((!SiteId.HasValue && SiteName.IsNullOrEmpty()) || SectionName.IsNullOrEmpty())
                {
                    output.TagName = "p";
                    output.Attributes.Add("class", "p-2 bg-warning text-dark");
                    output.Content.SetContent($"Please set the value of Section, or set the values of SiteId and SectionName");
                    return;
                }

                if (!SiteId.HasValue && !SiteName.IsNullOrEmpty())
                {
                    var site = await _siteAppService.FindByNameAsync(SiteName);
                    SiteId = site.Id;
                }

                Section = await _sectionAppService.FindByNameAsync(SiteId.Value, SectionName);
            }

            var model = await GetViewModel();
            var body = await _renderer.RenderAsync(PartialName, model);

            output.Content.SetHtmlContent(body);
            output.Attributes.Clear();
        }

        protected async Task<EntryListViewModel> GetViewModel()
        {
            var result = await _entryAppService.GetListAsync(new GetEntriesInput
            {
                SectionId = Section.Id,
                Language = Language,
                QueryingByFieldParameters = QueryingByFieldParameters,
                MaxResultCount =this.ResultCount.Value,
                SkipCount = (this.CurrentPage-1) * ResultCount.Value
            });


            var model = new EntryListViewModel(Section, result.Items, (int)result.TotalCount, CurrentPage, ResultCount.Value);
            return model;
        }
    }
}
