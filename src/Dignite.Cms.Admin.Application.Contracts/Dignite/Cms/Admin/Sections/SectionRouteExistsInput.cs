using System;
using System.ComponentModel.DataAnnotations;

namespace Dignite.Cms.Admin.Sections
{
    public class SectionRouteExistsInput
    {
        public SectionRouteExistsInput()
        {
        }

        public SectionRouteExistsInput(Guid siteId, string route)
        {
            SiteId = siteId;
            Route = route;
        }

        [Required]
        public Guid SiteId { get; set; }
        [Required]
        public string Route { get; set; }
    }
}
