using System.Collections.Generic;
using System.Linq;
using Dignite.Abp.DynamicForms.Textbox;
using Dignite.Cms.Entries;
using JetBrains.Annotations;

namespace Dignite.Abp.Data;
public class TextCustomFieldQuerying : CustomFieldQueryingBase<TextEditFormControl>
{
    public TextCustomFieldQuerying() : base()
    {
    }
    public override IEnumerable<Entry> Query([NotNull] IEnumerable<Entry> source, [NotNull] QueryingByCustomField customField)
    {
        return source.Where(e => e.ExtraProperties.ContainsKey(customField.Value) &&
            e.ExtraProperties[customField.Name].ToString().Contains(customField.Value)
        );
    }
}
