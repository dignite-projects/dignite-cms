using Dignite.Cms.Sites;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Dignite.Cms.Admin.Sites
{
    [Serializable]
    public class SiteLanguageInput
    {
        public SiteLanguageInput()
        {
        }

        public SiteLanguageInput(bool isDefault, string cultureName)
        {
            IsDefault = isDefault;
            CultureName = cultureName;
        }

        [Required]
        public bool IsDefault { get; set; }

        [Required]
        [DynamicMaxLength(typeof(SiteConsts), nameof(SiteConsts.MaxLanguageCultureNameLength))]
        public string CultureName { get; set; }
    }
}
