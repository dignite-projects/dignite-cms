using Dignite.Abp.DynamicForms;
using Dignite.Cms.Admin.Entries;
using Dignite.Cms.Admin.Sections;
using Dignite.Cms.Localization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Localization;
using Dignite.Abp.Data;

namespace Dignite.Cms.Admin.Blazor.Pages.Cms.Admin.Entries
{

    public partial class CreateOrUpdateEntryComponent
    {
        [Parameter] public CreateOrUpdateEntryInputBase Entry { get; set; }
        [Parameter] public SectionDto Section { get; set; }

        protected EntryTypeDto CurrentEntryType { get; set; }
        protected IReadOnlyList<LanguageInfo> AllLanguages = new List<LanguageInfo>();
        protected IReadOnlyList<EntryDto> AllEntriesOfStructure;
        public CreateOrUpdateEntryComponent()
        {
            LocalizationResource = typeof(CmsResource);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await SetEntryType(Entry.EntryTypeId);
            AllLanguages = await LanguageProvider.GetLanguagesAsync();
            await SetCultureAsync(Entry.Culture);
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

        protected async Task OnCultureSelectedValueChanged(string value)
        { 
            await SetCultureAsync(value);
        }

        protected async Task SetCultureAsync(string culture)
        { 
            Entry.Culture = culture;
            if (Section.Type == Dignite.Cms.Sections.SectionType.Structure && Entry.GetType() == typeof(CreateEntryInput))
            {
                AllEntriesOfStructure =( await AppService.GetListAsync(new GetEntriesInput { 
                    SectionId=Section.Id,
                    Culture= culture,
                    MaxResultCount=1000
                })).Items;
            }
        }
        private void OnFieldValueChanged(FormField field)
        { 
            Entry.SetField(field.Name, field.Value);
        }
        void TitleTextboxBlur()
        {
            if (!Entry.Title.IsNullOrEmpty() && Entry.Slug.IsNullOrEmpty())
            {
                Entry.Slug = SlugNormalizer.Normalize(Entry.Title);
            }
        }
    }
}
