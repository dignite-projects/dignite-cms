
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Dignite.Cms.Public.Entries
{
    public class GetEntriesInput : PagedResultRequestDto
    {
        public GetEntriesInput()
        {
            MaxResultCount = 20;
        }

        [Required]
        public Guid SectionId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Filter { get; set; }

        public Guid? CreatorId { get; set; }

        public DateTime? StartPublishDate { get; set; }

        public DateTime? ExpiryPublishDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string QueryingByFieldParameters { get; set; }
    }
}
