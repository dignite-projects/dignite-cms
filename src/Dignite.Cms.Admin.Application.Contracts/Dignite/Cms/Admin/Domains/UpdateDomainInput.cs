using Dignite.Cms.Domains;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Dignite.Cms.Admin.Domains
{
    public class UpdateDomainInput
    {
        [Required]
        [DynamicMaxLength(typeof(DomainConsts), nameof(DomainConsts.MaxDomainNameLength))]
        [RegularExpression(@"^(?!-)[A-Za-z0-9-]{1,63}(?<!-)\.[A-Za-z]{2,6}$")]
        public string DomainName { get; set; }
    }
}
