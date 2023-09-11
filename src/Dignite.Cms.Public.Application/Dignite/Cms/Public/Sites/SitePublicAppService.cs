﻿using Dignite.Cms.Public.Sites;
using Dignite.Cms.Sites;
using System;
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

        public async Task<SiteDto> FindByHostUrlAsync(string host)
        {
            var result = await _siteRepository.FindByHostAsync(host.RemovePostFix("/"));
            return ObjectMapper.Map<Site, SiteDto>(result);
        }
    }
}