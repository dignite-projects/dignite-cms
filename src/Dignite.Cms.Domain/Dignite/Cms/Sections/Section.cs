using Dignite.Cms.Entries;
using Dignite.Cms.Sites;
using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Dignite.Cms.Sections
{
    /// <summary>
    /// Site content section
    /// </summary>
    public class Section : FullAuditedAggregateRoot<Guid>,IMultiTenant
    {
        public Section(Guid id, Guid siteId, SectionType type, string displayName, string name, bool isDefault, bool isActive, EntryPage entryPage, Guid? tenantId)
            :base(id)
        {
            SiteId = siteId;
            Type = type;
            DisplayName = displayName;
            Name = name;
            IsDefault = isDefault;
            IsActive = isActive;
            EntryPage = entryPage;
            TenantId = tenantId;
            this.EntryTypes = new List<EntryType>();
        }

        protected Section()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Guid SiteId { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual SectionType Type { get; protected set; }

        /// <summary>
        /// Display Name of this section.
        /// </summary>
        public virtual string DisplayName { get; protected set; }

        /// <summary>
        /// Name of this section.
        /// Section Unique Name.
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// The default section in the site
        /// </summary>
        public virtual bool IsDefault { get; protected set; }

        /// <summary>
        /// Is this section active
        /// </summary>
        public virtual bool IsActive { get; protected set; }

        /// <summary>
        /// Entry Page of this section
        /// </summary>
        public virtual EntryPage EntryPage { get; protected set; }

        /// <summary>
        /// TenantId of this section.
        /// </summary>
        public virtual Guid? TenantId { get; protected set; }

        public virtual Site Site { get; protected set; }

        public virtual ICollection<EntryType> EntryTypes { get; protected set; }


        public virtual ICollection<Entry> Entries { get; protected set; }


        public virtual void SetActive(bool isActive)
        {
            if (IsDefault && !isActive)
            {
                throw new DefaultSectionCannotSetNotActiveException(DisplayName);
            }
            IsActive = isActive;
        }
        public virtual void SetDefault(bool isDefault)
        {
            IsDefault = isDefault;
        }
        public virtual void SetDisplayName(string displayName)
        {
            DisplayName = displayName;
        }
        public virtual void SetName(string name)
        {
            Name = name;
        }
        public virtual void SetEntryPage(EntryPage entryPage)
        {
            EntryPage = entryPage;
        }
        public virtual void SetType(SectionType type)
        {
            Type = type;
        }
    }
}
