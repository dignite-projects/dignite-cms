using AutoMapper;
using Dignite.Cms.Admin.Entries;
using Dignite.Cms.Admin.Fields;
using Dignite.Cms.Admin.Sections;
using Dignite.Cms.Admin.Sites;
using Dignite.Cms.Sections;
using Dignite.Cms.Sites;

namespace Dignite.Cms.Admin.Blazor
{
    public class CmsAdminBlazorAutoMapperProfile : Profile
    {
        public CmsAdminBlazorAutoMapperProfile()
        {
            CreateMap<FieldGroupDto, CreateOrUpdateFieldGroupInput>();
            CreateMap<FieldDto, UpdateFieldInput>();
            CreateMap<SiteDto, UpdateSiteInput>()
                .MapExtraProperties();
            CreateMap<SiteCultureDto, CreateOrUpdateCultureInput>(); 
            CreateMap<SectionDto, UpdateSectionInput>()
                .MapExtraProperties();
            CreateMap<EntryTypeDto, UpdateEntryTypeInput>();
            CreateMap<EntryFieldTabDto, EntryFieldTabInput>();
            CreateMap<EntryFieldDto, EntryFieldInput>();
            CreateMap<EntryDto, CreateEntryInput>();
            CreateMap<EntryDto, UpdateEntryInput>();
        }
    }
}