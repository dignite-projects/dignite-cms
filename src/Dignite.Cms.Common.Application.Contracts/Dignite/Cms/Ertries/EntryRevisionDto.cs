using System;

namespace Dignite.Cms.Entries
{
    /// <summary>
    /// Entry Revision Information
    /// </summary>
    public class EntryRevisionDto
    {

        /// <summary>
        /// The entry of the initial entry
        /// </summary>
        public virtual Guid InitialId { get; set; }

        /// <summary>
        /// Version number
        /// </summary>
        public virtual int Version { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Notes on this modification operation
        /// </summary>
        public virtual string Notes { get; set; }
    }
}
