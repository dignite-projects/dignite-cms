using Dignite.Cms.Public.Entries;
using Dignite.Cms.Public.Sections;
using Dignite.Cms.Public.Sites;
using Dignite.Cms.Public.Web.Controllers;
using Dignite.Cms.Public.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace Dignite.Cms.Public.Web.TagHelpers
{
    /// <summary>
    /// Tag helper for linking to entry
    /// </summary>
    [HtmlTargetElement("a", Attributes = "[entry]")]
    public class LinkToEntryTagHelper : TagHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public EntryViewModel Entry { get; set; }= null;


        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var entryUrl = GetEntryUrl(Entry.Entry,Entry.Section);

            output.Attributes.SetAttribute("href", entryUrl);
        }

        /// <summary>
        /// set entry url
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="section"></param>
        private string GetEntryUrl(EntryDto entry, SectionDto section)
        {
            var hostAddress = ViewContext.HttpContext.Request.Scheme + "://" + ViewContext.HttpContext.Request.Host;
            var urlHelper = ViewContext.GetUrlHelper();
            var routeParameters = GetRouteParameters(section.Route).ToArray();
            string siteDefaultLanguageCulture = section.Site.GetDefaultLanguage().CultureName;
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
                        var propertyValue = GetPropertyValue(entry, propertyName);
                        url = url.Replace(routePerameter, string.Format(parameterFormat, propertyValue));
                    }
                    else
                    {
                        var propertyValue = GetPropertyValue(entry, routeParameterName);
                        url = url.Replace(routePerameter, propertyValue.ToString());
                    }
                }
            }

            //splice Culture path
            if (siteDefaultLanguageCulture.Equals(entry.Culture, StringComparison.OrdinalIgnoreCase))
            {
                url = urlHelper.ActionLink(nameof(CmsController.Entry), CmsController.ControllerName, new { url });
            }
            else
            {
                url = urlHelper.ActionLink(nameof(CmsController.EntryByCulture), CmsController.ControllerName, new { culture = entry.Culture, url });
            }

            //Since the URL is encoded in the ActionLink method, it is decoded here
            url = HttpUtility.UrlDecode(url).RemovePreFix(hostAddress);

            //Splicing the site's host
            if (!section.Site.Host.Equals(hostAddress, StringComparison.OrdinalIgnoreCase))
            {
                url = hostAddress + url.StartsWith('/');
            }
            return url;
        }

        /// <summary>
        /// Get Route Parameters
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        private IEnumerable<string> GetRouteParameters(string route)
        {
            Regex regex = new Regex(@"\{[a-zA-Z][\w:\-.\/]*\}");
            var matchCollection = regex.Matches(route);

            for (int i = 0; i < matchCollection.Count; i++)
            {
                yield return matchCollection[i].Groups[0].Value;
            }
        }

        /// <summary>
        /// Using reflection to get the value of an entry property
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <exception cref="Volo.Abp.AbpException"></exception>
        private object GetPropertyValue(EntryDto entry, string propertyName)
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
