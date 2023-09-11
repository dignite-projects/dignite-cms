using Dignite.Cms.Sites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Dignite.Cms.Admin.Sites
{
    public abstract class CreateOrUpdateSiteInputBase: ExtensibleObject
    {
        protected CreateOrUpdateSiteInputBase() : base(false)
        {
            this.Regions = new List<CreateOrUpdateRegionInput>();
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
        /// Regions supported on this site
        /// </summary>
        [Required]
        public ICollection<CreateOrUpdateRegionInput> Regions { get; set; }

        /// <summary>
        /// Host of this site.
        /// The host of the site must be a domain name
        /// </summary>
        [Required]
        [DynamicMaxLength(typeof(SiteConsts), nameof(SiteConsts.MaxHostUrlLength))]
        public virtual string HostUrl { get;  set; }


        /// <summary>
        /// Is this site active
        /// </summary>
        public virtual bool IsActive { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsHostURL())
            {
                yield return new ValidationResult(
                "The host of the site must be a domain name or IP",
                new[] { nameof(HostUrl) });
            }

            if (Regions.Count(l => l.IsDefault) != 1)
            {
                yield return new ValidationResult(
                "The site's region list is missing a unique default region!",
                new[] { nameof(Regions) });
            }

            base.Validate(validationContext);
        }

        protected bool IsHostURL()
        {
            HostUrl = HostUrl.RemovePostFix("/");
            HostUrl = HostUrl.RemovePostFix("\\");

            // Regular expression that matches a host URL with an IP address containing a port number
            string hostURLPattern = @"^(http|https)://((\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})|([a-zA-Z0-9\-]{1,63}(\.[a-zA-Z0-9\-]{1,63})*))(:\d+)?$";

            return Regex.IsMatch(HostUrl, hostURLPattern);
        }
    }
}
