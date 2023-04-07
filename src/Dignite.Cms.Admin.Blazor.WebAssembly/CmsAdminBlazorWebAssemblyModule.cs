
using Volo.Abp.AspNetCore.Components.WebAssembly.Theming;
using Volo.Abp.Modularity;

namespace Dignite.Cms.Admin.Blazor.WebAssembly
{
    [DependsOn(
        typeof(CmsAdminBlazorModule),
        typeof(CmsAdminHttpApiClientModule),
        typeof(AbpAspNetCoreComponentsWebAssemblyThemingModule)
        )]
    public class CmsAdminBlazorWebAssemblyModule : AbpModule
    {
        
    }
}