
using Dignite.Abp.DynamicForms;

namespace Dignite.Cms.Admin.Fields
{
    /// <summary>
    /// 
    /// </summary>
    public class FormDto
    {
        public FormDto(string name, string displayName, FormType formType)
        {
            Name = name;
            DisplayName = displayName;
            FormType = formType;
        }

        protected FormDto()
        {
        }


        public string Name { get; protected set; }

        public string DisplayName { get; protected set; }

        public FormType FormType { get; protected set; }
    }
}
