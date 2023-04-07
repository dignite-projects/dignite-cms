using System;
using Volo.Abp.Application.Dtos;

namespace Dignite.Cms.Admin.Fields
{
    public class FieldGroupDto: ExtensibleEntityDto<Guid>
    {
        public virtual string Name { get; set; }
    }
}
