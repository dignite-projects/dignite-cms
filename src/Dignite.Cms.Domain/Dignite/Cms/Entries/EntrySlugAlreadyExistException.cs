using JetBrains.Annotations;
using System;
using Volo.Abp;

namespace Dignite.Cms.Entries
{
    [Serializable]
    public class EntrySlugAlreadyExistException : BusinessException
    {
        public EntrySlugAlreadyExistException([NotNull]string region, [NotNull] string slug)
        {
            Code = CmsErrorCodes.Entries.SlugAlreadyExist;
            WithData(nameof(Entry.Region), region);
            WithData(nameof(Entry.Slug), slug);
        }
    }
}
