using Blazorise;
using Dignite.Cms.Admin.Sites;
using Dignite.Cms.Localization;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Localization;

namespace Dignite.Cms.Admin.Blazor.Pages.Cms.Admin.Sites
{
    public partial class CreateOrUpdateSiteComponent
    {
        [Parameter] public CreateOrUpdateSiteInputBase Entity { get; set; }
        [Parameter] public IReadOnlyList<LanguageInfo> AllLanguages { get; set; }

        //Will not change again after assignment, used to verify that the site name already exists
        private string siteNameForValidation;
        private string siteHostForValidation;

        public CreateOrUpdateSiteComponent()
        {
            LocalizationResource = typeof(CmsResource);
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            siteNameForValidation = Entity.Name;
            siteHostForValidation = Entity.Host;
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        void OnCultureChanged(string cultureName, bool value)
        {
            if (value)
            {
                Entity.Languages.Add(new SiteLanguageInput(false, cultureName));
            }
            else
            {
                Entity.Languages.RemoveAll(l => l.CultureName == cultureName);
            }
        }

        void OnSetDefault(string cultureName)
        {
            foreach (var sl in Entity.Languages)
            {
                if (sl.CultureName == cultureName)
                {
                    sl.IsDefault = true;
                }
                else
                {
                    sl.IsDefault = false;
                }
            }
        }

        private async Task NameExistsValidatorAsync(ValidatorEventArgs e, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var name = Convert.ToString(e.Value);
            if (!name.IsNullOrEmpty())
            {
                if (!name.Equals(siteNameForValidation, StringComparison.InvariantCultureIgnoreCase))
                {
                    e.Status = await _siteAdminAppService.NameExistsAsync(name)
                        ? ValidationStatus.Error
                        : ValidationStatus.Success;

                    e.ErrorText = L["SiteName{0}AlreadyExist", name];
                }
            }
            else
            {
                e.Status = ValidationStatus.Error;
            }
        }
        private async Task HostExistsValidatorAsync(ValidatorEventArgs e, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var host = Convert.ToString(e.Value);
            if (!host.IsNullOrEmpty())
            {
                if (!host.Equals(siteHostForValidation, StringComparison.InvariantCultureIgnoreCase))
                {
                    e.Status = await _siteAdminAppService.HostExistsAsync(host)
                        ? ValidationStatus.Error
                        : ValidationStatus.Success;

                    e.ErrorText = L["SiteHost{0}AlreadyExist", host];
                }
            }
            else
            {
                e.Status = ValidationStatus.Error;
            }
        }
        

        void DisplayNameTextboxBlur()
        {
            if (!Entity.DisplayName.IsNullOrEmpty() && Entity.Name.IsNullOrEmpty())
            {
                Entity.Name = SlugNormalizer.Normalize(Entity.DisplayName);
            }
        }
    }
}
