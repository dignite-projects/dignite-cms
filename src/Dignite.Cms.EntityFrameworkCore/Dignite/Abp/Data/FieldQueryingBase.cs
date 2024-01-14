using System;
using System.Collections.Generic;
using Dignite.Abp.DynamicForms;
using Dignite.Cms.Entries;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;

namespace Dignite.Abp.Data;
public abstract class FieldQueryingBase<TForm> : IFieldQuerying, ITransientDependency
    where TForm : IFormControl
{

    protected FieldQueryingBase()
    {
        FormType = typeof(TForm);
    }

    public Type FormType { get; private set; }


    public abstract IEnumerable<Entry> Query([NotNull] IEnumerable<Entry> source, [NotNull] QueryingByFieldParameter parameter);
}
