using Dignite.Abp.DynamicForms;
using System;
using Volo.Abp.Application.Dtos;

namespace Dignite.Cms.Fields
{
    public class FieldDto: EntityDto<Guid>
    {

        /// <summary>
        /// Field Unique Name
        /// </summary>
        public virtual string Name { get; protected set; }


        /// <summary>
        /// 
        /// </summary>
        public virtual string DisplayName { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Description { get; protected set; }

        /// <summary>
        /// Field <see cref="IFormControl.Name"/>
        /// </summary>
        public virtual string FormControlName { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual FormConfigurationDictionary FormConfiguration { get; set; }
        
    }
}
