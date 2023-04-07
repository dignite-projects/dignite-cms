using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Dignite.Cms.Sections
{
    /// <summary>
    /// 
    /// </summary>
    public class EntryPageInput
    {
        [DynamicMaxLength(typeof(SectionConsts), nameof(SectionConsts.MaxPageRouteLength))]
        [RegularExpression(SectionConsts.PageRouteRegularExpression)]
        public string Route { get; set; }


        [DynamicMaxLength(typeof(SectionConsts), nameof(SectionConsts.MaxPagetemplateLength))]
        [RegularExpression(SectionConsts.PageTemplateRegularExpression)]
        public string Template { get; set; }
    }
}
