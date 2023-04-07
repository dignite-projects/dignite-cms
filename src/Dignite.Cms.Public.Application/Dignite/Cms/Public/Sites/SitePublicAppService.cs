using Dignite.Cms.Public.Sites;
using Dignite.Cms.Sites;
using System.Linq;
using System.Threading.Tasks;

namespace Dignite.Cms.Public.Pages
{
    public class SitePublicAppService : CmsPublicAppService, ISitePublicAppService
    {
        private readonly ISiteRepository _siteRepository;

        public SitePublicAppService(ISiteRepository siteRepository)
        {
            _siteRepository = siteRepository;
        }


        public async Task<SiteDto> FindByNameAsync(string name)
        { 
            var result = await _siteRepository.FindByNameAsync(name);
            return ObjectMapper.Map<Site, SiteDto>(result);
        }

        public async Task<SiteDto> GetDefaultAsync()
        {
            var result = await _siteRepository.GetListAsync(null,true);
            var site = result.OrderByDescending(e => e.IsDefault).FirstOrDefault();
            return ObjectMapper.Map<Site, SiteDto>(site);
        }
    }
}