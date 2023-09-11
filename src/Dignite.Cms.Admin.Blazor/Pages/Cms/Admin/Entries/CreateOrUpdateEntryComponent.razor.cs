using Dignite.Cms.Admin.Entries;
using Dignite.Cms.Admin.Sections;
using Dignite.Cms.Localization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace Dignite.Cms.Admin.Blazor.Pages.Cms.Admin.Entries
{

    public partial class CreateOrUpdateEntryComponent
    {
        [Parameter] public CreateOrUpdateEntryInputBase Entry { get; set; }
        [Parameter] public SectionDto Section { get; set; }

        protected EntryTypeDto CurrentEntryType { get; set; }
        protected IReadOnlyList<LanguageInfo> AllRegions = new List<LanguageInfo>();
        protected IReadOnlyList<EntryDto> AllEntriesOfStructure;
        public CreateOrUpdateEntryComponent()
        {
            LocalizationResource = typeof(CmsResource);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await SetEntryType(Entry.EntryTypeId);
            AllRegions = await LanguageProvider.GetLanguagesAsync();
            await SetRegionAsync(Entry.Region);
        }
        protected async Task OnEntryTypeSelectedValueChanged(Guid value)
        {
            Entry.EntryTypeId = value;
            await SetEntryType(value);
        }

        protected Task SetEntryType(Guid entryTypeId)
        { 
            CurrentEntryType = Section.EntryTypes.FirstOrDefault(et => et.Id == entryTypeId);
            return Task.CompletedTask;
        }

        protected async Task OnRegionSelectedValueChanged(string value)
        { 
            await SetRegionAsync(value);
        }

        protected async Task SetRegionAsync(string region)
        { 
            Entry.Region = region;
            if (Section.Type == Dignite.Cms.Sections.SectionType.Structure && Entry.GetType() == typeof(CreateEntryInput))
            {
                AllEntriesOfStructure =( await AppService.GetListAsync(new GetEntriesInput { 
                    SectionId=Section.Id,
                    Region= region,
                    MaxResultCount=1000
                })).Items;
            }
        }
    }
}
