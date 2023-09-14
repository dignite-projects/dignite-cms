﻿using System.Collections.Generic;
using Volo.Abp.Localization;

namespace Dignite.Cms.Public.Web.Components.RegionSwitch;

public class RegionSwitchViewComponentModel
{
    public LanguageInfo CurrentLanguage { get; set; }

    public IReadOnlyList<LanguageInfo> AllLanguages { get; set; }
}