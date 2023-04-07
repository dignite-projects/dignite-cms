

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
        public virtual Guid SiteId { get; set; }
    }
}
