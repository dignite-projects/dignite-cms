using Dignite.Abp.DynamicForms;
using Dignite.Abp.FieldCustomizing;
using System;
using Volo.Abp.Auditing;
using Volo.Abp.MultiTenancy;

namespace Dignite.Cms.Fields
{
    public class Field: CustomizeFieldDefinitionBase, IFullAuditedObject,IMultiTenant
    {
        public Field(
            Guid id,
            string displayName,
            string name,
            string defaultValue,
            string formName, 
            FormConfigurationDictionary formConfiguration, 
            Guid? groupId, 
            Guid? tenantId)
            :base( id, displayName,name,defaultValue,formName,formConfiguration)
        {
            GroupId = groupId;
            TenantId = tenantId;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual Guid? GroupId { get; protected set; }

        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }

        public Guid? LastModifierId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? DeleterId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public bool IsDeleted { get; set; }

        public Guid? TenantId { get; set; }

        public virtual void SetGroupId(Guid? groupId)
        {
            this.GroupId= groupId;
        }
    }
}
