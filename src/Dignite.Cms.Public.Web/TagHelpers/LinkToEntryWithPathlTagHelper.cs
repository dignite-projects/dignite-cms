using Dignite.Cms.Public.Web.Routing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using System;

namespace Dignite.Cms.Public.Web.TagHelpers
{
    /// <summary>
    /// Link to entry
    /// </summary>
    [HtmlTargetElement("a", Attributes = "[entry-path]")]
    public class LinkToEntryWithPathlTagHelper : TagHelper
    {
        /// <summary>
        /// Specify the path of the link to the entry
        /// </summary>
        public string EntryPath { get; set; }

        /// <summary>
        /// Specify Culture;
        /// If not specified, defaults to current Culture;
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// The site's host url
        /// </summary>
        public string Host { get; set; }


        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = ViewContext.GetUrlHelper();

            if (urlHelper.IsLocalUrl(EntryPath))
            {
                if (Culture.IsNullOrEmpty())
                {
                    Culture = ViewContext.HttpContext.GetRouteValue(CultureRouteSegmentConstraint.RouteSegmentName)?.ToString();
                }

                EntryPath = ("~/" + Culture).EnsureEndsWith('/') + EntryPath.RemovePreFix("~").RemovePreFix("/");

                if (!Host.IsNullOrEmpty())
                {
                    EntryPath = Host + urlHelper.Content(EntryPath);
                }
            }

            output.Attributes.SetAttribute("href", urlHelper.Content(EntryPath));
        }
    }
}
