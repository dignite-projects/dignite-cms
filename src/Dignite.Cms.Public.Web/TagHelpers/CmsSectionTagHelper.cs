﻿using Dignite.Cms.Public.Sections;
using Dignite.Cms.Public.Web.Razor;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;

namespace Dignite.Cms.Public.Web.TagHelpers
{
    public class CmsSectionTagHelper : TagHelper
    {
        /// <summary>
        /// If no <see cref="Section"/> is specified, entries can be queried by <see cref="SectionName"/>
        /// </summary>

        public Guid SiteId { get; set; }

        /// <summary>
        /// If no <see cref="Section"/> is specified, entries can be queried by <see cref="SectionName"/>
        /// </summary>
        public string SectionName{ get; set; }

        /// <summary>
        /// The name or path of the view that is rendered to the response.
        /// </summary>
        public string PartialName { get; set; }


        private readonly IRazorPartialRenderer _renderer;
        private readonly ISectionPublicAppService _sectionAppService;

        public CmsSectionTagHelper(
            IRazorPartialRenderer renderer,
            ISectionPublicAppService sectionAppService
            )
        {
            _renderer = renderer;
            _sectionAppService = sectionAppService;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var model = await _sectionAppService.FindByNameAsync(SiteId,SectionName);
            var body = await _renderer.RenderAsync(PartialName, model);

            output.Content.SetHtmlContent(body);
            output.Attributes.Clear();
        }
    }
}
