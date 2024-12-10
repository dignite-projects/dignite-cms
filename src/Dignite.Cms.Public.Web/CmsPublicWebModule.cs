using Dignite.Abp.Localization;
using Dignite.Cms.Localization;
using Dignite.Cms.Public.Web.Routing;
using Dignite.CmsKit.Public.Web;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc.Localization;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.VirtualFileSystem;

namespace Dignite.Cms.Public.Web;

[DependsOn(
    typeof(CmsPublicApplicationContractsModule),
    typeof(DigniteCmsKitPublicWebModule),
    typeof(DigniteAbpLocalizationModule)
    )]
public class CmsPublicWebModule : AbpModule
{
    public override void PreConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.PreConfigure<AbpMvcDataAnnotationsLocalizationOptions>(options =>
        {
            options.AddAssemblyResource(typeof(CmsResource), typeof(CmsPublicWebModule).Assembly);
        });

        PreConfigure<IMvcBuilder>(mvcBuilder =>
        {
            mvcBuilder.AddApplicationPartIfNotExists(typeof(CmsPublicWebModule).Assembly);
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<CmsPublicWebModule>();
        });

        context.Services.AddAutoMapperObjectMapper<CmsPublicWebModule>();
        Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<CmsPublicWebModule>(validate: true);
        });

        context.Services.AddRouting(options =>
        {
            options.ConstraintMap.Add(CultureRouteSegmentConstraint.RouteConstraintName, typeof(CultureRouteSegmentConstraint));
        });
    }
}
