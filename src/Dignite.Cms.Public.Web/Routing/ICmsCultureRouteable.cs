﻿
namespace Dignite.Cms.Public.Web.Routing
{
    /// <summary>
    /// ICultureRouteable 是一个标记接口，用于指示页面是否需要通过 CmsRouteRequestCultureProvider 进行文化/语言处理。
    /// 只有实现了该接口的页面才会触发 DetermineProviderCultureResult 方法的执行，从而优化性能。
    /// </summary>
    /// <example>
    /// Example of use in Razor Pages
    /// <code>
    /// public class ProductPage : PageModel, ICultureRouteable 
    /// {
    ///     ...
    /// }
    /// </code>
    /// 
    /// Example of use in MVC
    /// <code>
    /// public class HomeController : Controller, ICultureRouteable
    /// {
    ///     ...
    /// }
    /// </code>
    /// </example>
    public interface ICmsCultureRouteable
    {
    }
}
