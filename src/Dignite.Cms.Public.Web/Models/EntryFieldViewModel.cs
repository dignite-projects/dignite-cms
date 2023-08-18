using Dignite.Abp.DynamicForms;

namespace Dignite.Cms.Public.Web.Models
{
    public class EntryFieldViewModel
    {
        public EntryFieldViewModel(ICustomizeFieldInfo field, IHasCustomFields entry)
        {
            Field = field;
            Entry = entry;
        }

        public ICustomizeFieldInfo Field { get; set; }

        public IHasCustomFields Entry { get; set; }
    }
}
