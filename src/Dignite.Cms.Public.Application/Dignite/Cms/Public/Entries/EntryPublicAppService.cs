using Dignite.Abp.DynamicForms;
using Dignite.Abp.FieldCustomizing;
using Dignite.Cms.Entries;
using Dignite.Cms.Public.Sections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
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
            int count = 0;
            List<Entry> result = new List<Entry>();
            var section = await _sectionPublicAppService.GetAsync(input.SectionId);
            if (input.Language.IsNullOrEmpty())
            {
                input.Language = section.Site.Languages.OrderByDescending(l => l.IsDefault).First().Language;
            }

            if (section.Type == Cms.Sections.SectionType.Single)
            {
                result = await _entryRepository.GetListAsync(input.SectionId, input.Language, null, EntryStatus.Published, null, null, null, null, 1, 0);
            }
            else if (section.Type == Cms.Sections.SectionType.Structure)
            {
                result = await _entryRepository.GetListAsync(input.SectionId, input.Language, null, EntryStatus.Published, null, null, null, null, 1000, 0);
                count = result.Count;
            }
            else
            {
                List<QueryingByFieldParameter> queryingByFieldParameters = input.QueryingByFieldParameters.IsNullOrEmpty() ? null : JsonSerializer.Deserialize<List<QueryingByFieldParameter>>(input.QueryingByFieldParameters);
                count = await _entryRepository.GetCountAsync(input.SectionId, input.Language, input.CreatorId, EntryStatus.Published, input.Filter, input.StartPublishDate, input.ExpiryPublishDate, queryingByFieldParameters);
                if (count == 0)
                {
                    return new PagedResultDto<EntryDto>(0, new List<EntryDto>());
                }
                result = await _entryRepository.GetListAsync(
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
            }

            var dto = ObjectMapper.Map<List<Entry>, List<EntryDto>>(result);
            foreach (var entry in dto)
            {
                SetEntryUrl(entry, section);
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
            SetEntryUrl(dto, section);
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

        /// <summary>
        /// set entry url
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="section"></param>
        protected void SetEntryUrl(EntryDto entry, SectionDto section)
        {
            var routeParameters = GetRouteParameters(section.EntryPage.Route).ToArray();
            var siteDefaultLanguage = section.Site.Languages.OrderByDescending(l => l.IsDefault).First().Language;
            entry.Url = section.EntryPage.Route;

            //If there is a routing parameter, get the routing parameter value and update the URL
            if (routeParameters.Any()) 
            {
                foreach (string routePerameter in routeParameters)
                {
                    var routeParameterName = routePerameter.RemovePreFix("{").RemovePostFix("}");
                    if (routeParameterName.IndexOf(':') > -1)
                    {
                        var propertyName = routeParameterName.Split(':')[0];
                        var parameterFormat = $"{{0:{routeParameterName.Split(':')[1]}}}";
                        var propertyValue = GetPropertyValue(entry, propertyName);
                        entry.Url = entry.Url.Replace(routePerameter, string.Format(parameterFormat, propertyValue));
                    }
                    else
                    {
                        var propertyValue = GetPropertyValue(entry, routeParameterName);
                        entry.Url = entry.Url.Replace(routePerameter, propertyValue.ToString());
                    }
                }
            }

            //splice language path
            if (!siteDefaultLanguage.Equals(entry.Language, StringComparison.OrdinalIgnoreCase))
            {
                entry.Url = entry.Language + entry.Url.EnsureStartsWith('/');
            }

            entry.Url = section.Site.BaseUrl.EnsureEndsWith('/') + entry.Url.RemovePreFix("/");
        }

        /// <summary>
        /// Get Route Parameters
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        private IEnumerable<string> GetRouteParameters(string route)
        {
            Regex regex = new Regex(@"\{[a-zA-Z][\w:\-.\/]*\}");
            var matchCollection = regex.Matches(route);

            for (int i = 0; i < matchCollection.Count; i++)
            {
                yield return matchCollection[i].Groups[0].Value;
            }
        }

        /// <summary>
        /// Using reflection to get the value of an entry property
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <exception cref="Volo.Abp.AbpException"></exception>
        private object GetPropertyValue(EntryDto entry, string propertyName)
        {
            Type type = entry.GetType();
            var property = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.IgnoreCase| BindingFlags.Instance);
            if (property != null)
            {
                return property.GetValue(entry, new object[0]);
            }
            else
            {
                throw new Volo.Abp.AbpException($"The entry property corresponding to the routing parameter {propertyName} was not found in the entry");
            }
        }
    }
}
