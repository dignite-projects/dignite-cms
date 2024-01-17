﻿using Dignite.Cms.Entries;
using Dignite.Cms.Sections;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using static Dignite.Cms.CmsErrorCodes;

namespace Dignite.Cms.Admin.Entries
{
    public class EntryAdminAppService : CmsAdminAppServiceBase, IEntryAdminAppService
    {
        private readonly IEntryRepository _entryRepository;        
        private readonly ISectionRepository _sectionRepository;
        private readonly EntryManager _entryManager;
        private readonly IEntryTypeRepository _entryTypeRepository;

        public EntryAdminAppService(
            IEntryRepository entryRepository, 
            ISectionRepository sectionRepository,
            EntryManager entryManager, 
            IEntryTypeRepository entryTypeRepository)
        {
            _entryRepository = entryRepository;
            _sectionRepository = sectionRepository;
            _entryManager = entryManager;
            _entryTypeRepository = entryTypeRepository;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [Authorize(Permissions.CmsAdminPermissions.Entry.Create)]
        public async Task<EntryDto> CreateAsync(CreateEntryInput input)
        {
            var entry = await _entryManager.CreateAsync(
                input.EntryTypeId,
                input.Culture,
                input.Title,
                input.Slug,
                input.PublishTime,
                input.Draft ? EntryStatus.Draft : EntryStatus.Published,
                input.ParentId,
                input.ExtraProperties,
                input.InitialVersionId,
                input.VersionNotes,
                CurrentTenant.Id
                );

            return ObjectMapper.Map<Entry, EntryDto>(entry);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(Permissions.CmsAdminPermissions.Entry.Update)]
        public async Task<EntryDto> UpdateAsync(Guid id, UpdateEntryInput input)
        {
            var entry = await _entryManager.UpdateAsync(
                id, input.Culture, input.Title, input.Slug, input.PublishTime,
                input.Draft ? EntryStatus.Draft : EntryStatus.Published,
                input.ExtraProperties, input.VersionNotes
                );

            return ObjectMapper.Map<Entry, EntryDto>(entry);
        }

        /// <summary>
        /// Delete entry;
        /// Delete all revisions synchronously;
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Permissions.CmsAdminPermissions.Entry.Delete)]
        public async Task DeleteAsync(Guid id)
        {
            var entry = await _entryRepository.GetAsync(id, false);

            await _entryRepository.DeleteAsync(entry);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(Permissions.CmsAdminPermissions.Entry.Default)]
        public async Task<PagedResultDto<EntryDto>> GetListAsync(GetEntriesInput input)
        {
            if (input.SectionId == Guid.Empty)
                return new PagedResultDto<EntryDto>(0, new List<EntryDto>());

            var count = await _entryRepository.GetCountAsync(input.SectionId, input.Culture, input.CreatorId, input.Status, input.Filter,input.StartPublishDate,input.ExpiryPublishDate, null);
            if (count == 0)
                return new PagedResultDto<EntryDto>(0, new List<EntryDto>());

            //get entry list
            var result = await _entryRepository.GetListAsync(input.SectionId, input.Culture, input.CreatorId, input.Status, input.Filter, input.StartPublishDate, input.ExpiryPublishDate, null, input.MaxResultCount, input.SkipCount, input.Sorting);
            var dto = ObjectMapper.Map<List<Entry>, List<EntryDto>>(result);

            return new PagedResultDto<EntryDto>(count, dto);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Permissions.CmsAdminPermissions.Entry.Default)]
        public async Task<EntryDto> GetAsync(Guid id)
        {
            var result = await _entryRepository.GetAsync(id, true);
            return ObjectMapper.Map<Entry, EntryDto>(
                result
                );
        }


        [Authorize(Permissions.CmsAdminPermissions.Entry.Default)]
        public async Task<ListResultDto<EntryDto>> GetAllVersionsAsync(Guid id)
        {
            var entry = await _entryRepository.GetAsync(id, false);
            var result = await _entryManager.GetAllVisionsAsync(entry);

            return new ListResultDto<EntryDto>(
                ObjectMapper.Map<List<Entry>, List<EntryDto>>(result)
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Permissions.CmsAdminPermissions.Entry.Update)]
        public async Task ActivateAsync(Guid id)
        {
            var entry = await _entryRepository.GetAsync(id, false);
            await _entryManager.ActivateAsync(entry);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Authorize(Permissions.CmsAdminPermissions.Entry.Update)]
        public async Task MoveAsync(Guid id, MoveEntryInput input)
        {
            var entry = await _entryRepository.GetAsync(id, false);
            var section = await _sectionRepository.GetAsync(entry.SectionId,false);
            if (section.Type == SectionType.Structure)
            {
                await _entryManager.MoveAsync(entry, input.ParentId, input.Order);
            }
            else
            {
                throw new Volo.Abp.AbpException("Only entries in the structural section can be moved!");
            }
        }
    }
}
