namespace Dignite.Cms.Sites
{
    public class SiteLanguage
    {
        public SiteLanguage(bool isDefault, string cultureName)
        {
            IsDefault = isDefault;
            CultureName = cultureName;
        }

        protected SiteLanguage()
        {
        }

        public bool IsDefault { get; protected set; }

        public string CultureName { get; protected set; }
    }
}
