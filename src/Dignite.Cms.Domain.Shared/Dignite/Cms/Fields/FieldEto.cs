using System;
using Dignite.Abp.DynamicForms;
using Volo.Abp.EventBus;

namespace Dignite.Cms.Fields;

[EventName("Dignite.Cms.Fields.FieldEto")]
public class FieldEto
{
    public Guid Id { get; set; }

    public Guid? TenantId { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual Guid? GroupId { get; set; }

    /// <summary>
    /// Field Unique Name
    /// </summary>
    public virtual string Name { get; set; }


    /// <summary>
    /// 
    /// </summary>
    public virtual string DisplayName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual string Description { get; set; }

    /// <summary>
    /// Field <see cref="IFormControl.Name"/>
    /// </summary>
    public virtual string FormControlName { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual FormConfigurationDictionary FormConfiguration { get; set; }
}