using Blazorise;
using Dignite.Cms.Admin.Entries;
using Dignite.Cms.Admin.Sections;
using Dignite.Cms.Localization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;

namespace Dignite.Cms.Admin.Blazor.Pages.Cms.Admin.Entries
{
    public partial class CreateEntry
    {
        [Parameter]
        [SupplyParameterFromQuery]
        public Guid SectionId { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public string Region { get; set; }
        protected CreateEntryInput NewEntity { get; set; }
        protected SectionDto Section { get; set; }
        protected PageToolbar Toolbar { get; } = new();

        protected Validations CreateValidationsRef;

        public CreateEntry()
        {
            ObjectMapperContext = typeof(CmsAdminBlazorModule);
            LocalizationResource = typeof(CmsResource);
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Section = await SectionAppService.GetAsync(SectionId);
            Region = Region.IsNullOrEmpty() ? Section.Site.Regions.OrderByDescending(l => l.IsDefault).First().Region : Region;

            if (!Section.EntryTypes.Any())
            {
                throw new AbpException(L["{0}SectionHasNotCreatedAnEntryType", Section.DisplayName]);
            }

            NewEntity = new CreateEntryInput(SectionId)
            {
                Slug = Nanoid.Nanoid.Generate(size: 8),
                PublishTime = Clock.Now,
                EntryTypeId = Section.EntryTypes.First().Id,
                Region = Region,
            };
        }


        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await SetToolbarItemsAsync();
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        protected async ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["Save"],
                SaveAsync,
                IconName.Save);
            await InvokeAsync(StateHasChanged);
        }

        protected async Task SaveAsync()
        {
            try
            {
                var validate = true;
                if (CreateValidationsRef != null)
                {
                    validate = await CreateValidationsRef.ValidateAll();
                }
                if (validate)
                {
                    await EntryAppService.CreateAsync(NewEntity);
                    Navigation.NavigateTo($"/cms/admin/entries?sectionId={SectionId}&region={NewEntity.Region}");
                }
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
        }
    }
}
