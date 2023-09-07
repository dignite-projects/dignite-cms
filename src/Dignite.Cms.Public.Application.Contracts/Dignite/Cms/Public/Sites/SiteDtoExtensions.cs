using JetBrains.Annotations;
using System.Linq;

namespace Dignite.Cms.Public.Sites
{
    public static class SiteDtoExtensions
    {
        public static string GetDefaultLanguage([NotNull] this SiteDto source)
        {
            return source.Languages.OrderByDescending(l => l.IsDefault).First().Language;
        }
    }
}
