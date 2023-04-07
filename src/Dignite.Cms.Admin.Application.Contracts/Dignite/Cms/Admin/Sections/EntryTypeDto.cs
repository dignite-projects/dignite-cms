using Dignite.Cms.Sections;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Dignite.Cms.Admin.Sections
{
    public class EntryTypeDto: ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        /// <summary>
        /// Display Name of this entry type.
        /// </summary>
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Name of this entry type.
        /// Entry Type Unique Name.
        /// </summary>
        public virtual string Name { get; set; }

        public IList<EntryFieldTabDto> FieldTabs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ConcurrencyStamp { get; set; }
    }
}
