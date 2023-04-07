using Dignite.Abp.DynamicForms;
using Dignite.Abp.DynamicForms.Textbox;
using Dignite.Cms.Fields;
using JetBrains.Annotations;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Dignite.Cms.Admin.Fields
{
    public abstract class CreateOrUpdateFieldInputBase:ExtensibleObject, ICustomizeFieldInfo
    {
        protected CreateOrUpdateFieldInputBase() : base(false)
        {
            FormName = TextboxForm.TextboxFormName;
            FormConfiguration=new FormConfigurationDictionary();
        }

        public  Guid? GroupId { get; set; }

        /// <summary>
        /// Display name of this field.
        /// </summary>
        [Required]
        [DynamicMaxLength(typeof(CustomizeFieldInfoConsts), nameof(CustomizeFieldInfoConsts.MaxDisplayNameLength))]
        public string DisplayName { get; set; }

        /// <summary>
        /// Unique Name
        /// </summary>
        [Required]
        [DynamicMaxLength(typeof(CustomizeFieldInfoConsts), nameof(CustomizeFieldInfoConsts.MaxNameLength))]
        [RegularExpression(CustomizeFieldInfoConsts.NameRegularExpression)]
        public string Name { get; set; }

        /// <summary>
        /// Default value of the field.
        /// </summary>
        [CanBeNull]
        public string DefaultValue { get; set; }

        [Required]
        [DynamicMaxLength(typeof(CustomizeFieldInfoConsts), nameof(CustomizeFieldInfoConsts.MaxFormNameLength))]
        public string FormName { get; set; }

        [Required]
        public virtual FormConfigurationDictionary FormConfiguration { get; set; }
    }
}
