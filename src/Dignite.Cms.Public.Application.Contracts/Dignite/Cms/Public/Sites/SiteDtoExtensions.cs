using JetBrains.Annotations;
using System;
using System.Linq;

namespace Dignite.Cms.Public.Sites
{
    public static class SiteDtoExtensions
    {
        public static string GetDefaultCulture([NotNull] this SiteDto source)
        {
            return source.Cultures.OrderByDescending(l => l.IsDefault).First().CultureName;
        }
        public static bool CultureExists([NotNull] this SiteDto source,string culture)
        {
            return source.Cultures.Any(r=>r.CultureName.Equals(culture,StringComparison.OrdinalIgnoreCase));
        }        
    }
}
