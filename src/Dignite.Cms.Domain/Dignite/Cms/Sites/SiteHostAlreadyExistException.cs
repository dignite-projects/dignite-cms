using JetBrains.Annotations;
using System;
using Volo.Abp;

namespace Dignite.Cms.Sites
{
    [Serializable]
    public class SiteHostAlreadyExistException : BusinessException
    {
        public SiteHostAlreadyExistException([NotNull]string host)
        {
            Code = CmsErrorCodes.Sites.HostAlreadyExist;
            WithData(nameof(Site.Host), host);
        }
    }
}
