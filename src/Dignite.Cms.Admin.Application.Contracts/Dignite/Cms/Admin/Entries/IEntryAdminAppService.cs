using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dignite.Cms.Admin.Entries
{
    public interface IEntryAdminAppService
    : ICrudAppService<
        EntryDto,
        Guid,
        GetEntriesInput,
        CreateEntryInput,
        UpdateEntryInput>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ListResultDto<EntryDto>> GetAllVersionsAsync(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task ActivateAsync(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task MoveAsync(Guid id, MoveEntryInput input);

        Task<bool> SlugExistsAsync(SlugExistsInput input);

        /// <summary>
        /// Determines whether creation of an entry is allowed under the specified entry type.
        /// When the section is of type Single and an entry already exists under the specified entryTypeId, no new entries are allowed to be created.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<bool> CanCreateForEntryTypeAsync(CanCreateEntryForSectionInput input);
    }
}
