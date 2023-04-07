using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Dignite.Cms.Sections
{
    public class EntryType: FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public EntryType(Guid id, Guid sectionId, string displayName, string name, IList<EntryFieldTab> fieldTabs, Guid? tenantId)
            :base(id)
        {
            SectionId = sectionId;
            DisplayName = displayName;
            Name = name;
            FieldTabs = fieldTabs;
            TenantId = tenantId;
            FieldTabs= fieldTabs;
        }

        protected EntryType()
        {
            this.FieldTabs=new List<EntryFieldTab>();
        }

        /// <summary>
        /// 
        /// </summary>
        public Guid SectionId { get; protected set; }

        /// <summary>
        /// Display Name of this entry type.
        /// </summary>
        public virtual string DisplayName { get; protected set; }

        /// <summary>
        /// Name of this entry type.
        /// Entry Type Unique Name.
        /// </summary>
        public virtual string Name { get; protected set; }

        public IList<EntryFieldTab> FieldTabs { get; protected set; }

        /// <summary>
        /// TenantId of this entry type.
        /// </summary>
        public virtual Guid? TenantId { get; protected set; }

        public virtual void SetDisplayName(string displayName)
        {
            DisplayName = displayName;
        }
        public virtual void SetName(string name)
        {
            Name = name;
        }
    }
}
