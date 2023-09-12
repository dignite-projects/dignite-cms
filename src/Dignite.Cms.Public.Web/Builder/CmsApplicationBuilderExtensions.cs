using Dignite.Cms.Public.Web.Controllers;
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
                    name: "cms-entry-by-region",
                    pattern: "{region:RegionalConstraint}/{*url}",
                    defaults: new { controller = CmsController.ControllerName, action = "EntryByRegion" });
                endpoints.MapControllerRoute(
                    name: "cms-entry",
                    pattern: "{*url:regex(^(?!swagger/).*)}",
                    defaults: new { controller = CmsController.ControllerName, action = "Entry" });
            });

            return app;
        }
    }
}
