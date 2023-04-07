using System;
using Volo.Abp.Domain.Entities;

namespace Dignite.Cms.Admin.Sites
{
    [Serializable]
    public class UpdateSiteInput : CreateOrUpdateSiteInputBase, IHasConcurrencyStamp
    {
        public UpdateSiteInput() : base()
        {
        }

        public string ConcurrencyStamp { get; set; }
    }
}
