namespace Dignite.Cms.Sites
{
    public class SiteCulture
    {
        public SiteCulture(bool isDefault, string cultureName)
        {
            IsDefault = isDefault;
            CultureName = cultureName;
        }

        protected SiteCulture()
        {
        }

        public bool IsDefault { get; protected set; }

        public string CultureName { get; protected set; }
    }
}
