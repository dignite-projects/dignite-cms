using Dignite.Cms.Sections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Dignite.Cms.Admin.Sections
{
    public class EntryFieldTabInput
    {
        public EntryFieldTabInput()
        {
            Fields = new List<EntryFieldInput>();
        }

        [Required]
        [DynamicMaxLength(typeof(EntryTypeConsts), nameof(EntryTypeConsts.MaxDisplayNameLength))]
        public string Name { get; set; }

        [Required]
        public IList<EntryFieldInput> Fields { get; set; }
    }
}
