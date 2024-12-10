using System.Collections.Generic;
using System.Linq;

namespace Dignite.Cms.Sites
{
    public class SiteDto
    {
        public SiteDto()
        {
            AllLanguages = Enumerable.Empty<string>();
        }

        public string DefaultLanguage { get; set; }

        public IEnumerable<string> AllLanguages { get; set; }
    }
}
