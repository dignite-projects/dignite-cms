using Dignite.Cms.Localization;
using Dignite.Cms.Sites;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Dignite.Cms.Admin.Sites
{
    public abstract class CreateOrUpdateSiteInputBase: ExtensibleObject
    {
        protected CreateOrUpdateSiteInputBase() : base(false)
        {
            this.Languages = new List<SiteLanguageInput>();
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
        public ICollection<SiteLanguageInput> Languages { get; set; }

        /// <summary>
        /// Host of this site.
        /// The host of the site must be a domain name
        /// </summary>
        [Required]
        [DynamicMaxLength(typeof(SiteConsts), nameof(SiteConsts.MaxHostLength))]
        [Url]
        public virtual string Host { get;  set; }


        /// <summary>
        /// Is this site active
        /// </summary>
        public virtual bool IsActive { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var stringLocalizerFactory = validationContext.GetRequiredService<IStringLocalizerFactory>();
            var L = stringLocalizerFactory.Create(typeof(CmsResource));

            if (Languages == null || !Languages.Any())
            {
                yield return new ValidationResult(
                    L["SelectSiteSupportLanguages"],
                    new[] { nameof(Languages) }
                    );
            }

            base.Validate(validationContext);
        }
    }
}
