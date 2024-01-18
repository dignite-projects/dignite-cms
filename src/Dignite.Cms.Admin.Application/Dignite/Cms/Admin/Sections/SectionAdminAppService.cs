﻿using Dignite.Cms.Fields;
using Dignite.Cms.Sections;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dignite.Cms.Admin.Sections
{
    public class SectionAdminAppService : CmsAdminAppServiceBase, ISectionAdminAppService
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IFieldRepository _fieldRepository;
        private readonly SectionManager _sectionManager;

        public SectionAdminAppService(ISectionRepository sectionRepository, IFieldRepository fieldRepository, SectionManager sectionManager)
        {
            _sectionRepository = sectionRepository;
            _fieldRepository = fieldRepository;
            _sectionManager = sectionManager;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [Authorize(Permissions.CmsAdminPermissions.Section.Create)]
        public async Task<SectionDto> CreateAsync(CreateSectionInput input)
        {
            var section = await _sectionManager.CreateAsync(input.SiteId, input.Type, input.DisplayName, input.Name, input.IsDefault, input.IsActive, input.Route, input.Template, CurrentTenant.Id);
            return ObjectMapper.Map<Section, SectionDto>(section);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(Permissions.CmsAdminPermissions.Section.Update)]
        public async Task<SectionDto> UpdateAsync(Guid id, UpdateSectionInput input)
        {
            var section = await _sectionManager.UpdateAsync(id, input.Type, input.DisplayName, input.Name, input.IsDefault, input.IsActive, input.Route, input.Template);
            return ObjectMapper.Map<Section, SectionDto>(section);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(Permissions.CmsAdminPermissions.Section.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _sectionRepository.DeleteAsync(id);
        }

        [Authorize(Permissions.CmsAdminPermissions.Section.Default)]
        public async Task<PagedResultDto<SectionDto>> GetListAsync(GetSectionsInput input)
        {
            if (input.SiteId == Guid.Empty)
                return new PagedResultDto<SectionDto>(0, new List<SectionDto>());

            var count = await _sectionRepository.GetCountAsync(input.SiteId, input.Filter, input.IsActive);
            var result = await _sectionRepository.GetListAsync(input.SiteId, input.Filter, input.IsActive,true, input.MaxResultCount, input.SkipCount, input.Sorting);

            var dto = ObjectMapper.Map<List<Section>, List<SectionDto>>(result);

            return new PagedResultDto<SectionDto>(count, dto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(Permissions.CmsAdminPermissions.Section.Default)]
        public async Task<SectionDto> GetAsync(Guid id)
        {
            var result = await _sectionRepository.GetAsync(id, true);
            var dto = ObjectMapper.Map<Section, SectionDto>(
                result
                );
            await FillSectionFields(dto);
            return dto;
        }



        protected async Task FillSectionFields(SectionDto dto)
        {
            var allFields = await _fieldRepository.GetListAsync(false);
            var fieldsDto = ObjectMapper.Map<List<Field>, List<FieldDto>>(allFields);
            foreach (var entryType in dto.EntryTypes)
            {
                foreach (var fieldTab in entryType.FieldTabs)
                {
                    foreach (var entryField in fieldTab.Fields)
                    {
                        entryField.Field = fieldsDto.FirstOrDefault(f => f.Id == entryField.FieldId);
                    }
                }
            }
        }
    }
}
