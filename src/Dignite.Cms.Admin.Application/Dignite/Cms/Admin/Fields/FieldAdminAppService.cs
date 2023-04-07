using Dignite.Abp.DynamicForms;
using Dignite.Cms.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dignite.Cms.Admin.Fields
{
    public class FieldAdminAppService : CmsAdminAppServiceBase, IFieldAdminAppService
    {
        private readonly IFieldRepository  _fieldRepository;
        private readonly IEnumerable<IForm> _forms;

        public FieldAdminAppService(IFieldRepository fieldRepository, IEnumerable<IForm> forms)
        {
            _fieldRepository = fieldRepository;
            _forms = forms;
        }

        public async Task<FieldDto> CreateAsync(CreateFieldInput input)
        {
            var entity = new Field(GuidGenerator.Create(),
                input.DisplayName,
                input.Name,
                input.DefaultValue,
                input.FormName,
                input.FormConfiguration,
                input.GroupId.HasValue ? (input.GroupId.Value == Guid.Empty ? null : input.GroupId.Value) : null,
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

        public async Task<ListResultDto<FormDto>> GetFormsAsync()
        {
            return await Task.FromResult(
                new ListResultDto<FormDto>(
                _forms.Select(
                    f=>new FormDto(f.Name,f.DisplayName,f.FormType)
                    ).ToList()
                    ));
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
            entity.SetDisplayName(input.DisplayName);
            entity.SetName(input.Name);
            entity.SetDefaultValue(input.DefaultValue);
            entity.SetFormName(input.FormName);
            entity.SetGroupId(input.GroupId.HasValue ? (input.GroupId.Value == Guid.Empty ? null : input.GroupId.Value) : null);
            entity.SetFormConfigurationDictionary(input.FormConfiguration);
            await _fieldRepository.UpdateAsync(entity);

            var dto =
                ObjectMapper.Map<Field, FieldDto>(
                    entity
                    );

            return dto;
        }
    }
}
