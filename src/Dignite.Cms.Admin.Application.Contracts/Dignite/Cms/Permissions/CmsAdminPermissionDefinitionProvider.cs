﻿using Dignite.Cms.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Dignite.Cms.Permissions
{
    public class CmsAdminPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var group = context.AddGroup(CmsAdminPermissions.GroupName, L("Permission:CmsAdmin"));

            var sites = group.AddPermission(CmsAdminPermissions.Site.Default, L("Permission:Site"));
            sites.AddChild(CmsAdminPermissions.Site.Create, L("Permission:Create"));
            sites.AddChild(CmsAdminPermissions.Site.Update, L("Permission:Edit"));
            sites.AddChild(CmsAdminPermissions.Site.Delete, L("Permission:Delete"));

            var fields = group.AddPermission(CmsAdminPermissions.Field.Default, L("Permission:Field"));
            fields.AddChild(CmsAdminPermissions.Field.Create, L("Permission:Create"));
            fields.AddChild(CmsAdminPermissions.Field.Update, L("Permission:Edit"));
            fields.AddChild(CmsAdminPermissions.Field.Delete, L("Permission:Delete"));

            var sections = group.AddPermission(CmsAdminPermissions.Section.Default, L("Permission:Section"));
            sections.AddChild(CmsAdminPermissions.Section.Create, L("Permission:Create"));
            sections.AddChild(CmsAdminPermissions.Section.Update, L("Permission:Edit"));
            sections.AddChild(CmsAdminPermissions.Section.Delete, L("Permission:Delete"));

            var entries = group.AddPermission(CmsAdminPermissions.Entry.Default, L("Permission:Entry"));
            entries.AddChild(CmsAdminPermissions.Entry.Create, L("Permission:Create"));
            entries.AddChild(CmsAdminPermissions.Entry.Update, L("Permission:Edit"));
            entries.AddChild(CmsAdminPermissions.Entry.Delete, L("Permission:Delete"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<CmsResource>(name);
        }
    }
}