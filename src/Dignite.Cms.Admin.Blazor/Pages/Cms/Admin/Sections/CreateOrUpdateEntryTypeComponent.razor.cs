using Blazorise;
using Dignite.Cms.Admin.Fields;
using Dignite.Cms.Admin.Sections;
using Dignite.Cms.Localization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Dignite.Cms.Admin.Blazor.Pages.Cms.Admin.Sections
{
    public partial class CreateOrUpdateEntryTypeComponent
    {
        [Parameter] public CreateOrUpdateEntryTypeInputBase Entity { get; set; }
        [Parameter] public Guid SectionId{ get; set; }


        /// <summary>
        /// 
        /// </summary>
        protected Guid? DraggingFieldId;

        protected IReadOnlyList<FieldGroupDto> FieldGroups { get; set; } = new List<FieldGroupDto>();
        protected IReadOnlyList<FieldDto> AllFields { get; set; }=new List<FieldDto>();

        //Will not change again after assignment, used to verify that the site name already exists
        private string entryTypeNameForValidation;

        public CreateOrUpdateEntryTypeComponent()
        {
            LocalizationResource = typeof(CmsResource);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            entryTypeNameForValidation = Entity.Name;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            FieldGroups = (await FieldGroupAppService.GetListAsync(new GetFieldGroupsInput())).Items;
            AllFields = (await FieldAppService.GetListAsync(new GetFieldsInput
            {
                MaxResultCount = 1000
            })).Items;
        }


        private async Task SectionFieldDropped(string fieldTabName)
        {
            var field = AllFields.First(f => f.Id == DraggingFieldId);

            //Check if from FieldTabs, if true then remove
            foreach (var tab in Entity.FieldTabs)
            {
                if (tab.Fields.Any(f => f.FieldId == DraggingFieldId))
                {
                    tab.Fields.RemoveAll(f => f.FieldId == DraggingFieldId);
                }
            }

            //Add to FieldTabs
            Entity.FieldTabs.First(ft => ft.Name == fieldTabName)
                .Fields.Add(
                new EntryFieldInput()
                {
                    FieldId = DraggingFieldId.Value,
                    DisplayName = field.DisplayName
                });

            await InvokeAsync(StateHasChanged);
        }
        private async Task FieldDropped()
        {
            foreach (var ft in Entity.FieldTabs)
            {
                ft.Fields.RemoveAll(ft => ft.FieldId == DraggingFieldId);
            }
            await InvokeAsync(StateHasChanged);
        }

        private async Task NameExistsValidatorAsync(ValidatorEventArgs e, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var name = Convert.ToString(e.Value);
            if (!name.IsNullOrEmpty())
            {
                if (!name.Equals(entryTypeNameForValidation, StringComparison.InvariantCultureIgnoreCase))
                {
                    e.Status = await EntryTypeAdminAppService.NameExistsAsync(new EntryTypeNameExistsInput(SectionId,name))
                        ? ValidationStatus.Error
                        : ValidationStatus.Success;

                    e.ErrorText = L["EntryTypeName{0}AlreadyExist", name];
                }
            }
            else
            {
                e.Status = ValidationStatus.Error;
            }
        }
    }
}
