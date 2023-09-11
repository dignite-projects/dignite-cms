using JetBrains.Annotations;
using System;
using System.Linq;

namespace Dignite.Cms.Public.Sites
{
    public static class SiteDtoExtensions
    {
        public static string GetDefaultRegion([NotNull] this SiteDto source)
        {
            return source.Regions.OrderByDescending(l => l.IsDefault).First().Region;
        }
        public static bool RegionExists([NotNull] this SiteDto source,string region)
        {
            return source.Regions.Any(r=>r.Region.Equals(region,StringComparison.OrdinalIgnoreCase));
        }        
    }
}
