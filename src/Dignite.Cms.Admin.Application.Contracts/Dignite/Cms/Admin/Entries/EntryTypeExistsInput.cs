using System;
using System.ComponentModel.DataAnnotations;

namespace Dignite.Cms.Admin.Entries
{
    public class EntryTypeExistsInput
    {
        public EntryTypeExistsInput()
        {
        }

        public EntryTypeExistsInput(string culture, Guid sectionId, Guid entryTypeId)
        {
            Culture = culture;
            SectionId = sectionId;
            EntryTypeId = entryTypeId;
        }

        [Required]
        public string Culture { get; set; }

        [Required]
        public Guid SectionId { get; set; }

        [Required]
        public Guid EntryTypeId { get; set; }
    }
}
