using System.Collections.Generic;

namespace Dignite.Cms.Sections
{
    public class EntryFieldTabDto
    {
        public string Name { get; set; }

        public IList<EntryFieldDto> Fields { get; set; }
    }
}
