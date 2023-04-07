using JetBrains.Annotations;
using System;
using Volo.Abp;

namespace Dignite.Cms.Entries
{
    [Serializable]
    public class EntrySlugAlreadyExistException : BusinessException
    {
        public EntrySlugAlreadyExistException([NotNull]string language, [NotNull] string slug)
        {
            Code = CmsErrorCodes.Entries.SlugAlreadyExist;
            WithData(nameof(Entry.Language), language);
            WithData(nameof(Entry.Slug), slug);
        }
    }
}
