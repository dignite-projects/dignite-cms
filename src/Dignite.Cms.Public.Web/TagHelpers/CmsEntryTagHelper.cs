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
        /// <summary>
        /// The culture corresponding to the entry
        /// </summary>
        public string Culture { get; set; }

        public string SectionName { get; set; }

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
        private readonly ISitePublicAppService _siteSettingsPublicAppService;

        public CmsEntryTagHelper(
            IRazorPartialRenderer renderer,
            IEntryPublicAppService entryAppService, 
            ISectionPublicAppService sectionAppService,
            ISitePublicAppService siteSettingsPublicAppService
            )
        {
            _renderer = renderer;
            _entryAppService = entryAppService;
            _sectionAppService = sectionAppService;
            _siteSettingsPublicAppService = siteSettingsPublicAppService;
        }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var section = await _sectionAppService.FindByNameAsync(SectionName);
            var site = await _siteSettingsPublicAppService.GetAsync();
            var defaultLanguageCulture = site.DefaultLanguage;
            if (Culture.IsNullOrEmpty())
            {
                Culture= defaultLanguageCulture;
            }

            var findEntryBySlugInput = new FindBySlugInput
            {
                SectionId = section.Id,
                Culture = Culture,
                Slug = Slug
            };
            var model = await _entryAppService.FindBySlugAsync(findEntryBySlugInput);
            if (model == null)
            {
                if (!Culture.Equals(defaultLanguageCulture, StringComparison.OrdinalIgnoreCase))
                {
                    findEntryBySlugInput.Culture = defaultLanguageCulture;
                    model = await _entryAppService.FindBySlugAsync(findEntryBySlugInput);
                }
            }

            if (model == null)
            {
                output.TagName = "p";
                output.Attributes.Add("class", "p-2 bg-warning text-dark");
                output.Content.SetContent($"No entries were found for Slug in {Culture} for {Slug}.");
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
