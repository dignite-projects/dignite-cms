namespace Dignite.Cms.Settings;

public static class CmsSettings
{
    private const string Prefix = "Cms";

    public static class Site
    {
        private const string SitePrefix = Prefix + ".Site";

        public const string Languages = SitePrefix + ".Languages";
    }
}
