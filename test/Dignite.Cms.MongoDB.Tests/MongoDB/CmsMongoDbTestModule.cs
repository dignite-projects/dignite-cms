
using Volo.Abp.Data;
using Volo.Abp.Modularity;

namespace Dignite.Cms.MongoDB;

[DependsOn(
    typeof(CmsTestBaseModule),
    typeof(CmsMongoDbModule)
    )]
public class CmsMongoDbTestModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpDbConnectionOptions>(options =>
        {
            options.ConnectionStrings.Default = MongoDbFixture.GetRandomConnectionString();
        });
    }
}
