using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;

namespace Dignite.Cms.Public.Domains
{
    [RemoteService(Name = CmsPublicRemoteServiceConsts.RemoteServiceName)]
    [Area(CmsPublicRemoteServiceConsts.ModuleName)]
    [Route("api/cms-public/domains")]
    public class DomainPublicController : CmsPublicController, IDomainPublicAppService
    {
        private readonly IDomainPublicAppService _domainPublicAppService;

        public DomainPublicController(IDomainPublicAppService domainPublicAppService)
        {
            _domainPublicAppService = domainPublicAppService;
        }

        [HttpGet]
        [Route("find-by-name")]
        public async Task<DomainDto> FindByNameAsync(string domainName)
        {
            return await _domainPublicAppService.FindByNameAsync(domainName);
        }
    }
}
