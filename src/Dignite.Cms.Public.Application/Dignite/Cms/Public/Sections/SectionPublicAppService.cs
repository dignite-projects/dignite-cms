using Dignite.Cms.Fields;
using Dignite.Cms.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
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
        /// <param name="siteId"></param>
        /// <param name="url">
        /// 
        /// </param>
        /// <returns></returns>
        public async Task<SectionDto> FindByUrlAsync(Guid siteId, string url)
        {
            url = url.RemovePreFix("/");
            var allSections = await _sectionRepository.GetListAsync(siteId,null,true,true);
            foreach ( var section in allSections) 
            {
                var route = section.EntryPage.Route.RemovePreFix("/");
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
            var dto = ObjectMapper.Map<Section, SectionDto>(
                await _sectionRepository.GetDefaultAsync(siteId)
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
