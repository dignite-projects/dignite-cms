using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;
using Dignite.Abp.Data;
using Dignite.Cms.Sections;
using Dignite.Cms.Fields;

namespace Dignite.Cms.Entries
{
    public class EntryManager : DomainService
    {
        protected readonly ISectionRepository _sectionRepository;
        protected readonly IEntryRepository _entryRepository;
        protected readonly IEntryTypeRepository _entryTypeRepository;
        protected readonly IFieldRepository _fieldRepository;

        public EntryManager(ISectionRepository sectionRepository, IEntryRepository entryRepository, IEntryTypeRepository entryTypeRepository, IFieldRepository fieldRepository)
        {
            _sectionRepository = sectionRepository;
            _entryRepository = entryRepository;
            _entryTypeRepository = entryTypeRepository;
            _fieldRepository = fieldRepository;
        }

        public virtual async Task<Entry> CreateAsync(Guid entryTypeId, string culture, string title, string slug,
            DateTime publishTime, EntryStatus status, Guid? parentId, ExtraPropertyDictionary extraProperties,
            Guid? initialVersionId, string versionNotes,Guid? tenantId)
        {
            var entryType = await _entryTypeRepository.GetAsync(entryTypeId);
            await ExistForTypeAsync(culture, entryType);
            await CheckSlugExistenceAsync(culture, entryType.SectionId, slug);
            await CheckExtraPropertiesAsync(entryType,extraProperties);

            var order = (await _entryRepository.GetMaxOrderAsync(culture,entryType.SectionId,  parentId)) + 1;
            var entry = new Entry(
                GuidGenerator.Create(),
                entryType.SectionId,
                entryTypeId,
                culture,
                title,
                slug,
                publishTime,
                status,
                parentId,
                order,
                initialVersionId,
                versionNotes,
                tenantId);

            foreach (var item in extraProperties)
            {
                entry.SetField(item.Key, item.Value);
            }

            //          
            return await _entryRepository.InsertAsync(entry);
        }

        public virtual async Task<Entry> UpdateAsync(
            Guid id, string culture, string title, string slug,
            DateTime publishTime, EntryStatus status, ExtraPropertyDictionary extraProperties,
            string versionNotes)
        {
            var entry = await _entryRepository.GetAsync(id, false);
            var entryType = await _entryTypeRepository.GetAsync(entry.EntryTypeId);
            await CheckExtraPropertiesAsync(entryType, extraProperties);

            if (!culture.Equals(entry.Culture, StringComparison.OrdinalIgnoreCase) ||
                !slug.Equals(entry.Slug, StringComparison.OrdinalIgnoreCase))
            {
                await CheckSlugExistenceAsync(culture,entry.SectionId,  slug);
            }

            //
            entry.Title = title;
            entry.Slug = slug;
            entry.PublishTime = publishTime;
            entry.SetStatus(status);
            entry.Culture = culture;
            entry.VersionNotes = versionNotes;

            entry.ExtraProperties.Clear();
            foreach (var item in extraProperties)
            {
                entry.SetField(item.Key, item.Value);
            }

            //
            return await _entryRepository.UpdateAsync(entry);
        }

        public virtual async Task<List<Entry>> GetAllVisionsAsync(Entry entry)
        {
            var list = await _entryRepository.GetVisionListAsync(entry);
            if (entry.InitialVersionId.HasValue)
            {
                var initialVersionEntry = await _entryRepository.FindAsync(entry.InitialVersionId.Value);
                if (initialVersionEntry != null)
                {
                    list.Add(initialVersionEntry);
                }
            }
            else
            {
                list.Add(entry);
            }
            return list;
        }

        public virtual async Task ActivateAsync(Entry entry)
        {
            if (!entry.IsActivatedVersion)
            {
                var visions = await GetAllVisionsAsync(entry);
                foreach (var item in visions)
                {
                    if (item.IsActivatedVersion)
                    {
                        item.SetIsActivatedVersion(false);
                    }
                }
                entry.SetIsActivatedVersion(true);
            }
        }

        public virtual async Task MoveAsync(Entry entry, Guid? parentId,int order)
        {
            var allEntries = (await _entryRepository.GetListAsync(entry.Culture,entry.SectionId))
                .Where(e => e.ParentId == parentId && e.Order >= order);
            var allVisions = await GetAllVisionsAsync(entry);

            foreach (var item in allEntries)
            {
                item.SetOrder(item.ParentId,item.Order + 1);
            }
            
            foreach (var item in allVisions)
            {
                item.SetOrder(parentId,order);
            }
        }
        protected virtual async Task ExistForTypeAsync(string culture, EntryType entryType)
        {
            var section = await _sectionRepository.GetAsync(entryType.SectionId);
            if (section.Type == SectionType.Single)
            {
                if (await _entryRepository.ExistForEntryTypeAsync(culture, entryType.SectionId, entryType.Id))
                {
                    throw new EntryExistForTypeException(culture, entryType.DisplayName);
                }
            }
        }
        protected virtual async Task CheckSlugExistenceAsync(string culture,Guid sectionId,  string slug)
        {
            if (await _entryRepository.SlugExistsAsync(culture,sectionId, slug))
            {
                throw new EntrySlugAlreadyExistException(culture, slug);
            }
        }
        protected virtual async Task CheckExtraPropertiesAsync(EntryType entryype, ExtraPropertyDictionary extraProperties)
        {
            var fields = await _fieldRepository.GetListAsync( 
                entryype.FieldTabs
                        .SelectMany(ft => ft.Fields)
                        .Select(ef => ef.FieldId)
                );
            if (extraProperties.Keys.Except(fields.Select(f=>f.Name)).Any())
            {
                throw new Volo.Abp.AbpException("Custom field values do not match custom fields of the entry type!");
            }
        }
    }
}
