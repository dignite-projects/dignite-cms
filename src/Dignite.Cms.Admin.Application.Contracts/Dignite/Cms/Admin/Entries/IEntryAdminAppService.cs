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
        Task<ListResultDto<EntryDto>> GetVersionListAsync(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task ActivateAsync(Guid id,int version);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        Task<EntryDto> GetByVersion(Guid id, int version);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task MoveAsync(Guid id, MoveEntryInput input);
    }
}
