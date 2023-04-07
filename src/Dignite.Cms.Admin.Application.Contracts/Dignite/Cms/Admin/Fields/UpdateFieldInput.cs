using Volo.Abp.Domain.Entities;

namespace Dignite.Cms.Admin.Fields
{
    public class UpdateFieldInput: CreateOrUpdateFieldInputBase, IHasConcurrencyStamp
    {
        public UpdateFieldInput():base()
        {
        }

        public string ConcurrencyStamp { get; set; }
    }
}
