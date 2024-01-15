using Dignite.Cms.Fields;
using Dignite.Cms.Sections;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dignite.Cms.Admin.Fields
{
    public class FieldAdminAppService : CmsAdminAppServiceBase, IFieldAdminAppService
    {
        private readonly IFieldRepository  _fieldRepository;

        public FieldAdminAppService(IFieldRepository fieldRepository)
        {
            _fieldRepository = fieldRepository;
        }

        public async Task<FieldDto> CreateAsync(CreateFieldInput input)
        {
            await CheckNameExistenceAsync(input.Name);
            var entity = new Field(
                GuidGenerator.Create(),
                input.GroupId.HasValue ? (input.GroupId.Value == Guid.Empty ? null : input.GroupId.Value) : null,
                input.Name,
                input.DisplayName,
                input.Description,
                input.FormControlName,
                input.FormConfiguration,
                CurrentTenant.Id);
            await _fieldRepository.InsertAsync(entity);

            var dto =
                ObjectMapper.Map<Field, FieldDto>(
                    entity
                    );

            return dto;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _fieldRepository.DeleteAsync(id);
        }

        public virtual async Task<FieldDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Field, FieldDto>(
                await _fieldRepository.GetAsync(id)
            );
        }

        public async Task<PagedResultDto<FieldDto>> GetListAsync(GetFieldsInput input)
        {
            var count = await _fieldRepository.GetCountAsync(input.GroupId, input.Filter);
            var result = await _fieldRepository.GetListAsync(input.GroupId,input.Filter,input.MaxResultCount, input.SkipCount,input.Sorting);
            var dto =
                ObjectMapper.Map<List<Field>, List<FieldDto>>(
                    result
                    );

            return new PagedResultDto<FieldDto>(count, dto);
        }

        public async Task<FieldDto> UpdateAsync(Guid id, UpdateFieldInput input)
        {
            var entity = await _fieldRepository.GetAsync(id,false);
            if (!entity.Name.Equals(input.Name, StringComparison.OrdinalIgnoreCase))
            {
                await CheckNameExistenceAsync(input.Name, id);
            }
            entity.SetDisplayName(input.DisplayName);
            entity.SetName(input.Name);
            entity.SetDescription(input.Description);
            entity.SetFormControlName(input.FormControlName);
            entity.SetGroupId(input.GroupId.HasValue ? (input.GroupId.Value == Guid.Empty ? null : input.GroupId.Value) : null);
            entity.SetFormConfigurationDictionary(input.FormConfiguration);
            await _fieldRepository.UpdateAsync(entity);

            var dto =
                ObjectMapper.Map<Field, FieldDto>(
                    entity
                    );

            return dto;
        }
        public async Task<FieldDto> FindByNameAsync(string name)
        {
            var result = await _fieldRepository.FindByNameAsync(name);
            return ObjectMapper.Map<Field, FieldDto>(result);
        }

        protected virtual async Task CheckNameExistenceAsync(string name, Guid? ignoredId = null)
        {
            if (await _fieldRepository.NameExistsAsync(name, ignoredId))
            {
                throw new FieldNameAlreadyExistException(name);
            }
        }
    }
}
