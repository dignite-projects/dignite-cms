

using System;
using System.ComponentModel.DataAnnotations;

namespace Dignite.Cms.Admin.Sections
{
    public class CreateSectionInput : CreateOrUpdateSectionInputBase
    {
        public CreateSectionInput():base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid SiteId { get; set; }
    }
}
