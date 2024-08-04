using System;
using Volo.Abp.Application.Dtos;

namespace Dignite.Cms.Admin.Domains
{
    public class DomainDto:CreationAuditedEntityDto<Guid>
    {
        public string DomainName { get; set; }
    }
}
