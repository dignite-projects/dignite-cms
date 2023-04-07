using JetBrains.Annotations;
using System;
using Volo.Abp;

namespace Dignite.Cms.Sites
{
    [Serializable]
    public class DefaultSiteCannotSetNotActiveException : BusinessException
    {
        public DefaultSiteCannotSetNotActiveException([NotNull]string displayName)
        {
            Code = CmsErrorCodes.Sites.DefaultSiteCannotSetNotActive;
            WithData(nameof(Site.DisplayName), displayName);
        }
    }
}
