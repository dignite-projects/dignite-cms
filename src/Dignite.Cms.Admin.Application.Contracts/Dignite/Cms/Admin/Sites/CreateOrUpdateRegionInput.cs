using Dignite.Cms.Sites;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Dignite.Cms.Admin.Sites
{
    [Serializable]
    public class CreateOrUpdateRegionInput
    {
        public CreateOrUpdateRegionInput()
        {
        }

        public CreateOrUpdateRegionInput(bool isDefault, string region)
        {
            IsDefault = isDefault;
            Region = region;
        }

        [Required]
        public bool IsDefault { get; set; }

        [Required]
        [DynamicMaxLength(typeof(SiteConsts), nameof(SiteConsts.MaxRegionLength))]
        public string Region { get; set; }
    }
}
