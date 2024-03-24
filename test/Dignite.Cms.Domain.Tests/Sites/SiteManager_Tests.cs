using Shouldly;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Dignite.Cms.Sites;

public class SiteManager_Tests : CmsDomainTestBase
{
    private readonly CmsTestData testData;
    private readonly SiteManager siteManager;
    private readonly ISiteRepository siteRepository;

    public SiteManager_Tests()
    {
        siteManager = GetRequiredService<SiteManager>();
        testData = GetRequiredService<CmsTestData>();
        siteRepository = GetRequiredService<ISiteRepository>();
    }


    [Fact]
    public async Task CreateAsync_ShouldWorkProperly()
    {
        var languages = new List<SiteLanguage> {
            new SiteLanguage(true, "en"),
            new SiteLanguage(false, "ja"),
            new SiteLanguage(false, "zh-Hant")
        };

        var site = await siteManager.CreateAsync(
            "New Site",
            "new-site-name",
            "https://new-site.com",
            true,
            languages,
            null
        );

        site.Id.ShouldNotBe(Guid.Empty);

        var siteFromDb = await siteRepository.GetAsync(site.Id);

        siteFromDb.Name.ShouldBe(site.Name);
        siteFromDb.DisplayName.ShouldBe(site.DisplayName);
        siteFromDb.Host.ShouldBe(site.Host);
        siteFromDb.Languages.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WithNonExistingName()
    {
        var languages = new List<SiteLanguage> {
            new SiteLanguage(true, "en"),
            new SiteLanguage(false, "ja"),
            new SiteLanguage(false, "zh-Hant")
        };

        var exception = await Should.ThrowAsync<SiteNameAlreadyExistException>(
            async () => await siteManager.CreateAsync(
                "New Site",
                testData.SiteName,
                "https://new-site1.com",
                true,
                languages,
                null
            ));

        exception.ShouldNotBeNull();
        exception.Code.ShouldBe(CmsErrorCodes.Sites.NameAlreadyExist);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WithNonExistingHost()
    {
        var languages = new List<SiteLanguage> {
            new SiteLanguage(true, "en"),
            new SiteLanguage(false, "ja"),
            new SiteLanguage(false, "zh-Hant")
        };

        var exception = await Should.ThrowAsync<SiteHostAlreadyExistException>(
            async () => await siteManager.CreateAsync(
                "New Site",
                "new-site1",
                testData.SiteHost,
                true,
                languages,
                null
            ));

        exception.ShouldNotBeNull();
        exception.Code.ShouldBe(CmsErrorCodes.Sites.HostAlreadyExist);
    }
}
