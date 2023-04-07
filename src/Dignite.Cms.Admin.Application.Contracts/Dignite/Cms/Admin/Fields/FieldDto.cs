using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Dignite.Cms.Admin.Fields
{
    public class FieldDto: Cms.Fields.FieldDto, IAuditedObject, IHasConcurrencyStamp
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual Guid? GroupId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ConcurrencyStamp { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }


        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}
