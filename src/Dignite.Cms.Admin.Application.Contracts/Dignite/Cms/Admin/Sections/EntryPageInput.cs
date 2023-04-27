using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Dignite.Cms.Sections
{
    /// <summary>
    /// 
    /// </summary>
    public class EntryPageInput
    {
        /// <summary>
        /// Routing format for entry page;
        /// </summary>
        /// <example>
        /// Route with formatting parameters:
        /// 1./news
        /// 2./news/{publishTime:yyyy-M}/{slug}
        /// </example>
        /// <remarks>
        /// 1.If the section is not a single type, {slug} must be included in the route;
        /// 2.Route parameters must be public properties in the entry;
        /// 3.Routing parameters support formatting
        /// </remarks>
        [DynamicMaxLength(typeof(SectionConsts), nameof(SectionConsts.MaxPageRouteLength))]
        [RegularExpression(SectionConsts.PageRouteRegularExpression)]
        public string Route { get; set; }

        /// <summary>
        /// asp.net core mvc Razor Page
        /// </summary>
        [DynamicMaxLength(typeof(SectionConsts), nameof(SectionConsts.MaxPagetemplateLength))]
        [RegularExpression(SectionConsts.PageTemplateRegularExpression)]
        public string Template { get; set; }
    }
}
