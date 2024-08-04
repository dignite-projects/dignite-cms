using AutoMapper;
using Dignite.Cms.Entries;
using Dignite.Cms.Fields;
using Dignite.Cms.Domains;
using Dignite.Cms.Public.Entries;
using Dignite.Cms.Public.Sections;
using Dignite.Cms.Public.Sites;
using Dignite.Cms.Public.Domains;
using Dignite.Cms.Sections;
using Dignite.Cms.Sites;
using Volo.Abp.AutoMapper;
using Volo.Abp.ObjectExtending;

namespace Dignite.Cms.Public;

public class CmsPublicApplicationAutoMapperProfile : Profile
{
    public CmsPublicApplicationAutoMapperProfile()
    {
        /**** domain *****************************************/
        CreateMap<Domain, DomainDto>();

        /**** site *****************************************/
        CreateMap<Site, SiteDto>();
        CreateMap<SiteLanguage, SiteLanguageDto>();

        /**** field *****************************************/
        CreateMap<Field, FieldDto>();

        /**** section *****************************************/
        CreateMap<Section, SectionDto>();
        CreateMap<EntryType, EntryTypeDto>();
        CreateMap<EntryFieldTab, EntryFieldTabDto>();
        CreateMap<EntryField, EntryFieldDto>()
                .Ignore(ef => ef.Field);


        /**** entity *****************************************/
        CreateMap<Entry, EntryDto>()
            .Ignore(e => e.Author)
            .MapExtraProperties(MappingPropertyDefinitionChecks.None);
    }
}
