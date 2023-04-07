using Dignite.Cms.Fields;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace Dignite.Cms.Admin.Fields
{
    public class FieldGroupAdminAppService : CmsAdminAppServiceBase, IFieldGroupAdminAppService
    {
        private readonly IFieldGroupRepository _fieldGroupRepository;

        public FieldGroupAdminAppService(IFieldGroupRepository fieldGroupRepository)
        {
            _fieldGroupRepository = fieldGroupRepository;
        }

        public async Task<FieldGroupDto> CreateAsync(CreateOrUpdateFieldGroupInput input)
        {
            var entity = new FieldGroup(GuidGenerator.Create(),input.Name,CurrentTenant.Id);
            await _fieldGroupRepository.InsertAsync(entity);

            var dto =
                ObjectMapper.Map<FieldGroup, FieldGroupDto>(
                    entity
                    );

            return dto;
        }

        public async Task DeleteAsync(Guid id)
        {
            await _fieldGroupRepository.DeleteAsync(id);
        }

        public async Task<FieldGroupDto> GetAsync(Guid id)
        {
            var entity = await _fieldGroupRepository.GetAsync(id);
            var dto =
                ObjectMapper.Map<FieldGroup, FieldGroupDto>(
                    entity
                    );

            return dto;
        }

        public async Task<ListResultDto<FieldGroupDto>> GetListAsync()
        {
            var result = await _fieldGroupRepository.GetListAsync();
            var dto =
                ObjectMapper.Map<List<FieldGroup>, List<FieldGroupDto>>(
                    result
                    );

            return new ListResultDto<FieldGroupDto>( dto);
        }

        public async Task<PagedResultDto<FieldGroupDto>> GetListAsync(GetFieldGroupsInput input)
        {
            var result = await _fieldGroupRepository.GetListAsync();
            var dto =
                ObjectMapper.Map<List<FieldGroup>, List<FieldGroupDto>>(
                    result
                    );

            return new PagedResultDto<FieldGroupDto>(result.Count, dto);
        }

        public async Task<FieldGroupDto> UpdateAsync(Guid id, CreateOrUpdateFieldGroupInput input)
        {
            var entity = await _fieldGroupRepository.GetAsync(id,false);
            entity.SetName(input.Name);
            await _fieldGroupRepository.UpdateAsync(entity);

            var dto =
                ObjectMapper.Map<FieldGroup, FieldGroupDto>(
                    entity
                    );

            return dto;
        }
    }
}
