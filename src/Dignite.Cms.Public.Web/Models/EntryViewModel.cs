using Dignite.Cms.Public.Entries;
using Dignite.Cms.Public.Sections;

namespace Dignite.Cms.Public.Web.Models
{
    public class EntryViewModel:EntryDto
    {
        public SectionDto Section { get; set; }
    }
}
