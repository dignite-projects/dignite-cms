using Dignite.Cms.Public.Sites;
using Dignite.Cms.Sites;
using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.MultiTenancy;

namespace Dignite.Cms.Public.Pages
{
    public class SitePublicAppService : CmsPublicAppService, ISitePublicAppService
    {
        private readonly ISiteRepository _siteRepository;
        private readonly IDataFilter _dataFilter;

        public SitePublicAppService(ISiteRepository siteRepository, IDataFilter dataFilter)
        {
            _siteRepository = siteRepository;
            _dataFilter = dataFilter;
        }


        public async Task<SiteDto> FindByNameAsync(string name)
        { 
            var result = await _siteRepository.FindByNameAsync(name);
            return ObjectMapper.Map<Site, SiteDto>(result);
        }

        public async Task<SiteDto> FindByHostAsync(string host)
        {
            using (_dataFilter.Disable<IMultiTenant>())
            {
                var result = await _siteRepository.FindByHostAsync(host.RemovePostFix("/"));
                return ObjectMapper.Map<Site, SiteDto>(result);
            }
        }
    }
}