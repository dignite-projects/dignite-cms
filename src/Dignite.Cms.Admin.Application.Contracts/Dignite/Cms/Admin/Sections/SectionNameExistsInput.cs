using System;
using System.ComponentModel.DataAnnotations;

namespace Dignite.Cms.Admin.Sections
{
    public class SectionNameExistsInput
    {
        public SectionNameExistsInput()
        {
        }

        public SectionNameExistsInput(Guid siteId, string name)
        {
            SiteId = siteId;
            Name = name;
        }

        [Required]
        public Guid SiteId { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
