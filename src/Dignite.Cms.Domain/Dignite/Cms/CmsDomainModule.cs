using Dignite.FileExplorer;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Domain;
using Volo.Abp.Domain.Entities.Events.Distributed;
using Volo.Abp.Modularity;
using Volo.CmsKit;

namespace Dignite.Cms;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(CmsDomainSharedModule),
    typeof(FileExplorerDomainModule),
    typeof(CmsKitDomainModule)
)]
public class CmsDomainModule : AbpModule
{
}
