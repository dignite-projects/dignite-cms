﻿using Dignite.Cms.Entries;
using Dignite.Cms.Sites;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Dignite.Cms.Public.Entries
{
    public class FindBySlugInput
    {
        [Required]
        public Guid SectionId { get; set; }

        /// <summary>
        /// The language corresponding to the entry
        /// </summary>
        [DynamicMaxLength(typeof(SiteConsts), nameof(SiteConsts.MaxLanguageLength))]
        public string Language { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DynamicMaxLength(typeof(EntryConsts), nameof(EntryConsts.MaxSlugLength))]
        [RegularExpression(EntryConsts.SlugRegularExpression)]
        public string Slug { get; set; }
    }
}
