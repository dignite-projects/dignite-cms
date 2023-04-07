using Dignite.Cms.Localization;
using Dignite.Cms.Permissions;
using System.Threading.Tasks;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.UI.Navigation;

namespace Dignite.Cms.Admin.Blazor.Menus
{
    public class CmsAdminMenuContributor : IMenuContributor
    {
        public async Task ConfigureMenuAsync(MenuConfigurationContext context)
        {
            if (context.Menu.Name == StandardMenus.Main)
            {
                await ConfigureMainMenuAsync(context);
            }
        }

        private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
        {
            var l = context.GetLocalizer<CmsResource>();


            var cmsAdminMenuItem = new ApplicationMenuItem(
                CmsAdminMenus.GroupName, 
                l["Menu:Cms"],
                icon: "far fa-id-card",
                requiredPermissionName:CmsAdminPermissions.Entry.Default);
            context.Menu.AddItem(cmsAdminMenuItem);


            cmsAdminMenuItem.AddItem(new ApplicationMenuItem(
                    CmsAdminMenus.Fields,
                    l["Fields"],
                    url: "~/cms/admin/fields",
                    icon: "fa fa-file-alt").RequirePermissions(CmsAdminPermissions.Field.Default));

            cmsAdminMenuItem.AddItem(new ApplicationMenuItem(
                    CmsAdminMenus.Sites,
                    l["Sites"],
                    url: "~/cms/admin/sites",
                    icon: "fa fa-file").RequirePermissions(CmsAdminPermissions.Site.Default));

            cmsAdminMenuItem.AddItem(new ApplicationMenuItem(
                    CmsAdminMenus.Sections,
                    l["Sections"],
                    url: "~/cms/admin/sections",
                    icon: "fa fa-file-alt").RequirePermissions(CmsAdminPermissions.Section.Default));

            cmsAdminMenuItem.AddItem(new ApplicationMenuItem(
                    CmsAdminMenus.Entries,
                    l["Entries"],
                    url: "~/cms/admin/entries",
                    icon: "fa fa-file-alt").RequirePermissions(CmsAdminPermissions.Entry.Default));

            return Task.CompletedTask;
        }
    }
}