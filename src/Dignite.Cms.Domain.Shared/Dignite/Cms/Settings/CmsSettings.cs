namespace Dignite.Cms.Settings;

public static class CmsSettings
{
    private const string Prefix = "Cms";

    public static class Site
    {
        private const string SitePrefix = Prefix + ".Site";

        /// <summary>
        /// Languages supported by this site
        /// </summary>
        public const string Languages = SitePrefix + ".Languages";

    }
}
