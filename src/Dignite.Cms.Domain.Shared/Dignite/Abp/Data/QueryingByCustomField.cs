namespace Dignite.Abp.Data;
public class QueryingByCustomField
{
    public QueryingByCustomField()
    {
    }

    public QueryingByCustomField(string name, string value)
    {
        Name = name;
        Value = value;
    }

    /// <summary>
    /// <see cref="DynamicForms.FormField.Name"/>
    /// </summary>
    public string Name { get; set; }

    public virtual string Value { get; set; }
}
