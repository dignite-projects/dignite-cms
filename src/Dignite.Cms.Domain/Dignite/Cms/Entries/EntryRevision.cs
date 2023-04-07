using System;

namespace Dignite.Cms.Entries
{
    /// <summary>
    /// Entry Revision Information
    /// </summary>
    public class EntryRevision
    {
        protected EntryRevision()
        {
        }

        public EntryRevision(Guid initialId, int version,bool isActive, string notes)
        {
            Version= version;
            IsActive= isActive;
            InitialId = initialId;
            Notes = notes;
        }

        /// <summary>
        /// The id of the initial entry id
        /// </summary>
        public virtual Guid InitialId { get; protected set; }

        /// <summary>
        /// Version number
        /// </summary>
        public virtual int Version { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsActive { get; protected set; }

        /// <summary>
        /// Notes on this modification operation
        /// </summary>
        public virtual string Notes { get; set; }


        public virtual void SetIsActive(bool isActive)
        {
            this.IsActive = isActive;
        }
    }
}
