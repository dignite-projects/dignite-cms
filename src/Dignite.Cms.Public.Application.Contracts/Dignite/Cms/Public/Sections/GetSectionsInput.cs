using System;
using System.ComponentModel.DataAnnotations;

namespace Dignite.Cms.Public.Sections
{
    public class GetSectionsInput
    {
        [Required]
        public Guid SiteId { get; set; }
    }
}
