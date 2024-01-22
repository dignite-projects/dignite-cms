using Blazorise;
using Dignite.Cms.Admin.Sites;
using Dignite.Cms.Localization;
using Dignite.Cms.Permissions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.EntityActions;
using Volo.Abp.AspNetCore.Components.Web.Extensibility.TableColumns;
using Volo.Abp.AspNetCore.Components.Web.Theming.PageToolbars;
using Volo.Abp.Localization;

namespace Dignite.Cms.Admin.Blazor.Pages.Cms.Admin.Sites
{
    public partial class SiteManagement
    {
        protected IReadOnlyList<LanguageInfo> AllLanguages = new List<LanguageInfo>();
        protected PageToolbar Toolbar { get; } = new();
        protected List<TableColumn> SiteManagementTableColumns => TableColumns.Get<SiteManagement>();

        public SiteManagement()
        {
            ObjectMapperContext = typeof(CmsAdminBlazorModule);
            LocalizationResource = typeof(CmsResource);

            CreatePolicyName = CmsAdminPermissions.Site.Create;
            UpdatePolicyName = CmsAdminPermissions.Site.Update;
            DeletePolicyName = CmsAdminPermissions.Site.Delete;
        }

        protected override int PageSize => 1000;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                AllLanguages= await LanguageProvider.GetLanguagesAsync();
            }
            catch (Exception ex)
            {
                await HandleErrorAsync(ex);
            }
            await base.OnInitializedAsync();
        }

        protected override ValueTask SetToolbarItemsAsync()
        {
            Toolbar.AddButton(L["New"],
                OpenCreateModalAsync,
                IconName.Add,
                requiredPolicyName: CreatePolicyName);

            return base.SetToolbarItemsAsync();
        }

        protected override ValueTask SetEntityActionsAsync()
        {
            EntityActions
                .Get<SiteManagement>()
                .AddRange(new EntityAction[]
                {
                    new EntityAction
                    {
                        Text = L["Edit"],
                        Visible = (data) => HasUpdatePermission,
                        Clicked = async (data) => { await OpenEditModalAsync(data.As<SiteDto>()); }
                    },
                    new EntityAction
                    {
                        Text = L["Delete"],
                        Visible = (data) => HasDeletePermission,
                        Clicked = async (data) => await DeleteEntityAsync(data.As<SiteDto>()),
                        ConfirmationMessage = (data) => GetDeleteConfirmationMessage(data.As<SiteDto>())
                    }
                });

            return base.SetEntityActionsAsync();
        }

        protected override ValueTask SetTableColumnsAsync()
        {
            SiteManagementTableColumns
                .AddRange(new TableColumn[]
                {
                    new TableColumn
                    {
                        Title = L["DisplayName"],
                        Sortable = true,
                        Data = nameof(SiteDto.DisplayName)
                    },
                    new TableColumn
                    {
                        Title = L["Name"],
                        Sortable = true,
                        Data = nameof(SiteDto.Name)
                    },
                    new TableColumn
                    {
                        Title = L["IsActive"],
                        Sortable = true,
                        Data = nameof(SiteDto.IsActive)
                    },
                    new TableColumn
                    {
                        Title = L["Host"],
                        Data = nameof(SiteDto.Host)
                    },
                    new TableColumn
                    {
                        Title = L["CreationTime"],
                        Data = nameof(SiteDto.CreationTime)
                    },
                    new TableColumn
                    {
                        Title = L["Actions"],
                        Actions = EntityActions.Get<SiteManagement>()
                    },
                });

            return base.SetTableColumnsAsync();
        }
    }
}
