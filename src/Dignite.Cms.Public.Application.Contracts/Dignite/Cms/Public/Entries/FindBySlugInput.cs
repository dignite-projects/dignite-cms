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
        /// The region corresponding to the entry
        /// </summary>
        [DynamicMaxLength(typeof(SiteConsts), nameof(SiteConsts.MaxRegionLength))]
        public string Region { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DynamicMaxLength(typeof(EntryConsts), nameof(EntryConsts.MaxSlugLength))]
        [RegularExpression(EntryConsts.SlugRegularExpression)]
        public string Slug { get; set; }
    }
}
