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
                    name: "cms-home",
                    pattern: "/",
                    defaults: new { controller = CmsController.ControllerName, action = "Index" });
                endpoints.MapControllerRoute(
                    name: "cms-entry-by-culture",
                    pattern: "{"+ CultureRouteSegmentConstraint.RouteSegmentName + ":"+ CultureRouteSegmentConstraint.RouteConstraintName + "}/{*url}",
                    defaults: new { controller = CmsController.ControllerName, action = "EntryByCulture" });
                endpoints.MapControllerRoute(
                    name: "cms-entry",
                    pattern: "{*url:regex(^(?!swagger/).*)}", //TODO: Use an options to configure the regular expression for the url
                    defaults: new { controller = CmsController.ControllerName, action = "Entry" });
            });

            return app;
        }
    }
}
