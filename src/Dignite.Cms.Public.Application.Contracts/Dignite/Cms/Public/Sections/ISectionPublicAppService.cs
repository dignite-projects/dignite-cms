using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Dignite.Cms.Public.Sections
{
    public interface ISectionPublicAppService : IApplicationService
    {
        Task<SectionDto> GetAsync(Guid sectionId);
        Task<SectionDto> FindByNameAsync(Guid siteId,string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="url">
        /// 
        /// </param>
        /// <returns></returns>
        Task<SectionDto> FindByUrlAsync(string url);

        Task<SectionDto> GetDefaultAsync(Guid siteId);
    }
}
