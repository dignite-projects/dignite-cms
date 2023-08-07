using Blazorise;
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

namespace Dignite.Cms.Admin.Blazor.Pages.Cms.Admin.Entries
{
    public partial class EntryManagement
    {
        [Parameter]
        [SupplyParameterFromQuery]
        public Guid? SectionId { get; set; }

        [Parameter]
        [SupplyParameterFromQuery]
        public string Language { get; set; }

        protected PageToolbar Toolbar { get; } = new();
        protected List<TableColumn> EntryManagementTableColumns => TableColumns.Get<EntryManagement>();
        protected IReadOnlyList<LanguageInfo> AllLanguages = new List<LanguageInfo>();
        protected IReadOnlyList<SiteDto> AllSites { get; set; }=new List<SiteDto>();
        protected IReadOnlyList<SectionDto> Sections { get; set; }=new List<SectionDto>();
        protected SiteDto CurrentSite { get; set; }
        protected SectionDto CurrentSection { get; set; }
        protected string CurrentSectionName { get; set; } 

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
            await base.OnInitializedAsync();
            await InitializePageDataAsync();
        }



        protected override ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["New"],
                OnNewEntryAsync,
                IconName.Add,
                requiredPolicyName: CreatePolicyName);

            return base.SetToolbarItemsAsync();
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
                            Navigation.NavigateTo($"/cms/admin/entries/{data.As<EntryDto>().Id}/edit");
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
                        Title = L["Title"],
                        Sortable = true,
                        Data = nameof(EntryDto.Title)
                    },
                    new TableColumn
                    {
                        Title = L["Name"],
                        Sortable = true,
                        Data = nameof(EntryDto.Slug)
                    },
                    new TableColumn
                    {
                        Title = L["Status"],
                        Sortable = true,
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


        protected override string GetDeleteConfirmationMessage(EntryDto entity)
        {
            return string.Format(L["EntryDeletionConfirmationMessage"], entity.Title);
        }

        protected override Task UpdateGetListInputAsync()
        {
            if (CurrentSection != null)
            {
                GetListInput.SectionId = CurrentSection.Id;
                GetListInput.Language = Language;
            }
            else
            {
                GetListInput.SectionId = SectionId.HasValue ? SectionId.Value : Guid.Empty;
                GetListInput.Language = Language.IsNullOrEmpty() ? "en" : Language;
            }
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
                CurrentSectionName = CurrentSection.Name;
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
                        OrderByDescending(s=>s.IsActive)
                        .ThenByDescending(s=>s.IsDefault)
                        .First()
                        .Id);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
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


            if (CurrentSection != null)
            {
                await OnSectionClickAsync(CurrentSection);
            }
        }

        protected async Task OnSectionClickAsync(SectionDto value)
        {
            CurrentSection = value;
            CurrentSectionName = CurrentSection.Name;
            Language = Language.IsNullOrEmpty()? 
                CurrentSite.Languages.OrderByDescending(l => l.IsDefault).First().Language:
                CurrentSite.Languages.Any(l=>l.Language== Language)?Language: CurrentSite.Languages.OrderByDescending(l => l.IsDefault).First().Language;

            GetListInput.Filter = null; 
            await SearchEntitiesAsync();
        }

        protected async Task OnLanguageChangedAsync(string value)
        { 
            Language = value;
            await SearchEntitiesAsync();
        }

        protected async Task OnNewEntryAsync()
        {
            if (CurrentSection == null)
            { 
                await Notify.Error(L["PleaseSelectSection"]);
            }
            else
            {
                if (CurrentSection.EntryTypes.Any())
                {
                    Navigation.NavigateTo($"/cms/admin/entries/create?sectionId={CurrentSection.Id}&language={Language}");
                }
                else
                {
                    await Notify.Error(L["{0}SectionHasNotCreatedAnEntryType", CurrentSection.DisplayName]);
                }
            }
        }
    }
}
