using Dignite.Cms.Admin.Sites;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Xunit;

namespace Dignite.Cms.Sites;

public class SiteAdminAppService_Tests : CmsApplicationTestBase
{
    private readonly ISiteAdminAppService siteAdminAppService;
    private readonly CmsTestData testData;

    public SiteAdminAppService_Tests()
    {
        siteAdminAppService = GetRequiredService<ISiteAdminAppService>();
        testData = GetRequiredService<CmsTestData>();
    }


    [Fact]
    public async Task GetAsync()
    {
        var site = await siteAdminAppService.GetAsync(testData.SiteId);

        site.Name.ShouldBe(testData.SiteName);
        site.Host.ShouldBe(testData.SiteHost);
        site.Languages.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task GetListAsync()
    {
        var sites = await siteAdminAppService.GetListAsync(new GetSitesInput());

        sites.TotalCount.ShouldBeGreaterThan(0);
        sites.Items.Any(x => x.Name == testData.SiteName).ShouldBeTrue();
    }

    [Fact]
    public async Task NameExistsAsync()
    {
        var result = await siteAdminAppService.NameExistsAsync(testData.SiteName);

        result.ShouldBe(true);
    }

    [Fact]
    public async Task HostExistsAsync()
    {
        var result = await siteAdminAppService.HostExistsAsync(testData.SiteHost);

        result.ShouldBe(true);
    }

    [Fact]
    public async Task CreateAsync_ShouldWork()
    {
        var name = "new-site";
        var host = "https://new-site.com";
        var site = await siteAdminAppService.CreateAsync(new CreateSiteInput
        {
            Name = name,
            DisplayName="New site",
            Host= host,
            IsActive=true,
            Languages=new List<SiteLanguageInput> { 
                new SiteLanguageInput(true,"en"),
                new SiteLanguageInput(false,"fr"),
                new SiteLanguageInput(false,"ja")
            }
        });

        site.ShouldNotBeNull();
        site.Name.ShouldBe(name);
        site.Host.ShouldBe(host);
        site.Languages.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WithExistName()
    {
        await Should.ThrowAsync<SiteNameAlreadyExistException>(
            async () =>
                await siteAdminAppService.CreateAsync(new CreateSiteInput
                {
                    Name = testData.SiteName,
                    DisplayName = "New site",
                    Host = "https://new-site1.com",
                    IsActive = true,
                    Languages = new List<SiteLanguageInput> {
                        new SiteLanguageInput(true,"en"),
                        new SiteLanguageInput(false,"fr"),
                        new SiteLanguageInput(false,"ja")
                    }
                })
            );
    }

    [Fact]
    public async Task CreateAsync_ShouldThrow_WithExistHost()
    {
        await Should.ThrowAsync<SiteHostAlreadyExistException>(
            async () =>
                await siteAdminAppService.CreateAsync(new CreateSiteInput
                {
                    Name = "new-site-1",
                    DisplayName = "New site",
                    Host = testData.SiteHost,
                    IsActive = true,
                    Languages = new List<SiteLanguageInput> {
                        new SiteLanguageInput(true,"en"),
                        new SiteLanguageInput(false,"fr"),
                        new SiteLanguageInput(false,"ja")
                    }
                })
            );
    }

    [Fact]
    public async Task UpdateAsync_ShouldWork()
    {
        var newDisplayName = "New Display Name";
        var site = await siteAdminAppService.UpdateAsync(
            testData.SiteId, 
            new UpdateSiteInput
            {
                DisplayName = newDisplayName,
                Name = testData.SiteName,
                Host = testData.SiteHost,
                IsActive = true,
                Languages = new List<SiteLanguageInput> {
                        new SiteLanguageInput(true,"en"),
                        new SiteLanguageInput(false,"fr"),
                        new SiteLanguageInput(false,"ja")
                    }
            });

        var updatedSite = await siteAdminAppService.GetAsync(testData.SiteId);

        updatedSite.DisplayName.ShouldBe(newDisplayName);
    }

    [Fact]
    public async Task DeleteAsync_ShouldWork()
    {
        await siteAdminAppService.DeleteAsync(testData.SiteId);

        await Should.ThrowAsync<EntityNotFoundException>(
            async () =>
                await siteAdminAppService.GetAsync(testData.SiteId)
        );
    }
}
