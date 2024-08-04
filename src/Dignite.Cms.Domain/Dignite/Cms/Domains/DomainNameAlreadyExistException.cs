using JetBrains.Annotations;
using System;
using Volo.Abp;

namespace Dignite.Cms.Domains
{
    [Serializable]
    public class DomainNameAlreadyExistException : BusinessException
    {
        public DomainNameAlreadyExistException([NotNull] string name)
        {
            Code = CmsErrorCodes.Domains.NameAlreadyExist;
            WithData(nameof(Domain.DomainName), name);
        }
    }
}
