using Dignite.Abp.DynamicForms.NumericEdit;
using Dignite.Abp.FieldCustomizing;
using Dignite.Cms.Entries;
using Dignite.Cms.Sections;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

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
            await CheckSlugExistenceAsync(input.SectionId,input.Language, input.Slug);

            var id = GuidGenerator.Create();
            var revision = await CreateRevisionVersionAsync(
                id,
                input.InitialId.HasValue ? input.InitialId.Value : id,
                input.RevisionNotes
                );
            var order = (await _entryRepository.GetMaxOrderAsync(input.SectionId,input.Language, input.ParentId))+1;
            var entry = new Entry(
                id, 
                input.SectionId, 
                input.EntryTypeId,
                input.Language,
                input.Title, 
                input.Slug, 
                input.PublishTime,
                input.Draft? EntryStatus.Draft: EntryStatus.Published,
                input.CustomFields,
                input.ParentId,
                order,
                revision,
                CurrentTenant.Id);

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
            var section = await _sectionRepository.GetAsync(entry.SectionId, true);

            if (input.EntryTypeId != entry.EntryTypeId)
            {
                CheckEntryType(section, input.EntryTypeId);
            }

            if (!input.Language.Equals(entry.Language, StringComparison.OrdinalIgnoreCase) || 
                !input.Slug.Equals(entry.Slug,StringComparison.OrdinalIgnoreCase))
            {
                await CheckSlugExistenceAsync(entry.SectionId,input.Language, input.Slug);
            }

            //
            entry.SetEntryTypeId( input.EntryTypeId);
            entry.Title=input.Title;
            entry.Slug = input.Slug;
            entry.PublishTime = input.PublishTime;
            entry.Language = input.Language;
            entry.CustomFields = input.CustomFields;
            entry.Revision.Notes = input.RevisionNotes;

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
            var revisionList = await _entryRepository.GetRevisionListAsync(entry.Revision.InitialId); //get all versions 

            await _entryRepository.DeleteManyAsync(revisionList);
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


            IList<QueryingByFieldParameter> queryingByFieldParameters = new List<QueryingByFieldParameter>
            {
                //new QueryingByFieldParameter("shuzi","5-25")
            };

            var count = await _entryRepository.GetCountAsync(input.SectionId, input.Language, input.CreatorId, input.Status, input.Filter,input.StartPublishDate,input.ExpiryPublishDate, queryingByFieldParameters);
            if (count == 0)
                return new PagedResultDto<EntryDto>(0, new List<EntryDto>());

            //get entry list
            var result = await _entryRepository.GetListAsync(input.SectionId, input.Language, input.CreatorId, input.Status, input.Filter, input.StartPublishDate, input.ExpiryPublishDate, queryingByFieldParameters, input.MaxResultCount, input.SkipCount, input.Sorting);
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
        public async Task<ListResultDto<EntryDto>> GetVersionListAsync(Guid id)
        {
            var entry = await _entryRepository.GetAsync(id, false);
            var result = await _entryRepository.GetRevisionListAsync(entry.Revision.InitialId);

            return new ListResultDto<EntryDto>(
                ObjectMapper.Map<List<Entry>, List<EntryDto>>(result)
                );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Authorize(Permissions.CmsAdminPermissions.Entry.Create)]
        public async Task ActivateAsync(Guid id, int version)
        {
            var entry = await _entryRepository.GetAsync(id, false);
            if (!entry.Revision.IsActive)
            {
                var result = await _entryRepository.GetRevisionListAsync(entry.Revision.InitialId);
                foreach (var item in result)
                {
                    if (item.Revision.IsActive)
                    {
                        item.Revision.SetIsActive(false);
                    }
                }
                entry.Revision.SetIsActive(true);
                entry.SetOrder(result.First(e=>e.Revision.IsActive).Order);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        [Authorize(Permissions.CmsAdminPermissions.Entry.Default)]
        public async Task<EntryDto> GetByVersion(Guid id, int version)
        {
            var entry = await _entryRepository.GetAsync(id, false);
            var result = await _entryRepository.FindByVersionAsync(entry.Revision.InitialId, version);
            return ObjectMapper.Map<Entry, EntryDto>(
                result
                );
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

        protected virtual async Task<EntryRevision> CreateRevisionVersionAsync(Guid newEntityId, Guid initialId, string notes)
        {
            var revisionList = await _entryRepository.GetRevisionListAsync(initialId);
            if (revisionList.Any())
            {
                var activeVersion = revisionList.First(r => r.Revision.IsActive);
                //A new revision is created, deactivating the previous active revision
                activeVersion.Revision.SetIsActive(false);
                await _entryRepository.UpdateAsync(activeVersion);
            }
            return new EntryRevision(
                initialId,
                revisionList.Select(r => r.Revision.Version).DefaultIfEmpty(0).Max() + 1,
                true,
                notes
                );
        }


        protected virtual async Task CheckSlugExistenceAsync(Guid sectionId,string language, string slug)
        {
            if (await _entryRepository.SlugExistsAsync(sectionId,language, slug))
            {
                throw new EntrySlugAlreadyExistException( language, slug);
            }
        }
        protected virtual void CheckEntryType(Section section, Guid entryTypeId)
        {
            if (!section.EntryTypes.Any(et=>et.Id==entryTypeId))
            {
                throw new Volo.Abp.AbpException($"id:{entryTypeId} entry type does not exist.");
            }
        }

        protected virtual async Task MoveAsync(Entry entry, Guid targetId, MoveEntryPosition position)
        {
            var result = await _entryRepository.GetRevisionListAsync(entry.Revision.InitialId);
            var newOrder = 0;
            Guid? parentId = null;

            if (position == MoveEntryPosition.Inside)
            {
                newOrder = (await _entryRepository.GetMaxOrderAsync(entry.SectionId, entry.Language, targetId)) + 1;
                parentId = targetId;
            }
            else if (position == MoveEntryPosition.Bottom)
            {
                var targetEntry = await _entryRepository.GetAsync(targetId, false);
                newOrder = (await _entryRepository.GetMaxOrderAsync(entry.SectionId, entry.Language, targetEntry.ParentId)) + 1;
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
