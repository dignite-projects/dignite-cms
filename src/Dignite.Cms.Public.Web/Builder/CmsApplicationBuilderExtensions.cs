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
        public static IApplicationBuilder UseCmsControllerRoute(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: CmsWebRouteConsts.DefaultRouteName,
                    pattern: "/",
                    defaults: new { controller = CmsController.ControllerName, action = nameof(CmsController.Default) });
                endpoints.MapControllerRoute(
                    name: CmsWebRouteConsts.CultureEntryRouteName,
                    pattern: "{"+ CultureRouteSegmentConstraint.RouteSegmentName + ":"+ CultureRouteSegmentConstraint.RouteConstraintName + "}/{*path}",
                    defaults: new { controller = CmsController.ControllerName, action = nameof(CmsController.CultureEntry) });
                endpoints.MapControllerRoute(
                    name: CmsWebRouteConsts.EntryRouteName,
                    pattern: "{*path:regex(^(?!swagger/|account/).*)}", //TODO: Use an options to configure the regular expression for the route
                    defaults: new { controller = CmsController.ControllerName, action = nameof(CmsController.Entry) });
            });

            return app;
        }
    }
}
