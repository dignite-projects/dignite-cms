using System;

namespace Dignite.Cms.Sections
{
    public class EntryField
    {
        public EntryField(Guid fieldId, string displayName, bool searchable)
        {
            FieldId = fieldId;
            DisplayName = displayName;
            Searchable = searchable;
        }

        public Guid FieldId { get; protected set; }


        /// <summary>
        /// Text to override field definition
        /// </summary>
        public virtual string DisplayName { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool Searchable { get; protected set; }
    }
}
