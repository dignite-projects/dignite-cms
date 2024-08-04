using System;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Dignite.Cms.Domains
{
    public class Domain : CreationAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Domain(Guid id, string domainName, Guid? tenantId)
            :base(id)
        {
            DomainName = domainName;
            TenantId = tenantId;
        }

        public string DomainName { get; set; }

        public Guid? TenantId { get; set; }
    }
}
