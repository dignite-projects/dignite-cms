using Dignite.Abp.Data;
using Dignite.Cms.Public.Entries;
using Dignite.Cms.Public.Sections;
using Dignite.Cms.Public.Sites;
using Dignite.Cms.Public.Web.Models;
using Dignite.Cms.Public.Web.Razor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Text.Json;
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
        public string Culture { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IList<QueryingByCustomField> QueryingByCustomFields { get; set; }

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
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CmsEntryListTagHelper(
            IRazorPartialRenderer renderer,
            IEntryPublicAppService entryAppService,
            ISectionPublicAppService sectionAppService,
            ISitePublicAppService siteAppService,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _renderer = renderer;
            _entryAppService = entryAppService;
            _sectionAppService = sectionAppService;
            _siteAppService = siteAppService;
            _httpContextAccessor = httpContextAccessor;
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
                if (Section==null)
                {
                    output.TagName = "p";
                    output.Attributes.Add("class", "p-2 bg-warning text-dark");
                    output.Content.SetContent($"The section named {SectionName} is null. Please check if the {SectionName} name is valid");
                    return;
                }
            }

            var model = await GetViewModel();
            var body = await _renderer.RenderAsync(PartialName, model);

            output.TagName = null;
            output.Content.SetHtmlContent(body);
            output.Attributes.Clear();
        }

        protected async Task<EntryListViewModel> GetViewModel()
        {
            var result = await _entryAppService.GetListAsync(new GetEntriesInput
            {
                SectionId = Section.Id,
                Culture = Culture,
                QueryingByCustomFieldsJson = QueryingByCustomFields == null ? null : JsonSerializer.Serialize(QueryingByCustomFields),
                MaxResultCount = this.ResultCount.Value,
                SkipCount = (this.CurrentPage - 1) * ResultCount.Value
            });


            result.Items.ForEach(entry => SetEntryUrl(entry));
            var model = new EntryListViewModel(Section, result.Items, (int)result.TotalCount, CurrentPage, ResultCount.Value);
            return model;
        }

        protected void SetEntryUrl(EntryDto entry)
        {
            var hostAddress = _httpContextAccessor.HttpContext.Request.Scheme + "://" + _httpContextAccessor.HttpContext.Request.Host;
            if (entry.Url.StartsWith(hostAddress, StringComparison.OrdinalIgnoreCase))
            {
                entry.Url = entry.Url.RemovePreFix(StringComparison.OrdinalIgnoreCase, hostAddress);
            }
        }
    }
}
