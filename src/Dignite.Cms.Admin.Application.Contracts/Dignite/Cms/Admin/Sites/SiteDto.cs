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
        /// Languages supported on this site
        /// </summary>
        public ICollection<SiteLanguageDto> Languages { get; set; }

        /// <summary>
        /// Base Url of this site
        /// </summary>
        public virtual string BaseUrl { get; set; }

        /// <summary>
        /// Is this site a default
        /// </summary>
        public virtual bool IsDefault { get; set; }


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
