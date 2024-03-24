using Dignite.Abp.Data;
using Dignite.Abp.DynamicForms.Select;
using Dignite.Abp.DynamicForms.TextEdit;
using Dignite.Cms.Entries;
using Dignite.Cms.Fields;
using Dignite.Cms.Sections;
using Dignite.Cms.Sites;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Timing;

namespace Dignite.Cms;

public class CmsDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IClock _clock;
    private readonly ICurrentTenant _currentTenant;
    private readonly CmsDataSeedData _cmsData;
    private readonly ISiteRepository _siteRepository;
    private readonly ISectionRepository _sectionRepository;
    private readonly IEntryTypeRepository _entryTypeRepository;
    private readonly IFieldGroupRepository _fieldGroupRepository;
    private readonly IFieldRepository _fieldRepository;
    private readonly IEntryRepository _entryRepository;

    public CmsDataSeedContributor(IClock clock, ICurrentTenant currentTenant, CmsDataSeedData cmsData, 
        ISiteRepository siteRepository, ISectionRepository sectionRepository, IEntryTypeRepository entryTypeRepository, 
        IFieldGroupRepository fieldGroupRepository, IFieldRepository fieldRepository, IEntryRepository entryRepository)
    {
        _clock = clock;
        _currentTenant = currentTenant;
        _cmsData = cmsData;
        _siteRepository = siteRepository;
        _sectionRepository = sectionRepository;
        _entryTypeRepository = entryTypeRepository;
        _fieldGroupRepository = fieldGroupRepository;
        _fieldRepository = fieldRepository;
        _entryRepository = entryRepository;
    }

    public async Task SeedAsync(DataSeedContext context)
    {
        using (_currentTenant.Change(context?.TenantId))
        {
            await SeedFieldGroupAsync();
            await SeedFieldsAsync();
            await SeedSitesAsync();
            await SeedSectionsAsync();
            await SeedEntryTypesAsync();
            await SeedEntriesAsync();
        }
    }


    private async Task SeedFieldGroupAsync()
    { 
        await _fieldGroupRepository.InsertAsync(new FieldGroup(_cmsData.FieldGroupId, "FieldGroup", null), autoSave: true);
    }

    private async Task SeedFieldsAsync()
    {
        var textboxFormConfiguration = new TextEditConfiguration();
        textboxFormConfiguration.CharLimit = 256;
        textboxFormConfiguration.Mode = TextEditMode.SingleLine;
        await _fieldRepository.InsertAsync(
            new Field(
                _cmsData.TextboxFieldId,
                _cmsData.FieldGroupId,
                _cmsData.TextboxFieldName,
                "Textbox Field","", 
                TextEditFormControl.ControlName,
                textboxFormConfiguration.ConfigurationDictionary,
                null),
            autoSave: true);

        var selectFormConfiguration = new SelectConfiguration();
        selectFormConfiguration.Multiple = false;
        selectFormConfiguration.Options = new List<SelectListItem> {
            new SelectListItem("Item 1",_cmsData.SelectFieldItem1Value,true),
            new SelectListItem("Item 2",_cmsData.SelectFieldItem2Value,false),
            new SelectListItem("Item 2",_cmsData.SelectFieldItem3Value,false)
        };
        await _fieldRepository.InsertAsync(
            new Field(
                _cmsData.SelectFieldId,
                _cmsData.FieldGroupId,
                _cmsData.SelectFieldName,
                "Select Field","",
                SelectFormControl.ControlName,
                selectFormConfiguration.ConfigurationDictionary,
                null
                ),
            autoSave:true
            );
    }

    private async Task SeedSitesAsync()
    {
        var site = new Site(
                _cmsData.SiteId,
                "Site",
                _cmsData.SiteName,
                _cmsData.SiteHost,
                true, null);
        site.AddLanguage(new SiteLanguage(true, "en"));
        site.AddLanguage(new SiteLanguage(false, "ja"));
        site.AddLanguage(new SiteLanguage(false, "zh-Hant"));

        await _siteRepository.InsertAsync(
            site,
            autoSave: true);
    }
    private async Task SeedSectionsAsync()
    {
        await _sectionRepository.InsertAsync(
            new Section(
                _cmsData.SingleSectionId,
                _cmsData.SiteId, 
                SectionType.Single, 
                "Single Section", 
                _cmsData.SingleSectionName,
                true,
                true,
                "/","home", null),
            autoSave: true);

        await _sectionRepository.InsertAsync(
            new Section(
                _cmsData.ChannelSectionId,
                _cmsData.SiteId,
                SectionType.Channel,
                "Channel Section",
                _cmsData.ChannelSectionName,
                false,
                true,
                _cmsData.ChannelSectionRoute,
                "blog/entry",null),
            autoSave: true);

        await _sectionRepository.InsertAsync(
            new Section(
                _cmsData.StructureSectionId,
                _cmsData.SiteId,
                SectionType.Structure,
                "Channel Section",
                _cmsData.StructureSectionName,
                false,
                true,
                _cmsData.StructureSectionRoute,
                "blog/entry", null),
            autoSave: true);
    }
    private async Task SeedEntryTypesAsync()
    {
        await _entryTypeRepository.InsertAsync(
            new EntryType(
                _cmsData.SingleSectionEntryTypeId,
                _cmsData.SingleSectionId,
                "Single Section Entry Type",
                _cmsData.SingleSectionEntryTypeName,
                new List<EntryFieldTab> { 
                    new EntryFieldTab(
                        "Entry Field Tab", 
                        new List<EntryField>{ 
                            new EntryField(
                                _cmsData.TextboxFieldId,
                                "Textbox Field",
                                true,
                                true
                                ),
                            new EntryField(
                                _cmsData.SelectFieldId,
                                "Select Field",
                                true,
                                true
                                )
                        })
                    },
                null),
            autoSave: true);

        await _entryTypeRepository.InsertAsync(
            new EntryType(
                _cmsData.ChannelSectionEntryTypeId,
                _cmsData.ChannelSectionId,
                "Channel Section Entry Type",
                _cmsData.ChannelSectionEntryTypeName,
                new List<EntryFieldTab> {
                    new EntryFieldTab(
                        "Entry Field Tab",
                        new List<EntryField>{
                            new EntryField(
                                _cmsData.TextboxFieldId,
                                "Author",
                                true,
                                true
                                ),
                            new EntryField(
                                _cmsData.SelectFieldId,
                                "Select Field",
                                true,
                                true
                                )
                        })
                    },
                null),
            autoSave: true);


        await _entryTypeRepository.InsertAsync(
            new EntryType(
                _cmsData.StructureSectionEntryTypeId,
                _cmsData.StructureSectionId,
                "Structure Section Entry Type",
                _cmsData.StructureSectionEntryTypeName,
                new List<EntryFieldTab> {
                    new EntryFieldTab(
                        "Entry Field Tab",
                        new List<EntryField>{
                            new EntryField(
                                _cmsData.TextboxFieldId,
                                "Author",
                                true,
                                true
                                ),
                            new EntryField(
                                _cmsData.SelectFieldId,
                                "Select Field",
                                true,
                                true
                                )
                        })
                    },
                null),
            autoSave: true);
    }
    private async Task SeedEntriesAsync()
    {
        var singleSection_Entry = new Entry(
                _cmsData.SingleSection_EntryId,
                _cmsData.SingleSectionId,
                _cmsData.SingleSectionEntryTypeId,
                _cmsData.EntryDefaultCulture,
                "Home Page",
                _cmsData.SingleSection_EntrySlug,
                _clock.Now,
                EntryStatus.Published,
                null,
                1,
                null,
                "",
                null
                );
        singleSection_Entry.SetField(_cmsData.TextboxFieldName, "An excellent program.");
        singleSection_Entry.SetField(
            _cmsData.SelectFieldName, 
            new List<string> 
            { 
                _cmsData.SelectFieldItem1Value 
            });

        await _entryRepository.InsertAsync(
            singleSection_Entry,
            autoSave:true
            );


        var channelSection_Entry1 = new Entry(
                _cmsData.ChannelSection_Entry1Id,
                _cmsData.ChannelSectionId,
                _cmsData.ChannelSectionEntryTypeId,
                _cmsData.EntryDefaultCulture,
                "A blog post",
                _cmsData.ChannelSection_Entry1Slug,
                _clock.Now,
                EntryStatus.Published,
                null,
                1,
                null,
                "",
                null
                );
        channelSection_Entry1.SetField(_cmsData.TextboxFieldName, "Tanaka");
        channelSection_Entry1.SetField(
            _cmsData.SelectFieldName,
            new List<string>
            {
                _cmsData.SelectFieldItem2Value,
                _cmsData.SelectFieldItem3Value
            });

        await _entryRepository.InsertAsync(
            channelSection_Entry1,
            autoSave: true
            );




        var channelSection_Entry2 = new Entry(
                _cmsData.ChannelSection_Entry2Id,
                _cmsData.ChannelSectionId,
                _cmsData.ChannelSectionEntryTypeId,
                _cmsData.EntryDefaultCulture,
                "The second blog post",
                _cmsData.ChannelSection_Entry2Slug,
                _clock.Now.AddSeconds(1),
                EntryStatus.Published,
                null,
                2,
                null,
                "",
                null
                );
        channelSection_Entry2.SetField(_cmsData.TextboxFieldName, "Tanaka");
        channelSection_Entry2.SetField(
            _cmsData.SelectFieldName,
            new List<string>
            {
                _cmsData.SelectFieldItem1Value,
                _cmsData.SelectFieldItem2Value,
                _cmsData.SelectFieldItem3Value
            });

        await _entryRepository.InsertAsync(
            channelSection_Entry2,
            autoSave: true
            );

        var channelSection_Entry3VisionEntry = new Entry(
                _cmsData.ChannelSection_Entry2VisionEntryId,
                _cmsData.ChannelSectionId,
                _cmsData.ChannelSectionEntryTypeId,
                _cmsData.EntryDefaultCulture,
                "The second blog post",
                _cmsData.ChannelSection_Entry2Slug,
                _clock.Now.AddSeconds(1),
                EntryStatus.Published,
                null,
                2,
                _cmsData.ChannelSection_Entry2Id,
                "",
                null
                );
        channelSection_Entry3VisionEntry.SetField(_cmsData.TextboxFieldName, "Tanaka");
        channelSection_Entry3VisionEntry.SetField(
            _cmsData.SelectFieldName,
            new List<string>
            {
                _cmsData.SelectFieldItem1Value,
                _cmsData.SelectFieldItem2Value,
                _cmsData.SelectFieldItem3Value
            });
        channelSection_Entry3VisionEntry.SetIsActivatedVersion(false);
        await _entryRepository.InsertAsync(
            channelSection_Entry3VisionEntry,
            autoSave: true
            );



        var structureSection_Entry1 = new Entry(
                _cmsData.StructureSection_Entry1Id,
                _cmsData.StructureSectionId,
                _cmsData.StructureSectionEntryTypeId,
                _cmsData.EntryDefaultCulture,
                "A document",
                _cmsData.StructureSection_Entry1Slug,
                _clock.Now,
                EntryStatus.Published,
                null,
                1,
                null,
                "",
                null
                );
        structureSection_Entry1.SetField(_cmsData.TextboxFieldName, "Tanaka");
        structureSection_Entry1.SetField(
            _cmsData.SelectFieldName,
            new List<string>
            {
                _cmsData.SelectFieldItem2Value,
                _cmsData.SelectFieldItem3Value
            });

        await _entryRepository.InsertAsync(
            structureSection_Entry1,
            autoSave: true
            );


        var structureSection_Entry2 = new Entry(
                _cmsData.StructureSection_Entry2Id,
                _cmsData.StructureSectionId,
                _cmsData.StructureSectionEntryTypeId,
                _cmsData.EntryDefaultCulture,
                "A document",
                _cmsData.StructureSection_Entry2Slug,
                _clock.Now,
                EntryStatus.Published,
                null,
                1,
                null,
                "",
                null
                );
        structureSection_Entry2.SetField(_cmsData.TextboxFieldName, "Tanaka");
        structureSection_Entry2.SetField(
            _cmsData.SelectFieldName,
            new List<string>
            {
                _cmsData.SelectFieldItem2Value,
                _cmsData.SelectFieldItem3Value
            });

        await _entryRepository.InsertAsync(
            structureSection_Entry2,
            autoSave: true
            );
    }
}
