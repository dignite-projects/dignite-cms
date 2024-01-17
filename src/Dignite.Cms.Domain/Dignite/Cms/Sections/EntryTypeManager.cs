using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;

namespace Dignite.Cms.Sections
{
    public class EntryTypeManager : DomainService
    {
        protected readonly IEntryTypeRepository _entryTypeRepository;

        public EntryTypeManager(IEntryTypeRepository entryTypeRepository)
        {
            _entryTypeRepository = entryTypeRepository;
        }

        public virtual async Task<EntryType> CreateAsync(Guid sectionId, string displayName, string name,IList<EntryFieldTab> fieldTabs)
        {
            await CheckNameExistenceAsync(sectionId, name);

            var entity = new EntryType(
                GuidGenerator.Create(),
                sectionId,
                displayName,
                name,
                fieldTabs,
                CurrentTenant.Id);
            return await _entryTypeRepository.InsertAsync(entity);
        }
        public virtual async Task<EntryType> UpdateAsync(Guid id, string displayName, string name, IList<EntryFieldTab> fieldTabs)
        {
            var entity = await _entryTypeRepository.GetAsync(id, false);
            if (!entity.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            {
                await CheckNameExistenceAsync(entity.Id, name);
            }
            entity.SetDisplayName(displayName);
            entity.SetName(name);
            entity.FieldTabs.Clear();
            foreach (var ft in fieldTabs)
            {
                entity.FieldTabs.Add(ft);
            }

            //
            return await _entryTypeRepository.UpdateAsync(entity);
        }

        protected virtual async Task CheckNameExistenceAsync(Guid sectionId, string name)
        {
            if (await _entryTypeRepository.NameExistsAsync(sectionId, name))
            {
                throw new EntryTypeNameAlreadyExistException(name);
            }
        }
    }
}
