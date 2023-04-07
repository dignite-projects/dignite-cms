using Dignite.Cms.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace Dignite.Cms.Public.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class CmsPageModel : AbpPageModel
{
    protected CmsPageModel()
    {
        LocalizationResourceType = typeof(CmsResource);
        ObjectMapperContext = typeof(CmsPublicWebModule);
    }
}
