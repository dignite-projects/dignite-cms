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
                    name: CmsWebRouteConsts.HomePageRouteName,
                    pattern: "/",
                    defaults: new { controller = EntryController.ControllerName, action = nameof(EntryController.HomePage) });
                endpoints.MapControllerRoute(
                    name: CmsWebRouteConsts.EntryPageWithCultureRouteName,
                    pattern: "{"+ CultureRouteSegmentConstraint.RouteSegmentName + ":"+ CultureRouteSegmentConstraint.RouteConstraintName + "}/{*entryPath}",
                    defaults: new { controller = EntryController.ControllerName, action = nameof(EntryController.EntryPageWithCulture) });
                endpoints.MapControllerRoute(
                    name: CmsWebRouteConsts.EntryPageRouteName,
                    pattern: "{*entryPath:regex(^(?!swagger/|account/).*)}", //TODO: Use an options to configure the regular expression for the entryPath
                    defaults: new { controller = EntryController.ControllerName, action = nameof(EntryController.EntryPage) });
            });

            return app;
        }
    }
}
