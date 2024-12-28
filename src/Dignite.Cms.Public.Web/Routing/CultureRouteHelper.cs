using Dignite.Cms.Public.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Dignite.Cms.Public.Web.Routing
{
    public class CultureRouteHelper : ISingletonDependency
    {
        private readonly EndpointDataSource _endpointDataSource;
        private readonly IMemoryCache _EndpointCache;

        public CultureRouteHelper(EndpointDataSource endpointDataSource, IMemoryCache endpointCache)
        {
            _endpointDataSource = endpointDataSource;
            _EndpointCache = endpointCache;
        }

        public bool TryMatchRoute(HttpContext httpContext)
        {
            return TryMatchRoute(httpContext, out string routePattern);
        }

        /// <summary>
        /// 判断当前页面Url是否匹配带有Culture路由参数的路由
        /// </summary>
        /// <returns></returns>
        public bool TryMatchRoute(HttpContext httpContext, out string routePattern)
        {
            routePattern = null;
            //获取所有包含culture路由参数的路由
            //这里创建了一个新list对象，确保对list的任何操作不影响GetAllEndpoints方法内部的缓存数据
            var allEndpoints = GetAllEndpoints().ToList();

            //当路由表中找不到匹配的路由时，最终将使用{culture:CultureConstraint}/{*path}路由
            //造成任意页面URL都将匹配到一个路由，这种机制应该只适用于Cms Entry的页面
            //因此，如果当前页面不是Cms Entry时，则移除allEndpoints中{culture:CultureConstraint}/{*path}的路由
            var routeData = httpContext.GetRouteData();
            if (!routeData.Values.TryGetValue("controller", out var controllerName) ||
                !string.Equals(controllerName.ToString(), CmsController.ControllerName, StringComparison.OrdinalIgnoreCase))
            {
                var cmsControllerType = typeof(CmsController);
                var cmsControllerModuleName = cmsControllerType.Module.Name.Substring(0, cmsControllerType.Module.Name.LastIndexOf('.')); //Value is (Dignite.Cms.Public.Web)

                //移除allEndpoints中{culture:CultureConstraint}/{*path}的路由
                //即移除DisplayName为 Dignite.Cms.Public.Web.Controllers.CmsController.CultureEntry (Dignite.Cms.Public.Web) 的路由
                allEndpoints.RemoveAll(ep => ep.DisplayName == $"{cmsControllerType.FullName}.{nameof(CmsController.CultureEntry)} ({cmsControllerModuleName})");
            }

            //
            var culture = httpContext.GetRouteValue(CultureRouteSegmentConstraint.RouteSegmentName)?.ToString();

            //
            foreach (var endpoint in allEndpoints)
            {
                var routeEndpoint = (RouteEndpoint)endpoint;
                var requestPath = httpContext.Request.Path.Value; 

                //如果当前请求的页面路径(requestPath)中不包含culture路由参数
                //则根据本次循环的routePattern格式，向当前请求的页面路径(requestPath)中添加culture路由参数
                if (culture.IsNullOrEmpty())
                {
                    //在这种情况下，culture路由参数只会存在于routePattern的首部或routePattern的尾部
                    var routePatternRawText = routeEndpoint.RoutePattern.RawText.Replace(" ", "").Trim('/');
                    var cultureSegment = $"{{{CultureRouteSegmentConstraint.RouteSegmentName}:{CultureRouteSegmentConstraint.RouteConstraintName}}}";
                    var matchingCulture = "en";
                    if (routePatternRawText.StartsWith(cultureSegment))
                    {
                        requestPath = matchingCulture + requestPath;
                    }
                    else if (routePatternRawText.EndsWith(cultureSegment))
                    {
                        requestPath = requestPath.EnsureEndsWith('/') + matchingCulture;
                    }
                    else
                    {
                        continue;
                    }
                }


                // 添加自定义约束处理器
                var routeTemplate = new RouteTemplate(routeEndpoint.RoutePattern);
                var defaults = new RouteValueDictionary(); // 定义默认值
                var matcher = new TemplateMatcher(routeTemplate, defaults);
                var values = new RouteValueDictionary();


                if (matcher.TryMatch(requestPath.EnsureStartsWith('/'), values))
                {
                    // 解析 URL 段
                    var urlSegments = requestPath.Trim('/').Split('/');
                    if (urlSegments.Length > 0)
                    {
                        // 验证文化约束
                        var constraint = new CultureRouteSegmentConstraint();
                        if (constraint.Match(httpContext, null, CultureRouteSegmentConstraint.RouteSegmentName, values, RouteDirection.IncomingRequest))
                        {
                            routePattern = routeEndpoint.RoutePattern.RawText;
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private List<Endpoint> GetAllEndpoints()
        {
            var cacheKey = "SiteAllEndpoints";
            if (!_EndpointCache.TryGetValue(cacheKey, out List<Endpoint> cacheValue))
            {
                cacheValue = CreateEndpointsList();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromDays(100));

                _EndpointCache.Set(cacheKey, cacheValue, cacheEntryOptions);
            }

            return cacheValue;
        }

        private List<Endpoint> CreateEndpointsList()
        {
            var endpoints = new List<Endpoint>();

            // 添加常规路由端点
            foreach (var endpoint in _endpointDataSource.Endpoints)
            {
                var routeEndpoint = (RouteEndpoint)endpoint;
                var routePattern = routeEndpoint.RoutePattern;

                // 检查是否包含文化约束
                var hasCultureConstraint = routePattern.Parameters
                    .Any(p => p.Name == CultureRouteSegmentConstraint.RouteSegmentName &&
                             p.ParameterPolicies.Any(policy => policy.Content == CultureRouteSegmentConstraint.RouteConstraintName)
                        );

                if (hasCultureConstraint)
                {
                    if (!endpoints.Any(ep => ((RouteEndpoint)ep).RoutePattern.RawText == routeEndpoint.RoutePattern.RawText))
                    {
                        endpoints.Add(endpoint);
                    }
                }
            }

            return endpoints
                .OrderBy(ep => ((RouteEndpoint)ep).Order)
                .ToList();
        }
    }
}
