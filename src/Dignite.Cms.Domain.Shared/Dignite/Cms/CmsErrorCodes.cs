namespace Dignite.Cms;

public static class CmsErrorCodes
{
    public static class Sites
    {
        public const string NameAlreadyExist = "Cms:Site:0001";
        public const string DefaultSiteCannotSetNotActive = "Cms:Site:0002";
    }
    public static class Sections
    {
        public const string NameAlreadyExist = "Cms:Section:0001";
        public const string DefaultSectionCannotSetNotActive = "Cms:Section:0002";
    }
    public static class EntryTypes
    {
        public const string NameAlreadyExist = "Cms:EntryType:0001";
    }
    public static class Fields
    {
        public const string NameAlreadyExist = "Cms:Field:0001";
    }

    public static class Entries
    {
        public const string SlugAlreadyExist = "Cms:Entry:0001";
    }
}
