using System;
using System.Collections.Generic;
using Dignite.Abp.DynamicForms;
using Dignite.Cms.Entries;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;

namespace Dignite.Abp.Data;
public abstract class CustomFieldQueryingBase<TFormControl> : ICustomFieldQuerying, ITransientDependency
    where TFormControl : IFormControl
{

    protected CustomFieldQueryingBase()
    {
        FormControlType = typeof(TFormControl);
    }

    public Type FormControlType { get; private set; }


    public abstract IEnumerable<Entry> Query([NotNull] IEnumerable<Entry> source, [NotNull] QueryingByCustomField customField);
}
