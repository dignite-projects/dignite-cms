namespace Dignite.Cms.Sites
{
    public class SiteLanguage
    {
        public SiteLanguage(bool isDefault, string language)
        {
            IsDefault = isDefault;
            Language = language;
        }

        protected SiteLanguage()
        {
        }

        public bool IsDefault { get; protected set; }

        public string Language { get; protected set; }
    }
}
