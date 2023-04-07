using Dignite.Cms.Fields;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Dignite.Cms.Admin.Fields
{
    public class CreateOrUpdateFieldGroupInput: ExtensibleObject
    {
        [Required]
        [DynamicMaxLength(typeof(FieldGroupConsts), nameof(FieldGroupConsts.MaxNameLength))]
        public virtual string Name { get; set; }
    }
}
