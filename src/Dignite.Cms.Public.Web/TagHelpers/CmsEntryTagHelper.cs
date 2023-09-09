﻿using Dignite.Cms.Public.Entries;
using Dignite.Cms.Public.Sections;
using Dignite.Cms.Public.Sites;
using Dignite.Cms.Public.Web.Models;
using Dignite.Cms.Public.Web.Razor;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Threading.Tasks;

namespace Dignite.Cms.Public.Web.TagHelpers
{
    public class CmsEntryTagHelper : TagHelper
    {
        public Guid SiteId { get; set; }

        /// <summary>
        /// If no <see cref="Section"/> is specified, entries can be queried by <see cref="SectionName"/>
        /// </summary>
        public string SectionName { get; set; }

        /// <summary>
        /// The language corresponding to the entry
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// The name or path of the view that is rendered to the response.
        /// </summary>
        public string PartialName { get; set; }


        private readonly IRazorPartialRenderer _renderer;
        private readonly IEntryPublicAppService _entryAppService;
        private readonly ISectionPublicAppService _sectionAppService;

        public CmsEntryTagHelper(
            IRazorPartialRenderer renderer,
            IEntryPublicAppService entryAppService, 
            ISectionPublicAppService sectionAppService
            )
        {
            _renderer = renderer;
            _entryAppService = entryAppService;
            _sectionAppService = sectionAppService;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var section = await _sectionAppService.FindByNameAsync(SiteId, SectionName);
            var defaultLanguage = section.Site.GetDefaultLanguage();
            if (Language.IsNullOrEmpty())
            {
                Language= defaultLanguage;
            }

            var findEntryBySlugInput = new FindBySlugInput
            {
                SectionId = section.Id,
                Language = Language,
                Slug = Slug
            };
            var model = await _entryAppService.FindBySlugAsync(findEntryBySlugInput);
            if (model == null)
            {
                if (!Language.Equals(defaultLanguage, StringComparison.OrdinalIgnoreCase))
                {
                    findEntryBySlugInput.Language = defaultLanguage;
                    model = await _entryAppService.FindBySlugAsync(findEntryBySlugInput);
                }
            }

            if (model == null)
            {
                output.TagName = "p";
                output.Attributes.Add("class", "p-2 bg-warning text-dark");
                output.Content.SetContent($"No entries were found for Slug in language {Language} for {Slug}.");
            }
            else
            {
                var body = await _renderer.RenderAsync(PartialName, new EntryViewModel(model, section));

                output.TagName = null;
                output.Content.SetHtmlContent(body);
                output.Attributes.Clear();
            }
        }
    }
}
