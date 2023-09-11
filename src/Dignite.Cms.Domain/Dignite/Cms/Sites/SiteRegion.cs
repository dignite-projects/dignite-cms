namespace Dignite.Cms.Sites
{
    public class SiteRegion
    {
        public SiteRegion(bool isDefault, string region)
        {
            IsDefault = isDefault;
            Region = region;
        }

        protected SiteRegion()
        {
        }

        public bool IsDefault { get; protected set; }

        public string Region { get; protected set; }
    }
}
