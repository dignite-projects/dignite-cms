using Dignite.Cms.Fields;
using System;

namespace Dignite.Cms.Sections
{
    public class EntryFieldDto
    {
        public Guid FieldId { get; set; }

        public FieldDto Field { get; set; }

        /// <summary>
        /// Text to override field definition
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Value to override field definition
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Text to override <see cref="Fields.Field.Instructions"/>
        /// </summary>
        public string Instructions { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Searchable { get; set; }
    }
}
