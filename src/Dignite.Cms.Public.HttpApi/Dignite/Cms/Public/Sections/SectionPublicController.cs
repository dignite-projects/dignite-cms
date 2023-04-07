using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Xml.Linq;
using Volo.Abp;

namespace Dignite.Cms.Public.Sections
{
    [RemoteService(Name = CmsPublicRemoteServiceConsts.RemoteServiceName)]
    [Area(CmsPublicRemoteServiceConsts.ModuleName)]
    [Route("api/cms-public/sections")]
    public class SectionPublicController : CmsPublicController, ISectionPublicAppService
    {
        private readonly ISectionPublicAppService _sectionAppService;

        public SectionPublicController(ISectionPublicAppService sectionAppService)
        {
            _sectionAppService = sectionAppService;
        }

        [HttpGet]
        [Route("find-by-name")]
        public async Task<SectionDto> FindByNameAsync(Guid siteId, string name)
        {
            return await _sectionAppService.FindByNameAsync(siteId, name);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="url">
        /// 
        /// </param>
        /// <returns></returns>
        [HttpGet]
        [Route("find-by-url")]
        public async Task<SectionDto> FindByUrlAsync(Guid siteId, string url)
        {
            return await _sectionAppService.FindByUrlAsync(siteId, url);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<SectionDto> GetAsync(Guid sectionId)
        {
            return await _sectionAppService.GetAsync(sectionId);
        }

        [HttpGet]
        [Route("get-default")]
        public async Task<SectionDto> GetDefaultAsync(Guid siteId)
        {
            return await _sectionAppService.GetDefaultAsync(siteId);
        }
    }
}
