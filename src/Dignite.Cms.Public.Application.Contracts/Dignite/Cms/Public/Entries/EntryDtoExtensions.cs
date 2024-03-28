using Dignite.Cms.Entries;
using Dignite.Cms.Public.Sections;
using Dignite.Cms.Public.Sites;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Dignite.Cms.Public.Entries
{
    public static class EntryDtoExtensions
    {
        public static string GetUrl([NotNull] this EntryDto source, SectionDto section)
        {
            var routeParameters = GetRouteParameters(section.Route).ToArray();
            var siteDefaultLanguage = section.Site.GetDefaultLanguage();
            string url = section.Route;

            //If there is a routing parameter, get the routing parameter value and update the URL
            if (routeParameters.Any())
            {
                foreach (string routePerameter in routeParameters)
                {
                    var routeParameterName = routePerameter.RemovePreFix("{").RemovePostFix("}");
                    if (routeParameterName.IndexOf(':') > -1)
                    {
                        var propertyName = routeParameterName.Split(':')[0];
                        var parameterFormat = $"{{0:{routeParameterName.Split(':')[1]}}}";
                        var propertyValue = GetPropertyValue(source, propertyName);
                        url = url.Replace(routePerameter, string.Format(parameterFormat, propertyValue));
                    }
                    else
                    {
                        var propertyValue = GetPropertyValue(source, routeParameterName);
                        url = url.Replace(routePerameter, propertyValue.ToString());
                    }
                }
            }

            //splice Culture path
            if (!siteDefaultLanguage.CultureName.Equals(source.Culture, StringComparison.OrdinalIgnoreCase))
            {
                url = source.Culture + url.EnsureStartsWith('/');
            }

            url = section.Site.Host.EnsureEndsWith('/') + url.RemovePreFix("/").RemovePostFix($"/{EntryConsts.DefaultSlug}");

            return url;
        }

        private static IEnumerable<string> GetRouteParameters(string route)
        {
            Regex regex = new Regex(@"\{[a-zA-Z][\w:\-.\/]*\}");
            var matchCollection = regex.Matches(route);

            for (int i = 0; i < matchCollection.Count; i++)
            {
                yield return matchCollection[i].Groups[0].Value;
            }
        }

        private static object GetPropertyValue(EntryDto entry, string propertyName)
        {
            Type type = entry.GetType();
            var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance);
            if (property != null)
            {
                return property.GetValue(entry, new object[0]);
            }
            else
            {
                throw new Volo.Abp.AbpException($"The entry property corresponding to the routing parameter {propertyName} was not found in the entry");
            }
        }
    }
}
