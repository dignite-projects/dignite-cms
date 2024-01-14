using System;
using System.Collections.Generic;
using System.Linq;
using Dignite.Abp.DynamicForms.Switch;
using Dignite.Cms.Entries;
using JetBrains.Annotations;

namespace Dignite.Abp.Data;
public class SwitchCustomFieldQuerying : CustomFieldQueryingBase<SwitchFormControl>
{
    public SwitchCustomFieldQuerying() : base()
    { }


    public override IEnumerable<Entry> Query([NotNull] IEnumerable<Entry> source, [NotNull] QueryingByCustomField customField)
    {
        var value = bool.Parse(customField.Value);
        return source.Where(e => e.ExtraProperties.ContainsKey(customField.Name)
            && Convert.ToBoolean(e.ExtraProperties[customField.Name]) == value
        );
    }
}
