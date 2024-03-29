﻿using Dignite.Cms.Sites;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Dignite.Cms.Admin.Sites
{
    [Serializable]
    public class SiteDto : ExtensibleAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public SiteDto()
        {
            Languages = new List<SiteLanguageDto>();
        }

        /// <summary>
        /// Display Name of this site.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Name of this site.
        /// Site Unique Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Languages supported on this site
        /// </summary>
        public ICollection<SiteLanguageDto> Languages { get; set; }

        /// <summary>
        /// Host of this site
        /// </summary>
        public string Host { get; set; }


        /// <summary>
        /// Is this site active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ConcurrencyStamp { get; set; }
    }
}
