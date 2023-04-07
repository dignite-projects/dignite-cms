using Dignite.Cms.Fields;
using Dignite.Cms.Sections;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;

namespace Dignite.Cms.Admin.Sections
{
    public class EntryFieldInput
    {
        [Required]
        public Guid FieldId { get; set; }


        /// <summary>
        /// Text to override field definition
        /// </summary>
        [DynamicMaxLength(typeof(EntryTypeConsts), nameof(EntryTypeConsts.MaxDisplayNameLength))]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool Searchable { get; set; }
    }
}
