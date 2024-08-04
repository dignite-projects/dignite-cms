using Dignite.Cms.Public.Domains;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.MultiTenancy;
using Volo.Abp.MultiTenancy;

namespace Dignite.Cms.Public.Web.MultiTenancy
{
    public class CmsDomainTenantResolveContributor : HttpTenantResolveContributorBase
    {
        public const string ContributorName = "CmsDomain";

        public override string Name => ContributorName;

        public CmsDomainTenantResolveContributor()
        {
        }

        protected override async Task<string?> GetTenantIdOrNameFromHttpContextOrNullAsync(ITenantResolveContext context, HttpContext httpContext)
        {
            if (!httpContext.Request.Host.HasValue)
            {
                return null;
            }

            var hostName = httpContext.Request.Host.Host;
            using (var scope = context.ServiceProvider.CreateScope())
            {
                var domainPublicAppService = scope.ServiceProvider.GetRequiredService<IDomainPublicAppService>();
                var domain = await domainPublicAppService.FindByNameAsync(hostName);
                if (domain == null)
                    return null;
                else
                {
                    context.Handled = true;
                    return domain.TenantId.ToString();
                }
            }
        }
    }
}