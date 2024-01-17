using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.Domain.Services;
using Dignite.Abp.Data;
using Dignite.Cms.Sections;
using Dignite.Cms.Fields;
using static Dignite.Cms.CmsErrorCodes;

namespace Dignite.Cms.Entries
{
    public class EntryManager : DomainService
    {
        protected readonly IEntryRepository _entryRepository;
        protected readonly IEntryTypeRepository _entryTypeRepository;
        protected readonly IFieldRepository _fieldRepository;

        public EntryManager(IEntryRepository entryRepository, IEntryTypeRepository entryTypeRepository, IFieldRepository fieldRepository)
        {
            _entryRepository = entryRepository;
            _entryTypeRepository = entryTypeRepository;
            _fieldRepository = fieldRepository;
        }

        public virtual async Task<Entry> CreateAsync(Guid entryTypeId, string culture, string title, string slug,
            DateTime publishTime, EntryStatus status, Guid? parentId, ExtraPropertyDictionary extraProperties,
            Guid? initialVersionId, string versionNotes,Guid? tenantId)
        {
            var entryType = await _entryTypeRepository.GetAsync(entryTypeId);
            await CheckSlugExistenceAsync(entryType.SectionId, culture, slug);
            await CheckExtraPropertiesAsync(entryType,extraProperties);

            var order = (await _entryRepository.GetMaxOrderAsync(entryType.SectionId, culture, parentId)) + 1;
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

        public virtual async Task<Entry> UpdateAsync(Guid id, string culture, string title, string slug,
            DateTime publishTime, EntryStatus status, ExtraPropertyDictionary extraProperties,
            string versionNotes)
        {
            var entry = await _entryRepository.GetAsync(id, false);
            var entryType = await _entryTypeRepository.GetAsync(entry.EntryTypeId);
            await CheckExtraPropertiesAsync(entryType, extraProperties);

            if (!culture.Equals(entry.Culture, StringComparison.OrdinalIgnoreCase) ||
                !slug.Equals(entry.Slug, StringComparison.OrdinalIgnoreCase))
            {
                await CheckSlugExistenceAsync(entry.SectionId, culture, slug);
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
            var allEntries = (await _entryRepository.GetListAsync(entry.SectionId, entry.Culture))
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
        protected virtual async Task CheckSlugExistenceAsync(Guid sectionId, string culture, string slug)
        {
            if (await _entryRepository.SlugExistsAsync(sectionId, culture, slug))
            {
                throw new EntrySlugAlreadyExistException(culture, slug);
            }
        }
        protected virtual async Task CheckExtraPropertiesAsync(EntryType entryype, ExtraPropertyDictionary extraProperties)
        {
            var fields = await _fieldRepository.GetListAsync( entryype.FieldTabs.SelectMany(ft => ft.Fields).Select(ef => ef.FieldId));
            if (extraProperties.Keys.Except(fields.Select(f=>f.Name)).Any())
            {
                throw new Volo.Abp.AbpException("Custom field values do not match custom fields of the entry type!");
            }
        }
    }
}
