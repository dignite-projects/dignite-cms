using Dignite.Cms.Domains;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Dignite.Cms.Admin.Domains
{
    public class UpdateDomainInput
    {
        [Required]
        [DynamicMaxLength(typeof(DomainConsts), nameof(DomainConsts.MaxDomainNameLength))]
        [RegularExpression(DomainConsts.NameRegularExpression)]
        public string DomainName { get; set; }
    }
}
