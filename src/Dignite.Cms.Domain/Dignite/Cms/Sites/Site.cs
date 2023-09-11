using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace Dignite.Cms.Sites
{
    /// <summary>
    /// A sub site information
    /// </summary>
    public class Site: FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        protected Site()
        {
        }

        public Site(Guid id, string displayName, string name, ICollection<SiteRegion> regions, string hostUrl, bool isActive, Guid? tenantId)
            :base(id)
        {
            DisplayName = displayName;
            Name = name;
            Regions = regions;
            HostUrl = hostUrl;
            IsActive = isActive;
            TenantId = tenantId;
        }

        /// <summary>
        /// Display Name of this site.
        /// </summary>
        public virtual string DisplayName { get; protected set; }

        /// <summary>
        /// Name of this site.
        /// Site Unique Name.
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// Regions supported on this site
        /// </summary>
        public ICollection<SiteRegion> Regions { get; protected set; }

        /// <summary>
        /// Host url of this site
        /// </summary>
        public virtual string HostUrl { get; protected set; }


        /// <summary>
        /// Is this site active
        /// </summary>
        public virtual bool IsActive { get; protected set; }

        /// <summary>
        /// TenantId of this site.
        /// </summary>
        public virtual Guid? TenantId { get; protected set; }

        public virtual void SetActive(bool isActive) 
        {
            IsActive= isActive;
        }
        public virtual void SetDisplayName(string displayName)
        {
            DisplayName = displayName;
        }
        public virtual void SetName(string name)
        {
            Name = name;
        }
        public virtual void SetHostUrl(string hostUrl)
        {
            HostUrl = hostUrl;
        }
        public virtual void SetRegions(ICollection<SiteRegion> regions)
        {
            Regions = regions;
        }
    }
}
