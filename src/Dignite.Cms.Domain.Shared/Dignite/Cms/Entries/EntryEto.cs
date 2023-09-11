using Dignite.Abp.DynamicForms;
using System;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace Dignite.Cms.Entries
{
    [EventName("Dignite.Cms.Entries.Entry")]
    [Serializable]
    public class EntryEto: IHasCustomFields, IMultiTenant
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Guid SectionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Guid EntryTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Region { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime PublishTime { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public virtual EntryStatus Status { get; protected set; }


        public CustomFieldDictionary CustomFields { get; set; }


        /// <summary>
        /// 
        /// </summary>

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

        public virtual Guid? TenantId { get; protected set; }
    }
}
