using Dignite.Cms.Sites;
using JetBrains.Annotations;
using System;
using System.Linq;

namespace Dignite.Cms.Public.Sites
{
    public static class SiteDtoExtensions
    {
        public static SiteLanguageDto GetDefaultLanguage([NotNull] this SiteDto source)
        {
            return source.Languages.OrderByDescending(l => l.IsDefault).FirstOrDefault();
        }
        public static bool LanguageCultureExists([NotNull] this SiteDto source,string culture)
        {
            return source.Languages.Any(r=>r.CultureName.Equals(culture,StringComparison.InvariantCultureIgnoreCase));
        }        
    }
}
