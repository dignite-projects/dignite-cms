﻿using Dignite.Cms.Sites;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Dignite.Cms.Public.Sites
{
    [Serializable]
    public class SiteDto : ExtensibleEntityDto<Guid>
    {
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
        /// Cultures supported on this site
        /// </summary>
        public ICollection<SiteCultureDto> Cultures { get; set; }

        /// <summary>
        /// Host of this site
        /// </summary>
        public string HostUrl { get; set; }

        /// <summary>
        /// Is this site a active
        /// </summary>
        public bool IsActive { get; set; }
    }
}
