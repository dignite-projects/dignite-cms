using Dignite.Cms.Entries;
using Dignite.Cms.Sections;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Dignite.Abp.Data;

namespace Dignite.Cms.Admin.Entries
{
    public class EntryAdminAppService : CmsAdminAppServiceBase, IEntryAdminAppService
    {
        private readonly IEntryRepository _entryRepository;        
        private readonly ISectionRepository _sectionRepository;

        public EntryAdminAppService(
            IEntryRepository entryRepository, 
            ISectionRepository sectionRepository)
        {
            _entryRepository = entryRepository;
            _sectionRepository = sectionRepository;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [Authorize(Permissions.CmsAdminPermissions.Entry.Create)]
        public async Task<EntryDto> CreateAsync(CreateEntryInput input)
        {
            await CheckSlugExistenceAsync(input.SectionId,input.Culture, input.Slug);

            var id = GuidGenerator.Create();
            var order = (await _entryRepository.GetMaxOrderAsync(input.SectionId,input.Culture, input.ParentId))+1;
            var entry = new Entry(
                id, 
                input.SectionId, 
                input.EntryTypeId,
                input.Culture,
                input.Title, 
                input.Slug, 
                input.PublishTime,
                input.Draft? EntryStatus.Draft: EntryStatus.Published,
                input.ParentId,
                order,
                input.InitialVersionId,
                input.IsActivatedVersion,
                input.VersionNotes,
                CurrentTenant.Id);

            foreach (var item in input.ExtraProperties)
            {
                entry.SetField(item.Key, item.Value);
            }

            //          
            await _entryRepository.InsertAsync(entry);
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
            var entry = await _entryRepository.GetAsync(id, false);

            if (input.EntryTypeId != entry.EntryTypeId)
            {
                throw new Volo.Abp.AbpException("The system does not allow modification of the entry type!");
            }

            if (!input.Culture.Equals(entry.Culture, StringComparison.OrdinalIgnoreCase) || 
                !input.Slug.Equals(entry.Slug,StringComparison.OrdinalIgnoreCase))
            {
                await CheckSlugExistenceAsync(entry.SectionId,input.Culture, input.Slug);
            }

            //
            entry.Title=input.Title;
            entry.Slug = input.Slug;
            entry.PublishTime = input.PublishTime;
            entry.Culture = input.Culture;            
            entry.SetIsActivatedVersion(input.IsActivatedVersion);
            entry.VersionNotes = input.VersionNotes;

            foreach (var item in input.ExtraProperties)
            {
                entry.SetField(item.Key, item.Value);
            }

            //
            await _entryRepository.UpdateAsync(entry);
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
            var result = await _entryRepository.GetAllVisionListAsync(entry);

            return new ListResultDto<EntryDto>(
                ObjectMapper.Map<List<Entry>, List<EntryDto>>(result)
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Permissions.CmsAdminPermissions.Entry.Create)]
        public async Task ActivateAsync(Guid id)
        {
            var entry = await _entryRepository.GetAsync(id, false);
            if (!entry.IsActivatedVersion)
            {
                var result = await _entryRepository.GetAllVisionListAsync(entry);
                foreach (var item in result)
                {
                    if (item.IsActivatedVersion)
                    {
                        item.SetIsActivatedVersion(false);
                    }
                }
                entry.SetIsActivatedVersion(true);
            }
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
                await MoveAsync(entry, input.TargetId, input.Position);
            }
            else
            {
                throw new Volo.Abp.AbpException("Only entries in the structural section can be moved!");
            }
        }


        protected virtual async Task CheckSlugExistenceAsync(Guid sectionId,string culture, string slug)
        {
            if (await _entryRepository.SlugExistsAsync(sectionId,culture, slug))
            {
                throw new EntrySlugAlreadyExistException( culture, slug);
            }
        }

        protected virtual async Task MoveAsync(Entry entry, Guid targetId, MoveEntryPosition position)
        {
            var result = await _entryRepository.GetAllVisionListAsync(entry);
            var newOrder = 0;
            Guid? parentId = null;

            if (position == MoveEntryPosition.Inside)
            {
                newOrder = (await _entryRepository.GetMaxOrderAsync(entry.SectionId, entry.Culture, targetId)) + 1;
                parentId = targetId;
            }
            else if (position == MoveEntryPosition.Bottom)
            {
                var targetEntry = await _entryRepository.GetAsync(targetId, false);
                newOrder = (await _entryRepository.GetMaxOrderAsync(entry.SectionId, entry.Culture, targetEntry.ParentId)) + 1;
                parentId = targetEntry.ParentId;
            }
            else
            {
                throw new ArgumentException($"Parameter value '{position}' is invalid'", nameof(position));
            }
            foreach (var item in result)
            {
                item.SetOrder(newOrder);
                item.SetParentId(parentId);
            }

            await _entryRepository.UpdateManyAsync(result);
        }
    }
}
