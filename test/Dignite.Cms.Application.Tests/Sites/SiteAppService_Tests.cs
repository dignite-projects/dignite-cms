using System.Threading.Tasks;
using Dignite.Cms.Public.Sites;
using Shouldly;
using Xunit;

namespace Dignite.Cms.Samples;

public class SiteAppService_Tests : CmsApplicationTestBase
{
    private readonly ISitePublicAppService _sampleAppService;

    public SiteAppService_Tests()
    {
        _sampleAppService = GetRequiredService<ISitePublicAppService>();
    }

    [Fact]
    public async Task GetAsync()
    {
        /*
        var result = await _sampleAppService.GetAsync();
        result.Value.ShouldBe(42);
        */
    }

    [Fact]
    public async Task GetAuthorizedAsync()
    {
        /*
        var result = await _sampleAppService.GetAuthorizedAsync();
        result.Value.ShouldBe(42);
        */
    }
}
