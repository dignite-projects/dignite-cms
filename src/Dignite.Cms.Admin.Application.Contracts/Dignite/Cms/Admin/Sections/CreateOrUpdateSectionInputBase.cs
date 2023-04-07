using Dignite.Cms.Sections;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Dignite.Cms.Admin.Sections
{
    public abstract class CreateOrUpdateSectionInputBase: ExtensibleObject
    {
        protected CreateOrUpdateSectionInputBase()
        {
            Type = SectionType.Single;
            EntryPage = new EntryPageInput();
            IsActive = true;
        }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public virtual SectionType Type { get; set; }

        /// <summary>
        /// Display Name of this section.
        /// </summary>
        [Required]
        [DynamicMaxLength(typeof(SectionConsts), nameof(SectionConsts.MaxDisplayNameLength))]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Name of this section.
        /// Section Unique Name.
        /// </summary>
        [Required]
        [DynamicMaxLength(typeof(SectionConsts), nameof(SectionConsts.MaxNameLength))]
        [RegularExpression(SectionConsts.NameRegularExpression)]
        public virtual string Name { get; set; }

        /// <summary>
        /// The default section in the site
        /// </summary>
        public virtual bool IsDefault { get; set; }

        /// <summary>
        /// Is this section active
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Entry Page of this section
        /// </summary>
        public virtual EntryPageInput EntryPage { get; set; }
    }
}
