using Volo.Abp.Domain.Entities;

namespace Dignite.Cms.Admin.Sections
{
    public class UpdateEntryTypeInput : CreateOrUpdateEntryTypeInputBase, IHasConcurrencyStamp
    {
        public string ConcurrencyStamp { get; set; }
    }
}
