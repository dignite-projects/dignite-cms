using Dignite.Abp.Data;
using System;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Dignite.Cms.Entries
{
    /// <summary>
    /// Entry
    /// </summary>
    public class Entry: FullAuditedAggregateRoot<Guid>, IHasCustomFields, IMultiTenant
    {
        protected Entry()
        {
        }

        public Entry(
            Guid id, 
            Guid sectionId, 
            Guid entryTypeId, 
            string culture, 
            string title, 
            string slug, 
            DateTime publishTime, 
            EntryStatus status,
            ExtraPropertyDictionary extraProperties,
            Guid? parentId, 
            int order, 
            EntryRevision revision, 
            Guid? tenantId)
            :base(id)
        {
            SectionId = sectionId;
            EntryTypeId = entryTypeId;
            Culture = culture;
            Title = title;
            Slug = slug;
            PublishTime = publishTime;
            Status = status;
            ExtraProperties= extraProperties;
            ParentId = parentId;
            Order = order;
            Revision = revision;
            TenantId = tenantId;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Guid SectionId { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Guid EntryTypeId { get; protected set;}

        /// <summary>
        /// The Culture corresponding to the entry
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
        public virtual EntryStatus Status { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual EntryRevision Revision { get; protected set; }

        #region Section type is a exclusive property of Structure type

        /// <summary>
        /// Parent entry id of the entry;
        /// When it is affiliated with <see cref="Sections.Section.Type"/>=<see cref="Sections.SectionType.Structure"/>, this value is valid
        /// </summary>
        public virtual Guid? ParentId { get; protected set; }

        /// <summary>
        /// Order of the entry
        /// When it is affiliated with <see cref="Sections.Section.Type"/>=<see cref="Sections.SectionType.Structure"/>, this value is valid
        /// </summary>
        public virtual int Order { get; protected set; }

        #endregion


        public virtual Guid? TenantId { get; protected set; }


        public virtual void SetEntryTypeId(Guid entryTypeId)
        {
            this.EntryTypeId = entryTypeId;
        }
        public virtual void SetParentId(Guid? parentId)
        {
            this.ParentId = parentId;
        }
        public virtual void SetStatus(EntryStatus status)
        {
            this.Status = status;
        }

        public virtual void SetOrder(int order)
        {
            this.Order = order;
        }
    }
}
