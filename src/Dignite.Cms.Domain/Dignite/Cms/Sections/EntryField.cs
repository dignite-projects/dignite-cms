using System;

namespace Dignite.Cms.Sections
{
    public class EntryField
    {
        public EntryField(Guid fieldId, string displayName, bool required,  bool searchable)
        {
            FieldId = fieldId;
            DisplayName = displayName;
            Required = required;
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
        public virtual bool Required { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool Searchable { get; protected set; }
    }
}
