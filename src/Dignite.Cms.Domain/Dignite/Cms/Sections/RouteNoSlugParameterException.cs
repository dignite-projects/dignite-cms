using JetBrains.Annotations;
using System;
using Volo.Abp;

namespace Dignite.Cms.Sections
{
    [Serializable]
    public class RouteNoSlugParameterException : BusinessException
    {
        public RouteNoSlugParameterException([NotNull] string name,[NotNull]string route)
        {
            Code = CmsErrorCodes.Sections.RouteNoSlugParameter;
            WithData(nameof(Section.Name), name);
            WithData(nameof(Section.EntryPage.Route), route);
        }
    }
}
