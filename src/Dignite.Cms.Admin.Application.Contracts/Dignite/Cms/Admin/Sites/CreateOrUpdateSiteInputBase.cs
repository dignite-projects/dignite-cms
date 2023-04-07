using Dignite.Cms.Sites;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Dignite.Cms.Admin.Sites
{
    public abstract class CreateOrUpdateSiteInputBase: ExtensibleObject
    {
        protected CreateOrUpdateSiteInputBase() : base(false)
        {
            this.Languages = new List<CreateOrUpdateLanguageInput>();
        }

        /// <summary>
        /// Display Name of this site.
        /// </summary>
        [Required]
        [DynamicMaxLength(typeof(SiteConsts), nameof(SiteConsts.MaxDisplayNameLength))]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Name of this site.
        /// Site Unique Name.
        /// </summary>
        [Required]
        [DynamicMaxLength(typeof(SiteConsts), nameof(SiteConsts.MaxNameLength))]
        [RegularExpression(SiteConsts.NameRegularExpression)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Languages supported on this site
        /// </summary>
        [Required]
        public ICollection<CreateOrUpdateLanguageInput> Languages { get; set; }

        /// <summary>
        /// Base Url of this site
        /// </summary>
        [Required]
        [DynamicMaxLength(typeof(SiteConsts), nameof(SiteConsts.MaxBaseUrlLength))]
        public virtual string BaseUrl { get;  set; }

        /// <summary>
        /// Is this site a default
        /// </summary>
        public virtual bool IsDefault { get; set; }


        /// <summary>
        /// Is this site active
        /// </summary>
        public virtual bool IsActive { get; set; }
    }
}
