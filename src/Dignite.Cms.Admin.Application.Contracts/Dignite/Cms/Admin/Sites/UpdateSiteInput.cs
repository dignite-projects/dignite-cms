using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Dignite.Cms.Admin.Sites
{
    public class UpdateSiteInput
    {
        [Required]
        [MaxLength(32)]
        public string DefaultLanguage { get; set; }

        [Required]
        public IEnumerable<string> AllLanguages { get; set; }
    }
}
