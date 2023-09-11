using Dignite.Cms.Public.Sites;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Globalization;
using System.Linq;

namespace Dignite.Cms.Public.Web.TagHelpers
{
    /// <summary>
    /// Add region to href by specifying <see cref="CmsEntryUrlWithSiteTagHelper.CmsSite"/> and <see cref="CmsEntryUrlWithSiteTagHelper.CmsRegion"/> 
    /// </summary>
    [HtmlTargetElement("a",Attributes ="cms-site")]
    public class CmsEntryUrlWithSiteTagHelper : TagHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public SiteDto CmsSite { get; set; }

        /// <summary>
        /// Specify the region of the link,
        /// otherwise the site default region is used.
        /// </summary>
        public string CmsRegion { get; set; }


        [ViewContext, HtmlAttributeNotBound]
         public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var hostAddress = ViewContext.HttpContext.Request.Scheme + "://" + ViewContext.HttpContext.Request.Host;
            var href = output.Attributes.SingleOrDefault(a => a.Name == "href")?.Value.ToString();

            //
            if (CmsRegion.IsNullOrWhiteSpace())
            {
                CmsRegion = CultureInfo.CurrentCulture.Name;
            }

            //
            var defaultRegion = CmsSite.Regions.First(l => l.IsDefault);
            if (!defaultRegion.Region.Equals(CmsRegion, StringComparison.OrdinalIgnoreCase))
            {
                href = CmsRegion.EnsureEndsWith('/') + href.RemovePreFix("/");
            }

            //
            if (!CmsSite.HostUrl.Equals(hostAddress, StringComparison.OrdinalIgnoreCase))
            {
                href = CmsSite.HostUrl.EnsureEndsWith('/') + href.RemovePreFix("/");
            }

            //
            output.Attributes.SetAttribute("href", href);
        }
    }
}
