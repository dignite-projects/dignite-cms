using Dignite.Abp.DynamicForms;
using Dignite.Abp.FieldCustomizing;
using Dignite.Cms.Entries;
using Dignite.Cms.Public.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dignite.Cms.Public.Entries
{
    public class EntryPublicAppService : CmsPublicAppService, IEntryPublicAppService
    {
        private readonly IEntryRepository _entryRepository;
        private readonly ISectionPublicAppService _sectionPublicAppService;

        public EntryPublicAppService(IEntryRepository entryRepository, ISectionPublicAppService sectionPublicAppService)
        {
            _entryRepository = entryRepository;
            _sectionPublicAppService= sectionPublicAppService;
        }

        public async Task<EntryDto> FindBySlugAsync(FindBySlugInput input)
        {
            var section = await _sectionPublicAppService.GetAsync(input.SectionId);
            if (input.Language.IsNullOrEmpty())
            {
                input.Language = section.Site.Languages.OrderByDescending(l => l.IsDefault).First().Language;
            }
            var entry = await _entryRepository.FindBySlugAsync(input.SectionId,input.Language,input.Slug);

            return GetEntryDto(section,entry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EntryDto> FindPrevAsync(Guid id)
        {
            var entry = await _entryRepository.FindPrevAsync(id);
            var section = await _sectionPublicAppService.GetAsync(entry.SectionId);
            return GetEntryDto(section, entry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EntryDto> FindNextAsync(Guid id)
        {
            var entry = await _entryRepository.FindNextAsync(id);
            var section = await _sectionPublicAppService.GetAsync(entry.SectionId);
            return GetEntryDto(section, entry);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<EntryDto>> GetListAsync(GetEntriesInput input)
        {
            var section = await _sectionPublicAppService.GetAsync(input.SectionId);
            if (input.Language.IsNullOrEmpty())
            {
                input.Language = section.Site.Languages.OrderByDescending(l => l.IsDefault).First().Language;
            }
            List<QueryingByFieldParameter> queryingByFieldParameters = input.QueryingByFieldParameters.IsNullOrEmpty()? null: JsonSerializer.Deserialize<List<QueryingByFieldParameter>>(input.QueryingByFieldParameters);

            var count = await _entryRepository.GetCountAsync(input.SectionId, input.Language, input.CreatorId, EntryStatus.Published,input.Filter,input.StartPublishDate,input.ExpiryPublishDate, queryingByFieldParameters);
            if (count == 0)
            {
                return new PagedResultDto<EntryDto>(0, new List<EntryDto>());
            }
            var result = await _entryRepository.GetListAsync(
                    input.SectionId,
                    input.Language,
                    input.CreatorId,
                    EntryStatus.Published,
                    input.Filter,
                    input.StartPublishDate,
                    input.ExpiryPublishDate,
                    queryingByFieldParameters,
                    input.MaxResultCount,
                    input.SkipCount
                    );

            var dto = ObjectMapper.Map<List<Entry>, List<EntryDto>>(result);
            foreach (var entry in dto)
            {
                entry.SetDefaultsForCustomizeFields(
                    section.EntryTypes.First(et => et.Id == entry.EntryTypeId)
                    .FieldTabs
                    .SelectMany(ft => ft.Fields)
                    .Select(f => f.Field)
                    .ToList()
                    .AsReadOnly()
                );
            }

            return new PagedResultDto<EntryDto>(count, dto);
        }


        protected EntryDto GetEntryDto(SectionDto section, Entry entry)
        {
            if (entry == null)
            {
                return null;
            }

            var dto = ObjectMapper.Map<Entry, EntryDto>(entry);
            dto.SetDefaultsForCustomizeFields(
                section.EntryTypes.First(et => et.Id == entry.EntryTypeId)
                .FieldTabs
                .SelectMany(ft => ft.Fields)
                .Select(f => f.Field)
                .ToList()
                .AsReadOnly()
            );

            return dto;
        }
    }
}
