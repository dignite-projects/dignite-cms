using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;

namespace Dignite.Cms.Admin.Entries
{
    [RemoteService(Name = CmsAdminRemoteServiceConsts.RemoteServiceName)]
    [Area(CmsAdminRemoteServiceConsts.ModuleName)]
    [Authorize(Permissions.CmsAdminPermissions.Entry.Default)]
    [Route("api/cms-admin/entries")]
    public class EntryAdminController : CmsAdminController, IEntryAdminAppService
    {
        private readonly IEntryAdminAppService _entryAppService;

        public EntryAdminController(IEntryAdminAppService entryAppService)
        {
            _entryAppService = entryAppService;
        }


        /// <summary>
        /// 创建或更新条目
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>


        [HttpPost]
        public async Task<EntryDto> CreateAsync(CreateEntryInput input)
        {
            return await _entryAppService.CreateAsync(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [HttpPut]
        public async Task<EntryDto> UpdateAsync(Guid id,UpdateEntryInput input)
        {
            return await _entryAppService.UpdateAsync(id,input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task DeleteAsync(Guid id)
        {
            await _entryAppService.DeleteAsync(id);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<PagedResultDto<EntryDto>> GetListAsync(GetEntriesInput input)
        {
            return await _entryAppService.GetListAsync(input);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<EntryDto> GetAsync(Guid id)
        {
            return await _entryAppService.GetAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:Guid}/versions")]
        public async Task<ListResultDto<EntryDto>> GetVersionListAsync(Guid id)
        {
            return await _entryAppService.GetVersionListAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("activate/{id:Guid}/{version:int}")]
        public async Task ActivateAsync(Guid id, int version)
        {
            await _entryAppService.ActivateAsync(id, version);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id:Guid}/{version:int}")]
        public async Task<EntryDto> GetByVersion(Guid id, int version)
        {
            return await _entryAppService.GetByVersion(id,version);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("move/{id:Guid}")]
        public async Task MoveAsync(Guid id, MoveEntryInput input)
        {
            await _entryAppService.MoveAsync(id, input);
        }
    }
}
