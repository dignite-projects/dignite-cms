using Dignite.Abp.DynamicForms;
using System;
using Volo.Abp.Application.Dtos;

namespace Dignite.Cms.Fields
{
    public class FieldDto: ExtensibleEntityDto<Guid>, ICustomizeFieldInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// Field Unique Name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string DefaultValue { get; set; }

        /// <summary>
        /// Field <see cref="IForm.Name"/>
        /// </summary>
        public virtual string FormName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual FormConfigurationDictionary FormConfiguration { get; set; }
    }
}
