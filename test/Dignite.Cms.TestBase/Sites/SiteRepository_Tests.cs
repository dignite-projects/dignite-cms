using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Modularity;
using Xunit;

namespace Dignite.Cms.Sites;


public abstract class SiteRepository_Tests<TStartupModule> : CmsTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
    private readonly CmsTestData testData;
    private readonly ISiteRepository siteRepository;

    protected SiteRepository_Tests()
    {
        testData = GetRequiredService<CmsTestData>();
        siteRepository = GetRequiredService<ISiteRepository>();
    }


    [Fact]
    public async Task NameExistsAsync_ShouldReturnTrue_WithExistingName()
    {
        var result = await siteRepository.NameExistsAsync(testData.SiteName);

        result.ShouldBeTrue();
    }

    [Fact]
    public async Task NameExistsAsync_ShouldReturnFalse_WithNonExistingName()
    {
        var nonExistingName = "any-other-name";

        var result = await siteRepository.NameExistsAsync(nonExistingName);

        result.ShouldBeFalse();
    }

    [Fact]
    public async Task HostExistsAsync_ShouldReturnTrue_WithExistingHost()
    {
        var result = await siteRepository.HostExistsAsync(testData.SiteHost);

        result.ShouldBeTrue();
    }

    [Fact]
    public async Task HostExistsAsync_ShouldReturnFalse_WithNonExistingHost()
    {
        var nonExistingHost = "https://any-other-host.com";

        var result = await siteRepository.HostExistsAsync(nonExistingHost);

        result.ShouldBeFalse();
    }

    [Fact]
    public async Task FindByNameAsync_ShouldWorkProperly_WithCorrectParameters()
    {
        var result = await siteRepository.FindByNameAsync(testData.SiteName);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(testData.SiteId);
        result.Name.ShouldBe(testData.SiteName);
    }

    [Fact]
    public async Task ShouldNotFindByNameAsync()
    {
        var nonExistingName = "absolutely-non-existing-name";
        var site = await siteRepository.FindByNameAsync(nonExistingName);

        site.ShouldBeNull();
    }


    [Fact]
    public async Task FindByHostAsync_ShouldWorkProperly_WithCorrectParameters()
    {
        var result = await siteRepository.FindByHostAsync(testData.SiteHost);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(testData.SiteId);
        result.Host.ShouldBe(testData.SiteHost);
    }

    [Fact]
    public async Task ShouldNotFindByHostAsync()
    {
        var nonExistingHost = "absolutely-non-existing-host";
        var site = await siteRepository.FindByHostAsync(nonExistingHost);

        site.ShouldBeNull();
    }

    [Fact]
    public async Task GetPagedListAsync_ShouldWorkProperly_WhileGetting10_WithoutSorting()
    {
        var result = await siteRepository.GetListAsync();

        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
        result.Count.ShouldBe(1);
    }
}
