
using Dignite.Abp.AspNetCore.Components.CkEditor.Server;
using Volo.Abp.AspNetCore.Components.Server.Theming;
using Volo.Abp.Modularity;

namespace Dignite.Cms.Admin.Blazor.Server
{
    [DependsOn(
        typeof(AbpAspNetCoreComponentsServerThemingModule),
        typeof(CmsAdminBlazorModule),
        typeof(DigniteAbpAspNetCoreComponentsCkEditorServerModule)
        )]
    public class CmsAdminBlazorServerModule : AbpModule
    {
        
    }
}