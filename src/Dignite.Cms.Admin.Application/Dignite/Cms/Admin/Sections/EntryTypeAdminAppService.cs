using Dignite.Cms.Fields;
using Dignite.Cms.Sections;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Dignite.Cms.Admin.Sections
{
    public class EntryTypeAdminAppService : CmsAdminAppServiceBase, IEntryTypeAdminAppService
    {
        private readonly IEntryTypeRepository _entryTypeRepository;
        private readonly IFieldRepository _fieldRepository;

        public EntryTypeAdminAppService(IEntryTypeRepository entryTypeRepository, IFieldRepository fieldRepository)
        {
            _entryTypeRepository = entryTypeRepository;
            _fieldRepository= fieldRepository;
        }

        [Authorize(Permissions.CmsAdminPermissions.Section.Create)]
        public async Task<EntryTypeDto> CreateAsync(CreateEntryTypeInput input)
        {
            await CheckNameExistenceAsync(input.SectionId, input.Name);

            var entity = new EntryType(
                GuidGenerator.Create(), 
                input.SectionId, 
                input.DisplayName, 
                input.Name, 
                input.FieldTabs.Select(ft=>
                    new EntryFieldTab(
                        ft.Name,
                        ft.Fields.Select(f=>
                            new EntryField(
                                f.FieldId,
                                f.DisplayName,
                                f.Searchable
                                )).ToList()
                            )).ToList(), 
                CurrentTenant.Id);
            await _entryTypeRepository.InsertAsync(entity);

            return ObjectMapper.Map<EntryType, EntryTypeDto>(entity);
        }

        [Authorize(Permissions.CmsAdminPermissions.Section.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            await _entryTypeRepository.DeleteAsync(id);
        }

        [Authorize(Permissions.CmsAdminPermissions.Section.Default)]
        public async Task<EntryTypeDto> GetAsync(Guid id)
        {
            var entity = await _entryTypeRepository.GetAsync(id);
            var allFields = await _fieldRepository.GetListAsync(
                entity.FieldTabs.SelectMany(f=>f.Fields).Select(f=>f.FieldId)
                );
            var dto = ObjectMapper.Map<EntryType, EntryTypeDto>(entity);
            foreach (var tab in dto.FieldTabs)
            {
                foreach (var ef in tab.Fields)
                {
                    ef.Field = ObjectMapper.Map<Field,FieldDto>( allFields.FirstOrDefault(f => f.Id == ef.FieldId));
                }
            }

            return dto;
        }

        [Authorize(Permissions.CmsAdminPermissions.Section.Update)]
        public async Task<EntryTypeDto> UpdateAsync(Guid id, UpdateEntryTypeInput input)
        {
            var entity = await _entryTypeRepository.GetAsync(id,false);
            if (!entity.Name.Equals(input.Name, StringComparison.OrdinalIgnoreCase))
            {
                await CheckNameExistenceAsync(entity.Id, input.Name, id);
            }
            entity.SetDisplayName(input.DisplayName);
            entity.SetName(input.Name);
            entity.FieldTabs.Clear();
            foreach (var ft in input.FieldTabs)
            {
                entity.FieldTabs.Add(
                    new EntryFieldTab(
                        ft.Name, 
                        ft.Fields.Select(f => new EntryField(
                                f.FieldId,
                                f.DisplayName,
                                f.Searchable
                                )).ToList())
                    );
            }

            //
            await _entryTypeRepository.UpdateAsync(entity);
            return ObjectMapper.Map<EntryType, EntryTypeDto>(entity);
        }

        protected virtual async Task CheckNameExistenceAsync(Guid sectionId, string name, Guid? ignoredId = null)
        {
            if (await _entryTypeRepository.NameExistsAsync(sectionId, name, ignoredId))
            {
                throw new EntryTypeNameAlreadyExistException(name);
            }
        }
    }
}
