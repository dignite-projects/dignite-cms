using AutoMapper;
using Dignite.Cms.Admin.Entries;
using Dignite.Cms.Admin.Fields;
using Dignite.Cms.Admin.Sections;
using Dignite.Cms.Admin.Sites;
using Dignite.Cms.Entries;
using Dignite.Cms.Sections;
using Dignite.Cms.Sites;
using Volo.Abp.AutoMapper;

namespace Dignite.Cms.Admin
{
    public class CmsAdminApplicationAutoMapperProfile : Profile
    {
        public CmsAdminApplicationAutoMapperProfile()
        {
            /**** site *****************************************/
            CreateMap<Site, SiteDto>();
            CreateMap<SiteLanguage, SiteLanguageDto>();

            /**** field *****************************************/
            CreateMap<Cms.Fields.FieldGroup, FieldGroupDto>();
            CreateMap<Cms.Fields.Field, FieldDto>();

            /**** section *****************************************/
            CreateMap<Section, SectionDto>();
            CreateMap<EntryPage, EntryPageDto>();
            CreateMap<EntryType, EntryTypeDto>();
            CreateMap<EntryFieldTab, EntryFieldTabDto>();
            CreateMap<EntryField, EntryFieldDto>()
                .Ignore(ef => ef.Field);


            /**** entity *****************************************/
            CreateMap<Entry, EntryDto>()
                .Ignore(u => u.Author)
                .MapCustomizeFields();
            CreateMap<EntryRevision, EntryRevisionDto>();

        }
    }
}