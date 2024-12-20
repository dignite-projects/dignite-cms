﻿using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dignite.Cms.Public.Sections
{
    public interface ISectionPublicAppService : IApplicationService
    {
        Task<SectionDto> FindByNameAsync(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="route">
        /// The entry route does not contain culture.
        /// </param>
        /// <returns></returns>
        Task<SectionDto> FindByRouteAsync(string route);

        Task<ListResultDto<SectionDto>> GetListAsync(GetSectionsInput input);

        Task<SectionDto> GetAsync(Guid id);

        Task<SectionDto> GetDefaultAsync();
    }
}
