namespace Dignite.Cms.Sites
{
    public static class SiteConsts
    {
        /// <summary>
        /// Maximum length of the site display name property.
        /// Default value: 64
        /// </summary>
        public static int MaxDisplayNameLength { get; set; } = 64;

        /// <summary>
        /// Maximum length of the site name property.
        /// Default value: 64
        /// </summary>
        public static int MaxNameLength { get; set; } = 64;

        /// <summary>
        /// Regular Expression of the site name property.
        /// </summary>
        public const string NameRegularExpression= "^[A-Za-z0-9_-]+$";

        /// <summary>
        /// Maximum length of the site culture property.
        /// Default value: 16
        /// </summary>
        public static int MaxCultureLength { get; set; } = 16;

        /// <summary>
        /// Maximum length of the site host property.
        /// Default value: 128
        /// </summary>
        public static int MaxHostLength { get; set; } = 128;
    }
}
