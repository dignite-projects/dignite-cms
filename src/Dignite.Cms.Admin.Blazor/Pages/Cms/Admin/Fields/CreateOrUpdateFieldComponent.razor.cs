using Dignite.Cms.Admin.Fields;
using Dignite.Cms.Localization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dignite.Cms.Admin.Blazor.Pages.Cms.Admin.Fields
{
    public partial class CreateOrUpdateFieldComponent
    {
        [Parameter] public CreateOrUpdateFieldInputBase Entity { get; set; }
        protected IReadOnlyList<FormDto> AllForms { get; set; } = new List<FormDto>();
        protected IReadOnlyList<FieldGroupDto> AllGroups { get; set; } = new List<FieldGroupDto>();

        protected Type FieldFormConfigurationComponentType;
        protected Dictionary<string, object> FieldFormConfigurationComponentParameters = new();

        public CreateOrUpdateFieldComponent()
        {
            LocalizationResource = typeof(CmsResource);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            AllForms = (await FieldService.GetFormsAsync()).Items;
            AllGroups = (await FieldGroupService.GetListAsync(new GetFieldGroupsInput())).Items;
            await SetFieldFormConfigurationComponentAsync();
        }

        protected async Task OnFormChangedAsync(string formName)
        {
            Entity.FormName = formName;
            await SetFieldFormConfigurationComponentAsync();
        }

        private async Task SetFieldFormConfigurationComponentAsync()
        {
            var configurationComponent = ConfigurationComponentSelector.Get(Entity.FormName);
            FieldFormConfigurationComponentType = configurationComponent.GetType();
            FieldFormConfigurationComponentParameters = new Dictionary<string, object>
            {
                { "Field", Entity }
            };
            await Task.CompletedTask;
        }
    }
}
