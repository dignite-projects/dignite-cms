namespace Dignite.Cms.Domains
{
    public static class DomainConsts
    {
        /// <summary>
        /// Maximum length of the site domain name property.
        /// Default value: 256
        /// </summary>
        public static int MaxDomainNameLength { get; set; } = 256;


        /// <summary>
        /// Regular Expression of the Name property.
        /// </summary>
        public const string NameRegularExpression = @"^((?!-)[A-Za-z0-9-]{1,63}(?<!-)\.)+[A-Za-z]{2,6}(:\d{1,5})?$";
    }
}
