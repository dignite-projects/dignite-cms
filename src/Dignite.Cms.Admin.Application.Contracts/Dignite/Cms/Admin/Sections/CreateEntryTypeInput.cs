

using System;
using System.ComponentModel.DataAnnotations;

namespace Dignite.Cms.Admin.Sections
{
    public class CreateEntryTypeInput : CreateOrUpdateEntryTypeInputBase
    {
        public CreateEntryTypeInput(Guid sectionId):base()
        {
            SectionId = sectionId;
        }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public virtual Guid SectionId { get; set; }
    }
}
