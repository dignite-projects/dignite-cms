﻿using Dignite.Cms.Sections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Dignite.Cms.Admin.Sections
{
    public abstract class CreateOrUpdateEntryTypeInputBase: ExtensibleObject
    {
        protected CreateOrUpdateEntryTypeInputBase()
        {
            FieldTabs = new List<EntryFieldTabInput>();
        }

        /// <summary>
        /// Display Name of this entry type.
        /// </summary>
        [Required]
        [DynamicMaxLength(typeof(EntryTypeConsts), nameof(EntryTypeConsts.MaxDisplayNameLength))]
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Name of this entry type.
        /// Entry Type Unique Name.
        /// </summary>
        [Required]
        [DynamicMaxLength(typeof(EntryTypeConsts), nameof(EntryTypeConsts.MaxNameLength))]
        [RegularExpression(EntryTypeConsts.NameRegularExpression)]
        public virtual string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public IList<EntryFieldTabInput> FieldTabs { get; set; }
    }
}
