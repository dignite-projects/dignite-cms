using Dignite.Cms.Public.Sites;
using Dignite.Cms.Sections;
using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Dignite.Cms.Public.Sections
{
    [Serializable]
    public class SectionDto : ExtensibleEntityDto<Guid>
    {
        /// <summary>
        /// SiteId of this section.
        /// </summary>
        public Guid SiteId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SiteDto Site { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SectionType Type { get; set; }

        /// <summary>
        /// Display Name of this section.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Name of this section.
        /// Section Unique Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Is this section a active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Entry Page of this section
        /// </summary>
        public EntryPageDto EntryPage { get; set; }


        public ICollection<EntryTypeDto> EntryTypes { get; set; }
    }
}
