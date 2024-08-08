namespace Dignite.Cms.Settings
{
    public class BrandDto
    {
        public BrandDto()
        {
        }

        public BrandDto(string appName, string logoUrl, string logoReverseUrl)
        {
            AppName = appName;
            LogoUrl = logoUrl;
            LogoReverseUrl = logoReverseUrl;
        }

        /// <summary>
        /// Name of the site
        /// </summary>
        public string AppName { get; set; }

        /// <summary>
        /// LogoUrl of the site
        /// </summary>
        public string? LogoUrl { get; set; }

        /// <summary>
        /// LogoReverseUrl of the site
        /// </summary>
        public string? LogoReverseUrl { get; set; }
    }
}

