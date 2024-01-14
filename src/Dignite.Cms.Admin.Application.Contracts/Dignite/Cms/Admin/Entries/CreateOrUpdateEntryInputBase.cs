using Dignite.Abp.Data;
using Dignite.Abp.DynamicForms;
using Dignite.Cms.Admin.Sections;
using Dignite.Cms.Entries;
using Dignite.Cms.Fields;
using Dignite.Cms.Sections;
using Dignite.Cms.Sites;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Volo.Abp.Data;
using Volo.Abp.Threading;
using Volo.Abp.Validation;

namespace Dignite.Cms.Admin.Entries
{
    public abstract class CreateOrUpdateEntryInputBase: CustomizableObject
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
        /// The culture corresponding to the entry
        /// </summary>
        [Required]
        [DynamicMaxLength(typeof(SiteConsts), nameof(SiteConsts.MaxCultureLength))]
        public string Culture { get; set; }


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


        /// <summary>
        /// Get the definition information of the field for data validation
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public override IReadOnlyList<FormField> GetFieldDefinitions(ValidationContext validationContext)
        {
            var _entryTypeAppService = validationContext.GetRequiredService<IEntryTypeAdminAppService>();
            var entryType = AsyncHelper.RunSync(()=> _entryTypeAppService.GetAsync(this.EntryTypeId));
            return entryType.FieldTabs.SelectMany(ft => 
                ft.Fields.Select(f =>
                    new FormField(
                        f.Field.Name,
                        f.DisplayName,
                        f.Field.Description,
                        f.Field.FormControlName,
                        f.Field.FormConfiguration,
                        f.Required,
                        this.GetField(f.Field.Name))
                    )).ToList();
        }
    }
}
