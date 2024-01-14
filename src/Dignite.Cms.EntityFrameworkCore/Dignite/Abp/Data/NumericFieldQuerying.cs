using System;
using System.Collections.Generic;
using System.Linq;
using Dignite.Abp.DynamicForms.NumericEdit;
using Dignite.Cms.Entries;
using JetBrains.Annotations;

namespace Dignite.Abp.Data;
public class NumericFieldQuerying : FieldQueryingBase<NumericEditFormControl>
{
    public NumericFieldQuerying() : base()
    { }


    public override IEnumerable<Entry> Query([NotNull] IEnumerable<Entry> source, [NotNull] QueryingByFieldParameter parameter)
    {
        var min = double.Parse(parameter.Value.Split('-')[0]);
        var max = double.Parse(parameter.Value.Split('-')[1]);
        return source.Where(e => e.ExtraProperties.ContainsKey(parameter.FieldName)
            && Convert.ToDouble(e.ExtraProperties[parameter.FieldName]) >= min
            && Convert.ToDouble(e.ExtraProperties[parameter.FieldName]) < max
        );
    }
}
