using JetBrains.Annotations;
using System;
using Volo.Abp;

namespace Dignite.Cms.Sites
{
    [Serializable]
    public class SiteNameAlreadyExistException : BusinessException
    {
        public SiteNameAlreadyExistException([NotNull]string name)
        {
            Code = CmsErrorCodes.Sites.NameAlreadyExist;
            WithData(nameof(Site.Name), name);
        }
    }
}
