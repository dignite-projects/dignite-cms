using System;

namespace Dignite.Cms.Sites
{
    [Serializable]
    public class SiteLanguageDto
    {
        public bool IsDefault { get; set; }

        public string CultureName { get; set; }
    }
}
