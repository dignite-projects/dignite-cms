using JetBrains.Annotations;
using System;
using Volo.Abp;

namespace Dignite.Cms.Sections
{
    [Serializable]
    public class RouteNoSlugParameterException : BusinessException
    {
        public RouteNoSlugParameterException([NotNull] SectionType type,[NotNull]string route)
        {
            Code = CmsErrorCodes.Sections.RouteNoSlugParameter;
            WithData(nameof(Section.Type), type);
            WithData(nameof(Section.EntryPage.Route), route);
        }
    }
}
