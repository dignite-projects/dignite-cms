using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;
using Volo.Abp.MongoDB;
using Volo.CmsKit.MongoDB;

namespace Dignite.Cms.MongoDB;

[DependsOn(
    typeof(CmsDomainModule),
    typeof(AbpMongoDbModule),
    typeof(CmsKitMongoDbModule)
    )]
public class CmsMongoDbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddMongoDbContext<CmsMongoDbContext>(options =>
        {
                /* Add custom repositories here. Example:
                 * options.AddRepository<Question, MongoQuestionRepository>();
                 */
        });
    }
}
