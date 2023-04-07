﻿using Volo.Abp.Reflection;

namespace Dignite.Cms.Permissions
{
    public class CmsAdminPermissions
    {
        public const string GroupName = "CmsAdmin";

        public static class Site
        {
            public const string Default = GroupName + ".Site";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }
        public static class Field
        {
            public const string Default = GroupName + ".Field";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }


        public static class Section
        {
            public const string Default = GroupName + ".Section";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }

        public static class Entry
        {
            public const string Default = GroupName + ".Entry";
            public const string Create = Default + ".Create";
            public const string Update = Default + ".Update";
            public const string Delete = Default + ".Delete";
        }


        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(CmsAdminPermissions));
        }
    }
}