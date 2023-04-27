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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<SectionDto> FindByNameAsync(Guid siteId, string name)
        {
            var dto = ObjectMapper.Map<Section, SectionDto>(
                await _sectionRepository.FindByNameAsync(siteId, name)
                );

            await FillSectionFields( dto );
            return dto;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">
        /// 
        /// </param>
        /// <returns></returns>
        public async Task<SectionDto> FindByUrlAsync(string url)
        {
            url = url.RemovePreFix("/").RemovePostFix("/");
            var allSections = await _sectionRepository.GetListAsync(null,null,true,true);
            foreach ( var section in allSections) 
            {
                var route = section.EntryPage.Route.RemovePreFix("/").RemovePostFix("/");
                var extractResult = FormattedStringValueExtracter.Extract(url,route , ignoreCase: true);
                if (extractResult.IsMatch)
                {
                    return ObjectMapper.Map<Section, SectionDto>(section);
                }
            }

            return null;
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
