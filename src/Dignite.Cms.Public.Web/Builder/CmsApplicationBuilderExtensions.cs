using Dignite.Cms.Public.Web.Controllers;
using Dignite.Cms.Public.Web.Routing;
using Microsoft.AspNetCore.Builder;

namespace Dignite.Cms.Public.Web.Builder
{
    public static class CmsApplicationBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCmsRoute(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: RouteConsts.HomeRouteName,
                    pattern: "/",
                    defaults: new { controller = CmsPublicWebController.ControllerName, action = nameof(CmsPublicWebController.Index) });
                endpoints.MapControllerRoute(
                    name: RouteConsts.EntryWithCultureRouteName,
                    pattern: "{"+ CultureRouteSegmentConstraint.RouteSegmentName + ":"+ CultureRouteSegmentConstraint.RouteConstraintName + "}/{*path}",
                    defaults: new { controller = CmsPublicWebController.ControllerName, action = nameof(CmsPublicWebController.EntryWithCulture) });
                endpoints.MapControllerRoute(
                    name: RouteConsts.EntryRouteName,
                    pattern: "{*path:regex(^(?!swagger/).*)}", //TODO: Use an options to configure the regular expression for the path
                    defaults: new { controller = CmsPublicWebController.ControllerName, action = nameof(CmsPublicWebController.Entry) });
            });

            return app;
        }
    }
}
