using Dignite.Abp.AspNetCore.Mvc.UI.Theme.Pure;
using Dignite.Cms.Public.Web.Localization;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Volo.Abp.UI.Navigation;

namespace Dignite.Cms.Menus;

public class WebsitePublicMenuContributor : IMenuContributor
{
    private readonly IConfiguration _configuration;

    public WebsitePublicMenuContributor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
        else if (context.Menu.Name == PureMenus.SiteMap)
        {
            await ConfigureSiteMapMenuAsync(context);
        }
    }

    private Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<CmsPublicWebResource>();

        // Home
        context.Menu.AddItem(
            new ApplicationMenuItem(
                WebsitePublicMenus.HomePage,
                l["Menu:HomePage"],
                "~/",
                icon: "fa fa-home",
                order: 1
            )
        );

        // service items
        var servicesMenuItem = new ApplicationMenuItem(
                WebsitePublicMenus.Services,
                l["Menu:Services"]
            );
        servicesMenuItem.AddItem(new ApplicationMenuItem(WebsitePublicMenus.Services_WebDesign, l["Menu:WebDesign"], url: "~/service/web-design"));
        servicesMenuItem.AddItem(new ApplicationMenuItem(WebsitePublicMenus.Services_Ecommerce, l["Menu:eCommerce"], url: "~/service/ecommerce"));
        context.Menu.AddItem(servicesMenuItem);


        // blog
        context.Menu.AddItem(new ApplicationMenuItem(WebsitePublicMenus.Blog, l["Menu:Blog"], "~/blog"));



        return Task.CompletedTask;
    }

    private Task ConfigureSiteMapMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<CmsPublicWebResource>();
        var serviceGroupName = "Menu:Services";
        var blogGroupName = "Menu:Blog";
        var learn = "Menu:Learn";
        context.Menu.AddGroup(new ApplicationMenuGroup(learn, l[$"{learn}"]));
        context.Menu.AddGroup(new ApplicationMenuGroup(serviceGroupName, l[$"{serviceGroupName}"]));
        context.Menu.AddGroup(new ApplicationMenuGroup(blogGroupName, l[$"{blogGroupName}"]));

        //
        context.Menu.AddItem(
            new ApplicationMenuItem(
                "web-design",
                l["Menu:WebDesign"],
                "~/service/web-design",
                groupName: serviceGroupName
            )
        );
        context.Menu.AddItem(
            new ApplicationMenuItem(
                "ecommerce",
                l["Menu:eCommerce"],
                "~/service/ecommerce",
                groupName: serviceGroupName
            )
        );


        /* Blog */
        context.Menu.AddItem(
            new ApplicationMenuItem(
                "blog",
                l["Menu:Blog-All"],
                "~/blog",
                groupName: blogGroupName
            )
        );
        context.Menu.AddItem(
            new ApplicationMenuItem(
                "blog-company-news",
                l["Menu:Blog-company-news"],
                "~/blog?category=company-news",
                groupName: blogGroupName
            )
        );
        context.Menu.AddItem(
            new ApplicationMenuItem(
                "blog-tutorials",
                l["Menu:Blog-tutorials"],
                "~/blog?category=tutorials",
                groupName: blogGroupName
            )
        );
        context.Menu.AddItem(
            new ApplicationMenuItem(
                "blog-essays",
                l["Menu:Blog-essays"],
                "~/blog?category=essays",
                groupName: blogGroupName
            )
        );

        //
        context.Menu.AddItem(
            new ApplicationMenuItem(
                "Home",
                l["Menu:HomePage"],
                "~/",
                groupName: learn
            )
        );

        return Task.CompletedTask;
    }
}
