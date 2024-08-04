using System;
using Volo.Abp.Application.Dtos;

namespace Dignite.Cms.Public.Domains
{
    public class DomainDto:EntityDto<Guid>
    {
        public string DomainName { get; set; }

        public Guid TenantId { get; set; }
    }
}
