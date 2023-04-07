using Dignite.Cms.Sites;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Dignite.Cms.Admin.Sites
{
    [Serializable]
    public class CreateOrUpdateLanguageInput
    {
        public CreateOrUpdateLanguageInput()
        {
        }

        public CreateOrUpdateLanguageInput(bool isDefault, string language)
        {
            IsDefault = isDefault;
            Language = language;
        }

        [Required]
        public bool IsDefault { get; set; }

        [Required]
        [DynamicMaxLength(typeof(SiteConsts), nameof(SiteConsts.MaxLanguageLength))]
        public string Language { get; set; }
    }
}
