using System;
using System.ComponentModel.DataAnnotations;

namespace Dignite.Cms.Admin.Entries
{
    public class MoveEntryInput
    {
        [Required]
        public Guid TargetId { get; set; }

        [Required]
        public MoveEntryPosition Position { get; set; }
    }
}
