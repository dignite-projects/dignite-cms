using AutoMapper;
using Dignite.Cms.Admin.Entries;
using Dignite.Cms.Admin.Fields;
using Dignite.Cms.Admin.Sections;
using Dignite.Cms.Admin.Sites;
using Dignite.Cms.Sections;
using Dignite.Cms.Sites;
using Volo.Abp.AutoMapper;

namespace Dignite.Cms.Admin.Blazor
{
    public class CmsAdminBlazorAutoMapperProfile : Profile
    {
        public CmsAdminBlazorAutoMapperProfile()
        {
            CreateMap<FieldGroupDto, CreateOrUpdateFieldGroupInput>()
                .MapExtraProperties();
            CreateMap<FieldDto, UpdateFieldInput>()
                .MapExtraProperties();
            CreateMap<SiteDto, UpdateSiteInput>()
                .MapExtraProperties();
            CreateMap<SiteLanguageDto, CreateOrUpdateLanguageInput>(); 
            CreateMap<SectionDto, UpdateSectionInput>()
                .MapExtraProperties();
            CreateMap<EntryTypeDto, UpdateEntryTypeInput>()
                .MapExtraProperties();
            CreateMap<EntryFieldTabDto, EntryFieldTabInput>();
            CreateMap<EntryFieldDto, EntryFieldInput>();
            CreateMap<EntryDto, UpdateEntryInput>()
                .Ignore(u => u.CustomizedFieldFiles);
            CreateMap<EntryPageDto, EntryPageInput>();
        }
    }
}