using Dignite.Abp.Data;
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
        /// The culture corresponding to the entry
        /// </summary>
        public virtual string Culture { get; set; }

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

        #region Version control information for entry


        /// <summary>
        /// The id of the initial entry id;
        /// </summary>
        /// <remarks>
        /// The Id of the initial version is null;
        /// </remarks>
        public virtual Guid? InitialVersionId { get; set; }

        /// <summary>
        /// Whether it is an activated version
        /// </summary>
        public virtual bool IsActivatedVersion { get; set; }

        /// <summary>
        /// Notes on changes to this version
        /// </summary>
        public virtual string VersionNotes { get; set; }

        #endregion


        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CmsUserDto Author { get; set; }
        public string ConcurrencyStamp { get; set; }
    }
}
