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
        /// Host of this site.
        /// The host of the site must be a domain name
        /// </summary>
        [Required]
        [DynamicMaxLength(typeof(SiteConsts), nameof(SiteConsts.MaxHostLength))]
        public virtual string Host { get;  set; }


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
                new[] { nameof(Host) });
            }

            if (Languages.Count(l => l.IsDefault) != 1)
            {
                yield return new ValidationResult(
                "The site's language list is missing a unique default language!",
                new[] { nameof(Languages) });
            }

            base.Validate(validationContext);
        }

        protected bool IsHostURL()
        {
            Host = Host.RemovePostFix("/");
            Host = Host.RemovePostFix("\\");
            // 匹配可包含端口号的 IP 地址的主机 URL 的正则表达式
            string hostURLPattern = @"^(http|https)://((\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})|([a-zA-Z0-9\-]{1,63}(\.[a-zA-Z0-9\-]{1,63})*))(:\d+)?$";

            return Regex.IsMatch(Host, hostURLPattern);
        }
    }
}
