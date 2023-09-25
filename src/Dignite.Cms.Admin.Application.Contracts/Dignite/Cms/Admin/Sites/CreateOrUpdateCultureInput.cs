using Dignite.Cms.Sites;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Dignite.Cms.Admin.Sites
{
    [Serializable]
    public class CreateOrUpdateCultureInput
    {
        public CreateOrUpdateCultureInput()
        {
        }

        public CreateOrUpdateCultureInput(bool isDefault, string cultureName)
        {
            IsDefault = isDefault;
            CultureName = cultureName;
        }

        [Required]
        public bool IsDefault { get; set; }

        [Required]
        [DynamicMaxLength(typeof(SiteConsts), nameof(SiteConsts.MaxCultureLength))]
        public string CultureName { get; set; }
    }
}
