using Dignite.Cms.Public.Web.Routing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using System;

namespace Dignite.Cms.Public.Web.TagHelpers
{
    /// <summary>
    /// Link to entry
    /// </summary>
    [HtmlTargetElement("a", Attributes = "[entry-url]")]
    public class LinkToEntryByUrlTagHelper : TagHelper
    {
        /// <summary>
        /// Specify the url of the link to the entry
        /// </summary>
        public string EntryUrl { get; set; }

        /// <summary>
        /// Specify region;
        /// If not specified, defaults to current region;
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// The site's host url
        /// </summary>
        public string HostUrl { get; set; }


        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = ViewContext.GetUrlHelper();

            if (urlHelper.IsLocalUrl(EntryUrl))
            {
                if (Region.IsNullOrEmpty())
                {
                    Region = ViewContext.HttpContext.GetRouteValue(CultureRouteSegmentConstraint.RouteSegmentName)?.ToString();
                }

                EntryUrl = ("~/" + Region).EnsureEndsWith('/') + EntryUrl.RemovePreFix("~").RemovePreFix("/");

                if (!HostUrl.IsNullOrEmpty())
                {
                    EntryUrl = HostUrl + urlHelper.Content(EntryUrl);
                }
            }

            output.Attributes.SetAttribute("href", urlHelper.Content(EntryUrl));
        }
    }
}
