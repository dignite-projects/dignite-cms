using System;
using System.Collections.Generic;
using System.Linq;
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
            Languages = new List<SiteLanguage>();
        }

        public Site(Guid id, string displayName, string name, string host, bool isActive, Guid? tenantId)
            :base(id)
        {
            DisplayName = displayName;
            Name = name;
            Host = host;
            IsActive = isActive;
            TenantId = tenantId;
            Languages = new List<SiteLanguage>();
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
        /// Host url of this site
        /// </summary>
        public virtual string Host { get; protected set; }


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
        public virtual void SetHost(string host)
        {
            Host = host;
        }
        public virtual void AddLanguage(SiteLanguage language)
        {
            if (!Languages.Any(l => l.CultureName.Equals(language.CultureName, StringComparison.InvariantCultureIgnoreCase)))
                Languages.Add(language);
        }
        public virtual void RemoveLanguage(string cultureName) 
        {
            Languages.RemoveAll(l=>l.CultureName.Equals(cultureName,StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
