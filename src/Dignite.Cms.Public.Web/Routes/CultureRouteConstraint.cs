using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace Dignite.Cms.Public.Web.Routes
{
    public class CultureRouteConstraint: IRouteConstraint
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            string name = values["culture"]?.ToString();
            Regex rgx = new Regex(@"^(zh-hans|en)$", RegexOptions.IgnoreCase);
            if (rgx.IsMatch(name))
            {
                return true;
            }

            return false;
        }
    }
}
