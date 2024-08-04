using Dignite.Cms.Public.Web.MultiTenancy;
using System.Collections.Generic;
using Volo.Abp.MultiTenancy;

namespace Dignite.Cms.MultiTenancy;

public static class AbpMultiTenancyOptionsExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public static void AddCmsDomainTenantResolver(this AbpTenantResolveOptions options)
    {
        options.TenantResolvers.InsertAfter(
            r => r is CurrentUserTenantResolveContributor,
            new CmsDomainTenantResolveContributor()
        );
    }
}
