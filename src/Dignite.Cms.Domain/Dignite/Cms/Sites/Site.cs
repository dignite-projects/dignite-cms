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

        public Site(Guid id, string displayName, string name, ICollection<SiteLanguage> languages, string host, bool isDefault, bool isActive, Guid? tenantId)
            :base(id)
        {
            DisplayName = displayName;
            Name = name;
            Languages = languages;
            Host = host;
            IsDefault = isDefault;
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
        /// Languages supported on this site
        /// </summary>
        public ICollection<SiteLanguage> Languages { get; protected set; }

        /// <summary>
        /// Host of this site
        /// </summary>
        public virtual string Host { get; protected set; }

        /// <summary>
        /// The default site
        /// </summary>
        public virtual bool IsDefault { get; protected set; }


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
            if (IsDefault && !isActive)
            {
                throw new DefaultSiteCannotSetNotActiveException(DisplayName);
            }
            IsActive= isActive;
        }
        public virtual void SetDefault(bool isDefault)
        {
            IsDefault= isDefault;
        }
        public virtual void SetDisplayName(string displayName)
        {
            DisplayName = displayName;
        }
        public virtual void SetName(string name)
        {
            Name = name;
        }
        public virtual void SetHost(string host)
        {
            Host = host;
        }
        public virtual void SetLanguages(ICollection<SiteLanguage> languages)
        {
            Languages = languages;
        }
    }
}
