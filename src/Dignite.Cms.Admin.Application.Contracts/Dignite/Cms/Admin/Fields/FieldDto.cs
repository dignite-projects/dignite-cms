using System;
using Volo.Abp.Auditing;

namespace Dignite.Cms.Admin.Fields
{
    public class FieldDto: Cms.Fields.FieldDto, IAuditedObject
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid? GroupId { get; set; }

        public string GroupName { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }


        public Guid? LastModifierId { get; set; }

        public DateTime? LastModificationTime { get; set; }

    }
}
