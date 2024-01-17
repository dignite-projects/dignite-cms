using System;
using System.ComponentModel.DataAnnotations;

namespace Dignite.Cms.Admin.Entries
{
    public class CreateEntryInput : CreateOrUpdateEntryInputBase
    {
        public CreateEntryInput():base()
        {
        }

        public CreateEntryInput(Guid entryTypeId) : base() 
        {
            this.EntryTypeId = entryTypeId;
        }

        /// <summary>
        /// Parent entry id of the entry;
        /// When it is affiliated with <see cref="SectionDto.Type"/>=<see cref="Cms.Sections.SectionType.Structure"/>, this value is valid
        /// </summary>
        public Guid? ParentId { get; set; }

        public virtual Guid? InitialVersionId { get; set; }
    }
}
