using Dignite.Abp.DynamicForms;
using Dignite.Cms.Entries;
using System;
using System.Text.Json.Serialization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;
using Volo.CmsKit.Users;

namespace Dignite.Cms.Admin.Entries
{
    /// <summary>
    /// Entry
    /// </summary>
    public class EntryDto: ExtensibleAuditedEntityDto<Guid>, IHasCustomFields, IHasConcurrencyStamp
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual Guid SectionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Guid EntryTypeId { get; set;}

        /// <summary>
        /// The language corresponding to the entry
        /// </summary>
        public virtual string Language { get; set; }

        public virtual string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Slug { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime PublishTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual EntryStatus Status { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CustomFieldDictionary CustomFields { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EntryRevisionDto Revision { get; set; }

        #region Section type is a exclusive property of Structure type

        /// <summary>
        /// Parent entry id of the entry;
        /// When it is affiliated with <see cref="Sections.SectionDto.Type"/>=<see cref="Cms.Sections.SectionType.Structure"/>, this value is valid
        /// </summary>
        public virtual Guid? ParentId { get; set; }

        /// <summary>
        /// Order of the entry
        /// When it is affiliated with <see cref="Sections.SectionDto.Type"/>=<see cref="Cms.Sections.SectionType.Structure"/>, this value is valid
        /// </summary>
        public virtual int Order { get; set; }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CmsUserDto Author { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public string ConcurrencyStamp { get; set; }
    }
}
