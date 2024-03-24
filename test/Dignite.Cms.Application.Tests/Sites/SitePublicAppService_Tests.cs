using System.Threading.Tasks;
using Dignite.Cms.Public.Sites;
using Shouldly;
using Xunit;

namespace Dignite.Cms.Sites;

public class SitePublicAppService_Tests : CmsApplicationTestBase
{
    private readonly ISitePublicAppService sitePublicAppService;
    private readonly CmsTestData testData;

    public SitePublicAppService_Tests()
    {
        sitePublicAppService = GetRequiredService<ISitePublicAppService>();
        testData = GetRequiredService<CmsTestData>();
    }

    [Fact]
    public async Task GetAsync_ShouldWorkProperly_WithExistingName()
    {
        var result = await sitePublicAppService.FindByNameAsync(testData.SiteName);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(testData.SiteId);
        result.Host.ShouldBe(testData.SiteHost);
        result.Languages.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task GetAsync_ShouldWorkProperly_WithExistingHost()
    {
        var result = await sitePublicAppService.FindByHostAsync(testData.SiteHost);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(testData.SiteId);
        result.Host.ShouldBe(testData.SiteHost);
        result.Languages.ShouldNotBeEmpty();
    }
}
