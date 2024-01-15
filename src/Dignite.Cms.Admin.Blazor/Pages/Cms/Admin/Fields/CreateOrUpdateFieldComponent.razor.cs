using Blazorise;
using Dignite.Abp.DynamicForms.Components;
using Dignite.Cms.Admin.DynamicForms;
using Dignite.Cms.Admin.Fields;
using Dignite.Cms.Localization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Dignite.Cms.Admin.Blazor.Pages.Cms.Admin.Fields
{
    public partial class CreateOrUpdateFieldComponent
    {
        [Parameter] public CreateOrUpdateFieldInputBase Entity { get; set; }
        protected IReadOnlyList<FormControlDto> AllFormControls { get; set; } = new List<FormControlDto>();
        protected IReadOnlyList<FieldGroupDto> AllGroups { get; set; } = new List<FieldGroupDto>();

        protected Type FormConfigurationComponentType;
        protected Dictionary<string, object> FormConfigurationComponentParameters = new();

        public CreateOrUpdateFieldComponent()
        {
            LocalizationResource = typeof(CmsResource);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            AllFormControls = (await FormService.GetFormControlsAsync()).Items;
            AllGroups = (await FieldGroupService.GetListAsync(new GetFieldGroupsInput())).Items;
            await SetFormConfigurationComponentAsync();
        }

        protected async Task OnFormChangedAsync(string value)
        {
            Entity.FormControlName = value;
            await SetFormConfigurationComponentAsync();
        }

        private async Task SetFormConfigurationComponentAsync()
        {
            var configurationComponent = ConfigurationComponentSelector.Get(Entity.FormControlName);
            FormConfigurationComponentType = configurationComponent.GetType();
            FormConfigurationComponentParameters = new Dictionary<string, object>
            {
                { nameof(IFormConfigurationComponent.ConfigurationDictionary), Entity.FormConfiguration }
            };
            await Task.CompletedTask;
        }

        private async Task ValidateNameExistsAsync(ValidatorEventArgs e, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var fieldName = Convert.ToString(e.Value);
            if (!fieldName.IsNullOrEmpty())
            {
                if ((Entity.GetType() == typeof(CreateFieldInput)) ||
                    (Entity.GetType() == typeof(UpdateFieldInput) && !fieldName.Equals(Entity.Name, StringComparison.InvariantCultureIgnoreCase)))
                {
                    var field = await FieldService.FindByNameAsync(fieldName);
                    e.Status = field != null
                        ? ValidationStatus.Error
                        : ValidationStatus.Success;

                    e.ErrorText = L["FieldName{0}AlreadyExist", fieldName];
                }
            }
        }
    }
}
