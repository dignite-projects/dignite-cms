using Dignite.Cms.Admin.Sites;
using Dignite.Cms.Sections;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Dignite.Cms.Admin.Sections
{
    [Serializable]
    public class SectionDto : ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual Guid SiteId { get; set; }

        public SiteDto Site { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual SectionType Type { get; set; }

        /// <summary>
        /// Display Name of this section.
        /// </summary>
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Name of this section.
        /// Section Unique Name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// The default section in the site
        /// </summary>
        public virtual bool IsDefault { get; set; }

        /// <summary>
        /// Is this section active
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Entry Page of this section
        /// </summary>
        public virtual EntryPageDto EntryPage { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ConcurrencyStamp { get; set; }


        public virtual ICollection<EntryTypeDto> EntryTypes { get; set; }
    }
}
