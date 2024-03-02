using Blazorise;
using Dignite.Cms.Admin.Sections;
using Dignite.Cms.Admin.Sites;
using Dignite.Cms.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dignite.Cms.Admin.Blazor.Components.DynamicForms.Entry
{
    public partial class EntryFormConfigurationComponent
    {
        private readonly ISiteAdminAppService _siteAdminAppService;
        private readonly ISectionAdminAppService _sectionAdminAppService;

        protected IReadOnlyList<SiteDto> AllSites { get; set; } = new List<SiteDto>();
        protected IReadOnlyList<SectionDto> Sections { get; set; } = new List<SectionDto>();
        protected SiteDto CurrentSite { get; set; } = new();


        public EntryFormConfigurationComponent(ISiteAdminAppService siteAdminAppService, ISectionAdminAppService sectionAdminAppService)
        {
            LocalizationResource = typeof(CmsResource);
            _siteAdminAppService = siteAdminAppService;
            _sectionAdminAppService = sectionAdminAppService;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            AllSites = (await _siteAdminAppService.GetListAsync(new GetSitesInput())).Items;
            if (FormConfiguration.SectionId!= Guid.Empty)
            {
                var section = await _sectionAdminAppService.GetAsync(FormConfiguration.SectionId);
                CurrentSite = section.Site;
                Sections = (await _sectionAdminAppService.GetListAsync(
                    new GetSectionsInput()
                    {
                        SiteId = CurrentSite.Id,
                        MaxResultCount = 1000
                    })).Items;
            }
            else
            {
                if (AllSites.Any())
                {
                    await OnSiteChangedAsync(
                        AllSites.
                        OrderBy(s => s.CreationTime)
                        .First()
                        .Id);
                }
            }
        }

        protected async Task OnSiteChangedAsync(Guid siteId)
        {
            CurrentSite = AllSites.First(s => s.Id == siteId);
            Sections = (await _sectionAdminAppService.GetListAsync(
                new GetSectionsInput()
                {
                    SiteId = siteId,
                    MaxResultCount = 1000
                })).Items;

            if (Sections.Any())
            {
                FormConfiguration.SectionId = Sections
                .OrderByDescending(s => s.IsActive)
                .ThenByDescending(s => s.IsDefault)
                .First().Id;
            }
            else
            {
                FormConfiguration.SectionId = Guid.Empty;
            }
        }

        private void SectionSelectedValidator(ValidatorEventArgs e)
        {
            if (FormConfiguration.SectionId != Guid.Empty)
            {
                e.Status = ValidationStatus.Success;
            }
            else
            {
                e.Status = ValidationStatus.Error;
            }
        }
    }
}
