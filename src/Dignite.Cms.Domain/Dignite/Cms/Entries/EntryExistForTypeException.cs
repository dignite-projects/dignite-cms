using Dignite.Cms.Sections;
using JetBrains.Annotations;
using System;
using Volo.Abp;

namespace Dignite.Cms.Entries
{
    [Serializable]
    public class EntryExistForTypeException : BusinessException
    {
        public EntryExistForTypeException([NotNull] string culture, [NotNull] string entryTypeName)
        {
            Code = CmsErrorCodes.Entries.SlugAlreadyExist;
            WithData(nameof(Entry.Culture), culture);
            WithData(nameof(EntryType.Name), entryTypeName);
        }
    }
}
