using Dignite.Cms.Entries;
using Dignite.Cms.Fields;
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

        public SectionAdminAppService(ISectionRepository sectionRepository, IFieldRepository fieldRepository)
        {
            _sectionRepository = sectionRepository;
            _fieldRepository = fieldRepository;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [Authorize(Permissions.CmsAdminPermissions.Section.Create)]
        public async Task<SectionDto> CreateAsync(CreateSectionInput input)
        {
            await CheckNameExistenceAsync(input.SiteId, input.Name);
            await CheckRouteExistenceAsync(input.SiteId, input.EntryPage.Route);

            //
            var section = new Section(
                GuidGenerator.Create(),
                input.SiteId,
                input.Type,
                input.DisplayName, input.Name,
                input.IsDefault,
                input.IsActive,
                new EntryPage(input.EntryPage.Route,input.EntryPage.Template),
                CurrentTenant.Id);

            //
            CheckSectionType(section);
            CheckSlugRoutingParameter(section);

            //
            if (section.IsDefault)
            {
                var sections = await _sectionRepository.GetListAsync(section.SiteId);
                foreach (var item in sections)
                {
                    section.SetDefault(false);
                }
            }

            //
            await _sectionRepository.InsertAsync(section);
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
            var section = await _sectionRepository.GetAsync(id);
            if (!section.Name.Equals(input.Name,StringComparison.OrdinalIgnoreCase))
            {
                await CheckNameExistenceAsync(section.SiteId, input.Name);
            }
            if (!section.EntryPage.Route.Equals(input.EntryPage.Route, StringComparison.OrdinalIgnoreCase))
            {
                await CheckRouteExistenceAsync(section.SiteId, input.EntryPage.Route);
            }

            //
            if (input.IsDefault && !section.IsDefault)
            {
                var sections = await _sectionRepository.GetListAsync(section.SiteId);
                foreach (var item in sections)
                {
                    item.SetDefault(false);
                }
            }

            //
            section.SetActive(input.IsActive);
            section.SetDefault(input.IsDefault); 
            section.SetDisplayName(input.DisplayName);
            section.SetEntryPage(new EntryPage(input.EntryPage.Route, input.EntryPage.Template));
            section.SetName(input.Name);
            section.SetType(input.Type);

            //
            CheckSectionType(section);
            CheckSlugRoutingParameter(section);

            //
            await _sectionRepository.UpdateAsync(section);
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


        protected virtual async Task CheckNameExistenceAsync(Guid siteId, string name)
        {
            if (await _sectionRepository.NameExistsAsync(siteId,name))
            {
                throw new SectionNameAlreadyExistException( name);
            }
        }
        protected virtual async Task CheckRouteExistenceAsync(Guid siteId, string route)
        {
            if (await _sectionRepository.RouteExistsAsync(siteId, route))
            {
                throw new SectionRouteAlreadyExistException(route);
            }
        }


        protected virtual void CheckSectionType(Section section)
        {
            if (section.IsDefault && section.Type!= SectionType.Single)
            {
                throw new DefaultSectionMustBeSingleTypeException(section.Name);
            }
        }


        protected virtual void CheckSlugRoutingParameter(Section section)
        {
            if (section.Type != SectionType.Single && !section.EntryPage.Route.Contains($"{{{nameof(Entry.Slug)}}}", StringComparison.OrdinalIgnoreCase))
            {
                throw new MissingSlugRoutingParameterException(section.Type, section.EntryPage.Route);
            }
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
