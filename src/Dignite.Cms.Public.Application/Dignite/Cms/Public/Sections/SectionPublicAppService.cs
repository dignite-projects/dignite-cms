using Dignite.Cms.Entries;
using Dignite.Cms.Fields;
using Dignite.Cms.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Text.Formatting;

namespace Dignite.Cms.Public.Sections
{
    public class SectionPublicAppService : CmsPublicAppService, ISectionPublicAppService
    {
        private readonly ISectionRepository _sectionRepository;
        private readonly IFieldRepository _fieldRepository;

        public SectionPublicAppService(ISectionRepository sectionRepository, IFieldRepository fieldRepository)
        {
            _sectionRepository = sectionRepository;
            _fieldRepository= fieldRepository;
        }

        public async Task<SectionDto> GetAsync(Guid sectionId)
        {
            var dto = ObjectMapper.Map<Section, SectionDto>(
                await _sectionRepository.GetAsync(sectionId)
                );

            await FillSectionFields(dto);
            return dto;
        }

        public async Task<SectionDto> FindByNameAsync(Guid siteId, string name)
        {
            var dto = ObjectMapper.Map<Section, SectionDto>(
                await _sectionRepository.FindByNameAsync(siteId, name)
                );

            await FillSectionFields(dto);
            return dto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="entryPath">
        /// The entry path does not contain culture.
        /// </param>
        /// <returns></returns>
        public async Task<SectionDto> FindByEntryPathAsync(Guid siteId, string entryPath)
        {
            var allSections = await _sectionRepository.GetListAsync(siteId, null, true, true);
            var section = await MatchingSectionWithEntryPath(allSections, entryPath);

            /**
             * When no matching Section is found, add /index/ to the url to try again.
             * The reason is that when the Section type is SectionType.Single, the url may not contain a slug, so the default slug (i.e. index) is used to match again.
             * **/
            if (section == null)
            {
                entryPath = entryPath.EnsureEndsWith('/') + EntryConsts.DefaultSlug;
                section= await MatchingSectionWithEntryPath(allSections,entryPath);
            }

            return section;
        }

        public async Task<SectionDto> GetDefaultAsync(Guid siteId)
        {
            var result = await _sectionRepository.GetDefaultAsync(siteId);
            if (result == null)
            {
                return null;
            }
            else
            {
                var dto = ObjectMapper.Map<Section, SectionDto>(
                    result
                    );

                await FillSectionFields(dto);
                return dto;
            }
        }

        protected async Task<SectionDto> MatchingSectionWithEntryPath(List<Section> sections, string entryPath)
        {
            entryPath = entryPath.EnsureStartsWith('/').EnsureEndsWith('/');
            foreach (var section in sections.OrderByDescending(s => s.Route))
            {
                var route = section.Route.EnsureStartsWith('/').EnsureEndsWith('/');
                var extractResult = FormattedStringValueExtracter.Extract(entryPath, route, ignoreCase: true);
                if (
                    (!route.Contains("{slug}", StringComparison.InvariantCultureIgnoreCase) && route.Equals(entryPath,StringComparison.InvariantCultureIgnoreCase))
                    ||
                    (route.Contains("{slug}", StringComparison.InvariantCultureIgnoreCase) && extractResult.IsMatch)
                    )
                {
                    var dto = ObjectMapper.Map<Section, SectionDto>(
                        section
                        );
                    await FillSectionFields(dto);
                    return dto;
                }
            }

            return null;
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
