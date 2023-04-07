using Dignite.Cms.Localization;
using Dignite.Cms.Public.Sections;
using Dignite.Cms.Public.Sites;

namespace Dignite.Cms.Public.Web.Views
{
    public abstract class CmsEntryRazorPage<TModel> : AbpRazorPage<TModel>
    {
        protected CmsEntryRazorPage()
        {
            LocalizationResourceType = typeof(CmsResource);
            ObjectMapperContext = typeof(CmsPublicWebModule);
        }
        public SiteDto Site
        {
            get { return Section.Site; }
        }
        public SectionDto Section
        {
            get { return ViewBag.Section; }
        }
    }
}
