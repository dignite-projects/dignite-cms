﻿using AutoMapper;
using Dignite.Cms.Admin.Entries;
using Dignite.Cms.Admin.Fields;
using Dignite.Cms.Admin.Sections;
using Dignite.Cms.Admin.Sites;
using Dignite.Cms.Admin.Domains;
using Dignite.Cms.Domains;
using Dignite.Cms.Entries;
using Dignite.Cms.Sections;
using Dignite.Cms.Sites;
using Volo.Abp.AutoMapper;
using Volo.Abp.ObjectExtending;

namespace Dignite.Cms.Admin
{
    public class CmsAdminApplicationAutoMapperProfile : Profile
    {
        public CmsAdminApplicationAutoMapperProfile()
        {
            /**** domain *****************************************/
            CreateMap<Domain, DomainDto>();

            /**** site *****************************************/
            CreateMap<Site, SiteDto>();
            CreateMap<SiteLanguage, SiteLanguageDto>();

            /**** field *****************************************/
            CreateMap<Cms.Fields.FieldGroup, FieldGroupDto>();
            CreateMap<Cms.Fields.Field, Cms.Fields.FieldDto>();
            CreateMap<Cms.Fields.Field, FieldDto>()
                .Ignore(f=>f.GroupName);

            /**** section *****************************************/
            CreateMap<Section, SectionDto>();
            CreateMap<EntryType, EntryTypeDto>();
            CreateMap<EntryFieldTab, EntryFieldTabDto>();
            CreateMap<EntryField, EntryFieldDto>()
                .Ignore(ef => ef.Field);


            /**** entity *****************************************/
            CreateMap<Entry, EntryDto>()
                .Ignore(u => u.Author)
                .MapExtraProperties(MappingPropertyDefinitionChecks.None);

        }
    }
}