using System.Collections.Generic;
using System.Linq;
using Dignite.Abp.DynamicForms.Textbox;
using Dignite.Cms.Entries;
using JetBrains.Annotations;

namespace Dignite.Abp.Data;
public class TextFieldQuerying : FieldQueryingBase<TextEditFormControl>
{
    public TextFieldQuerying() : base()
    {
    }
    public override IEnumerable<Entry> Query([NotNull] IEnumerable<Entry> source, [NotNull] QueryingByFieldParameter parameter)
    {
        return source.Where(e => e.ExtraProperties.ContainsKey(parameter.Value) &&
            e.ExtraProperties[parameter.FieldName].ToString().Contains(parameter.Value)
        );
    }
}
