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
    /// Add language to href by specifying <see cref="CmsEntryUrlWithSiteTagHelper.CmsSite"/> and <see cref="CmsEntryUrlWithSiteTagHelper.CmsLanguage"/> 
    /// </summary>
    [HtmlTargetElement("a",Attributes ="cms-site")]
    public class CmsEntryUrlWithSiteTagHelper : TagHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public SiteDto CmsSite { get; set; }

        /// <summary>
        /// Specify the language of the link,
        /// otherwise the site default language is used.
        /// </summary>
        public string CmsLanguage { get; set; }


        [ViewContext, HtmlAttributeNotBound]
         public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var hostAddress = ViewContext.HttpContext.Request.Scheme + "://" + ViewContext.HttpContext.Request.Host;
            var href = output.Attributes.SingleOrDefault(a => a.Name == "href")?.Value.ToString();

            //
            if (CmsLanguage.IsNullOrWhiteSpace())
            {
                CmsLanguage = CultureInfo.CurrentCulture.Name;
            }

            //
            var defaultLanguage = CmsSite.Languages.First(l => l.IsDefault);
            if (!defaultLanguage.Language.Equals(CmsLanguage, StringComparison.OrdinalIgnoreCase))
            {
                href = CmsLanguage.EnsureEndsWith('/') + href.RemovePreFix("/");
            }

            //
            if (!CmsSite.Host.Equals(hostAddress, StringComparison.OrdinalIgnoreCase))
            {
                href = CmsSite.Host.EnsureEndsWith('/') + href.RemovePreFix("/");
            }

            //
            output.Attributes.SetAttribute("href", href);
        }
    }
}
