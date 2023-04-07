using System;

namespace Dignite.Cms.Admin.Sections
{
    public class GetSectionsInput
    {
        public Guid SiteId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Filter { get; set; }

        public bool? IsActive { get; set; }
    }
}
