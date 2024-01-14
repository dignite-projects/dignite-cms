using System;
using Volo.Abp.Application.Dtos;

namespace Dignite.Cms.Admin.Fields
{
    public class FieldGroupDto: EntityDto<Guid>
    {
        public virtual string Name { get; set; }
    }
}
