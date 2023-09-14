using Dignite.Cms.Public.Web.Routing;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using System;
using System.Linq;

namespace Dignite.Cms.Public.Web.TagHelpers
{
    [HtmlTargetElement("a", Attributes = "[href^='~/']")]
    [HtmlTargetElement("a", Attributes = "[href^='/']")]
    [HtmlTargetElement("a", Attributes = "[href^='~/'],[region]")]
    [HtmlTargetElement("a", Attributes = "[href^='/'],[region]")]
    public class RegionUrlResolutionTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public RegionUrlResolutionTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        [HtmlAttributeName("region")]
        public string Region { get; set; }= null;


        [ViewContext, HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            var href = context.AllAttributes.SingleOrDefault(a => a.Name == "href")?.Value.ToString();

            if (Region.IsNullOrEmpty())
            {
                Region = ViewContext.HttpContext.GetRouteValue(CultureRouteSegmentConstraint.RouteSegmentName)?.ToString();
            }

            if (Region == null)
            {
                return;
            }

            href = "~/"+Region + href.RemovePreFix("~").EnsureStartsWith('/');

            //
            output.Attributes.SetAttribute("href", urlHelper.Content(href));
        }
    }
}
