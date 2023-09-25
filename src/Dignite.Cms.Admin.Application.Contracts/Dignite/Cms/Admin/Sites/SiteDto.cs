using Dignite.Cms.Sites;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Dignite.Cms.Admin.Sites
{
    [Serializable]
    public class SiteDto : ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        /// <summary>
        /// Display Name of this site.
        /// </summary>
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Name of this site.
        /// Site Unique Name.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// cultures supported on this site
        /// </summary>
        public ICollection<SiteCultureDto> Cultures { get; set; }

        /// <summary>
        /// Host Url of this site
        /// </summary>
        public virtual string HostUrl { get; set; }


        /// <summary>
        /// Is this site active
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ConcurrencyStamp { get; set; }
    }
}
