﻿using Dignite.Abp.DynamicForms;
using Dignite.Cms.Entries;
using System;
using System.Text.Json.Serialization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Auditing;
using Volo.CmsKit.Users;

namespace Dignite.Cms.Public.Entries
{
    /// <summary>
    /// Entry
    /// </summary>
    public class EntryDto: ExtensibleEntityDto<Guid>, IHasCustomFields, IMayHaveCreator
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid SectionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid EntryTypeId { get; set;}

        /// <summary>
        /// The language corresponding to the entry
        /// </summary>
        public string Language { get; set; }

        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EntryStatus Status { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime PublishTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CustomFieldDictionary CustomFields { get; set; }


        /// <summary>
        /// Parent entry id of the entry;
        /// When it is affiliated with <see cref="Sections.SectionDto.Type"/>=<see cref="Cms.Sections.SectionType.Structure"/>, this value is valid
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Order of the entry
        /// When it is affiliated with <see cref="Sections.SectionDto.Type"/>=<see cref="Cms.Sections.SectionType.Structure"/>, this value is valid
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? CreatorId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CmsUserDto Author { get; set; }
    }
}
