using NUnit.Framework;
using PlaywrightNUnitDemo.Base;
using PlaywrightNUnitDemo.Pages;
using PlaywrightNUnitDemo.Utilities;

namespace PlaywrightNUnitDemo.Tests;

public class LoginTests : BaseTest
{
    [Test]
    public async Task GoogleHomepageLoads()
    {
        var loginPage = new LoginPage(Page!);
        var baseUrl = ConfigReader.GetBaseUrl();

        await loginPage.OpenHomePageAsync(baseUrl);
        var title = await loginPage.GetPageTitleAsync();

        Assert.That(title, Does.Contain("Google"));
    }

    [Test]
    public async Task HomePageTitleContainsExpectedText()
    {
        var homePage = new HomePage(Page!);
        var baseUrl = ConfigReader.GetBaseUrl();

        await homePage.OpenAsync(baseUrl);
        var title = await homePage.GetTitleAsync();

        Assert.That(title, Does.Contain("Swag Labs"));
    }
}
