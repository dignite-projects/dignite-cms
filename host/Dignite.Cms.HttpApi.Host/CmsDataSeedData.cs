﻿using System;
using Volo.Abp.DependencyInjection;

namespace Dignite.Cms;

public class CmsDataSeedData : ISingletonDependency
{
    public Guid TextboxFieldId { get; } = Guid.NewGuid();
    public string TextboxFieldName = "TextboxFieldName";
    public Guid SwitchFieldId { get; } = Guid.NewGuid();
    public string SwitchFieldName = "switch";

    public Guid ImageFieldId { get; } = Guid.NewGuid();
    public string ImageFieldName = "image";

    public Guid BlogCategoryFieldId { get; } = Guid.NewGuid();
    public string BlogCategoryFieldName = "BlogCategory";
    public string BlogCategoryFieldItem1Value = "company-news";
    public string BlogCategoryFieldItem2Value = "tutorials";
    public string BlogCategoryFieldItem3Value = "essays";

    public Guid CkeditorFieldId { get; } = Guid.NewGuid();
    public string CkeditorFieldName = "Ckeditor";

    public Guid ServiceMatrixFieldId { get; }= Guid.NewGuid();
    public string ServiceMatrixFieldName = "Services";
    public string ServiceItemBlockName = "service-item";
    public string ServiceItemName = "name";
    public string ServiceItemDescription = "description";

    public Guid SiteId { get; } = Guid.NewGuid();
    public string SiteName = "SiteName";
    public string SiteHost = "https://localhost:44339";


    public Guid HomeSectionId { get; } = Guid.NewGuid();
    public string HomeSectionName = "home";
    public string HomeSectionRoute = "/";
    public string HomeSectionTemplate = "HomePage";
    public Guid BlogSectionId { get; } = Guid.NewGuid();
    public string BlogSectionName = "blog-index";
    public string BlogSectionRoute = "blog";
    public string BlogSectionTemplate = "Blog/Index";
    public Guid BlogPostSectionId { get; } = Guid.NewGuid();
    public string BlogPostSectionName = "blog-post";
    public string BlogPostSectionRoute = "blog/{publishTime:yyyy}/{publishTime:MM}/{slug}";
    public string BlogPostSectionTemplate = "Blog/Entry";
    public Guid ServiceSectionId { get; } = Guid.NewGuid();
    public string ServiceSectionName = "service";
    public string ServiceSectionRoute = "service/{slug}";
    public string ServiceEntryTemplate = "Service/Entry";

    public Guid HomeSectionEntryTypeId { get; } = Guid.NewGuid();
    public string HomeSectionEntryTypeName = "home";
    public Guid BlogSectionEntryTypeId { get; } = Guid.NewGuid();
    public string BlogSectionEntryTypeName = "blog-index";
    public Guid BlogPostSectionEntryTypeId { get; } = Guid.NewGuid();
    public string BlogPostSectionEntryTypeName = "blog-post";

    public Guid ServiceSectionEntryTypeId { get; } = Guid.NewGuid();
    public string ServiceSectionEntryTypeName = "service";

    public string EntryDefaultCulture = "en";
}
