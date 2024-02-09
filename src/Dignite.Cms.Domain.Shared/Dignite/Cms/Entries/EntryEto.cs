using Dignite.Abp.Data;
using System;
using Volo.Abp.Data;
using Volo.Abp.EventBus;
using Volo.Abp.MultiTenancy;

namespace Dignite.Cms.Entries
{
    [EventName("Dignite.Cms.Entries.Entry")]
    [Serializable]
    public class EntryEto: IHasCustomFields, IMultiTenant
    {
        public Guid Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid SectionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid EntryTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Slug { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime PublishTime { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public EntryStatus Status { get; protected set; }


        public ExtraPropertyDictionary ExtraProperties { get; set; }


        #region Section type is a exclusive property of Structure type

        /// <summary>
        /// Parent entry id of the entry;
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Order of the entry
        /// </summary>
        public int Order { get; set; }
        #endregion

        public Guid? TenantId { get; protected set; }
    }
}
