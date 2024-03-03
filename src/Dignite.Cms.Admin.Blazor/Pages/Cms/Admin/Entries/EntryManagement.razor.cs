using Dignite.Cms.Admin.Entries;
using Dignite.Cms.Admin.Sections;
using Dignite.Cms.Admin.Sites;
using Dignite.Cms.Localization;
using Dignite.Cms.Permissions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.Localization;
using Dignite.Cms.Entries;

namespace Dignite.Cms.Admin.Blazor.Pages.Cms.Admin.Entries
{
    public partial class EntryManagement
    {
        [Parameter]
        [SupplyParameterFromQuery]
        public Guid? SectionId { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public string CultureName { get; set; }

        protected PageToolbar Toolbar { get; private set; } = new();
        protected List<TableColumn> EntryManagementTableColumns => TableColumns.Get<EntryManagement>();

        protected IReadOnlyList<LanguageInfo> AllLanguages = new List<LanguageInfo>();
        protected IReadOnlyList<SiteDto> AllSites { get; set; }=new List<SiteDto>();
        protected IReadOnlyList<SectionDto> Sections { get; set; }=new List<SectionDto>();
        protected SiteDto CurrentSite { get; set; } = new();
        protected SectionDto CurrentSection { get; set; }

        public EntryManagement()
        {
            ObjectMapperContext = typeof(CmsAdminBlazorModule);
            LocalizationResource = typeof(CmsResource);

            CreatePolicyName = CmsAdminPermissions.Entry.Create;
            UpdatePolicyName = CmsAdminPermissions.Entry.Update;
            DeletePolicyName = CmsAdminPermissions.Entry.Delete;
        }

        protected override async Task OnInitializedAsync()
        {
            await InitializePageDataAsync();
            await base.OnInitializedAsync();
        }


        protected override async ValueTask SetToolbarItemsAsync()
        {
            Toolbar = new();
            if (CurrentSection != null)
            {
                Toolbar.AddComponent<NewEntryButtonComponent>(
                    new Dictionary<string, object>
                    {
                        { nameof(NewEntryButtonComponent.ButtonText), L["New"]},
                        { nameof(NewEntryButtonComponent.CultureName), CultureName},
                        { nameof(NewEntryButtonComponent.Section), CurrentSection},
                    },
                    requiredPolicyName: CreatePolicyName
                    );
            }
            await base.SetToolbarItemsAsync();
        }

        protected override ValueTask SetEntityActionsAsync()
        {
            EntityActions
                .Get<EntryManagement>()
                .AddRange(new EntityAction[]
                {
                    new EntityAction
                    {
                        Text = L["Edit"],
                        Visible = (data) => HasUpdatePermission,
                        Clicked = async (data) => {
                            Navigation.NavigateTo($"cms/admin/entries/{data.As<EntryDto>().Id}/edit");
                            await Task.CompletedTask;
                        }
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data) => HasDeletePermission,
                        Clicked = async (data) => await DeleteEntityAsync(data.As<EntryDto>()),
                        ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<EntryDto>())
                    }
                });

            return base.SetEntityActionsAsync();
        }

        protected override ValueTask SetTableColumnsAsync()
        {
            EntryManagementTableColumns
                .AddRange(new TableColumn[]
                {
                    new TableColumn
                    {
                        Title = L["EntryType"],
                        Sortable = true,
                        ValueConverter=(data)=> CurrentSection.EntryTypes.FirstOrDefault(et=>et.Id==((EntryDto)data).EntryTypeId)?.DisplayName,
                        Data = nameof(EntryDto.EntryTypeId)
                    },
                    new TableColumn
                    {
                        Title = L["Title"],
                        Sortable = true,
                        Data = nameof(EntryDto.Title)
                    },
                    new TableColumn
                    {
                        Title = L["Slug"],
                        Sortable = true,
                        Data = nameof(EntryDto.Slug)
                    },
                    new TableColumn
                    {
                        Title = L["Status"],
                        Sortable = true,
                        ValueConverter = (data)=> L[((EntryDto)data).Status.ToLocalizationKey()],
                        Data = nameof(EntryDto.Status)
                    },
                    new TableColumn
                    {
                        Title = L["PublishTime"],
                        Sortable = true,
                        Data = nameof(EntryDto.PublishTime)
                    },
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<EntryManagement>()
                    },
                });

            return base.SetTableColumnsAsync();
        }

        protected override async Task GetEntitiesAsync()
        {
            if (SectionId.HasValue)
            {
                await base.GetEntitiesAsync();
            }
        }


        protected override Task UpdateGetListInputAsync()
        {
            GetListInput.SectionId = SectionId.HasValue ? SectionId.Value : Guid.Empty;
            GetListInput.Culture = CultureName.IsNullOrEmpty()
                ? CurrentSite?.Languages.OrderByDescending(l => l.IsDefault).FirstOrDefault()?.CultureName 
                : CultureName;
            return base.UpdateGetListInputAsync();
        }


        protected async Task InitializePageDataAsync()
        {
            AllLanguages = await LanguageProvider.GetLanguagesAsync();
            AllSites = (await SiteAppService.GetListAsync(new GetSitesInput())).Items;
            if (SectionId.HasValue)
            {
                CurrentSection = await SectionAppService.GetAsync(SectionId.Value);
                CurrentSite = AllSites.First(s => s.Id == CurrentSection.SiteId);
                Sections = (await SectionAppService.GetListAsync(
                    new GetSectionsInput()
                    {
                        SiteId = CurrentSite.Id,
                        MaxResultCount = 1000
                    })).Items;
            }
            else
            {
                if (AllSites.Any())
                {
                    await OnSiteChangedAsync(
                        AllSites.
                        OrderBy(s=>s.CreationTime)
                        .First()
                        .Id);
                }
            }
        }

        protected async Task OnSiteChangedAsync(Guid siteId)
        {
            CurrentSite = AllSites.First(s => s.Id == siteId);
            Sections = (await SectionAppService.GetListAsync(
                new GetSectionsInput()
                {
                    SiteId = siteId,
                    MaxResultCount = 1000
                })).Items;
            CurrentSection = Sections
                .OrderByDescending(s => s.IsActive)
                .ThenByDescending(s => s.IsDefault)
                .FirstOrDefault();
            SectionId=CurrentSection?.Id;


            if (CurrentSection != null)
            {
                await OnSectionChangedAsync(CurrentSection.Id);
            }
            else
            {
                await SetToolbarItemsAsync();
                await SearchEntitiesAsync();
            }
        }

        protected async Task OnSectionChangedAsync(Guid sectionId)
        {
            SectionId = sectionId;
            CurrentSection = Sections.FirstOrDefault(s => s.Id == sectionId);
            CultureName = CultureName.IsNullOrEmpty()? 
                CurrentSite.Languages.OrderByDescending(l => l.IsDefault).First().CultureName:
                CurrentSite.Languages.Any(l=>l.CultureName== CultureName) ? CultureName : CurrentSite.Languages.OrderByDescending(l => l.IsDefault).First().CultureName;
            
            await OnCultureChangedAsync(CultureName);
        }

        protected async Task OnCultureChangedAsync(string value)
        {
            CultureName = value;
            GetListInput.Filter = null; 
            await SetToolbarItemsAsync();
            await SearchEntitiesAsync();
        }
    }
}
