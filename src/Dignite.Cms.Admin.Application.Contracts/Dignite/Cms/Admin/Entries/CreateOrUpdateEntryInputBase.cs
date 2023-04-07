using Dignite.Abp.DynamicForms;
using Dignite.Cms.Admin.Sections;
using Dignite.Cms.Entries;
using Dignite.Cms.Fields;
using Dignite.Cms.Sites;
using Microsoft.Extensions.DependencyInjection;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Threading;
using Volo.Abp.Validation;

namespace Dignite.Cms.Admin.Entries
{
    public abstract class CreateOrUpdateEntryInputBase: CustomizableObject<FieldDto>
    {
        protected CreateOrUpdateEntryInputBase():base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid EntryTypeId { get; set; }

        /// <summary>
        /// The language corresponding to the entry
        /// </summary>
        [Required]
        [DynamicMaxLength(typeof(SiteConsts), nameof(SiteConsts.MaxLanguageLength))]
        public string Language { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DynamicMaxLength(typeof(EntryConsts), nameof(EntryConsts.MaxTitleLength))]
        public string Title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [DynamicMaxLength(typeof(EntryConsts), nameof(EntryConsts.MaxSlugLength))]
        [RegularExpression(EntryConsts.SlugRegularExpression)]
        public string Slug { get; set; }



        /// <summary>
        /// 
        /// </summary>
        [Required]
        public DateTime PublishTime { get; set; }


        /// <summary>
        /// Notes on this modification operation
        /// </summary>
        [DynamicMaxLength(typeof(EntryConsts), nameof(EntryConsts.MaxRevisionNotesLength))]
        public string RevisionNotes { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validationErrors = new List<ValidationResult>();
            return validationErrors;
        }

        /// <summary>
        /// Get the definition information of the field for data validation
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public override IReadOnlyList<FieldDto> GetFieldDefinitions(ValidationContext validationContext)
        {
            var _entryTypeAppService = validationContext.GetRequiredService<IEntryTypeAdminAppService>();
            var entryType = AsyncHelper.RunSync(()=> _entryTypeAppService.GetAsync(this.EntryTypeId));
            var fields=new List<FieldDto>();
            foreach (var tab in entryType.FieldTabs)
            {
                fields.AddRange(tab.Fields.Select(ef => ef.Field));
            }
            return fields;
        }
    }
}
