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
using System.Threading;
using Blazorise;

namespace Dignite.Cms.Admin.Blazor.Pages.Cms.Admin.Entries
{

    public partial class CreateOrUpdateEntryComponent
    {
        [Parameter] public CreateOrUpdateEntryInputBase Entry { get; set; }
        [Parameter] public SectionDto Section { get; set; }

        protected EntryTypeDto CurrentEntryType { get; set; }
        protected IReadOnlyList<LanguageInfo> AllLanguages = new List<LanguageInfo>();
        protected IReadOnlyList<EntryDto> AllEntriesOfStructure;

        //Will not change again after assignment, used to verify that the slug already exists
        private string slugForValidation;
        private string cultureForValidation;
        public CreateOrUpdateEntryComponent()
        {
            LocalizationResource = typeof(CmsResource);
        }
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            slugForValidation = Entry.Slug;
            cultureForValidation = Entry.Culture;
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
        private void TitleTextboxBlur()
        {
            if (!Entry.Title.IsNullOrEmpty() && Entry.Slug.IsNullOrEmpty())
            {
                Entry.Slug = SlugNormalizer.Normalize(Entry.Title);
            }
        }

        private async Task SlugExistsValidatorAsync(ValidatorEventArgs e, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var slug = Convert.ToString(e.Value);
            if (!slug.IsNullOrEmpty())
            {
                if (!slug.Equals(slugForValidation, StringComparison.InvariantCultureIgnoreCase))
                {
                    await ValidateNameExistsAsync(e, cancellationToken);
                }
            }
            else
            {
                e.Status = ValidationStatus.Error;
            }
        }

        private async Task CultureExistsValidatorAsync(ValidatorEventArgs e, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var culture = Convert.ToString(e.Value);
            if (!culture.IsNullOrEmpty())
            {
                if (!culture.Equals(cultureForValidation, StringComparison.InvariantCultureIgnoreCase))
                {
                    await ValidateNameExistsAsync(e,cancellationToken); 
                }
            }
            else
            {
                e.Status = ValidationStatus.Error;
            }
        }

        private async Task ValidateNameExistsAsync(ValidatorEventArgs e, CancellationToken cancellationToken)
        {
            e.Status = await AppService.SlugExistsAsync(Section.Id, Entry.Culture, Entry.Slug)
                ? ValidationStatus.Error
                : ValidationStatus.Success;

            e.ErrorText = L["EntrySlug{0}AlreadyExist", Entry.Slug];
        }
    }
}
