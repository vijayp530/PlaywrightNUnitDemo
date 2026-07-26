using Microsoft.Playwright;
using NUnit.Framework;
using PlaywrightNUnitDemo.Utilities;

namespace PlaywrightNUnitDemo.Base;

[TestFixture]
public class BaseTest
{
    protected IBrowser? Browser;
    protected IPlaywright? PlaywrightInstance;
    protected IBrowserContext? Context;
    protected IPage? Page;

    [OneTimeSetUp]
    public async Task OneTimeSetup()
    {
        PlaywrightInstance = await Playwright.CreateAsync();
        var launchOptions = new BrowserTypeLaunchOptions
        {
            Headless = ConfigReader.GetHeadless(),
            Args = ConfigReader.GetStartMaximized() ? new[] { "--start-maximized" } : Array.Empty<string>()
        };

        Browser = await PlaywrightInstance.Chromium.LaunchAsync(launchOptions);
    }

    [SetUp]
    public async Task Setup()
    {
        Context = await Browser!.NewContextAsync(
            new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize
                {
                    Width = ConfigReader.GetViewportWidth(),
                    Height = ConfigReader.GetViewportHeight()
                }
            });
        Page = await Context.NewPageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        if (Context is not null)
        {
            await Context.CloseAsync();
        }
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (Browser is not null)
        {
            await Browser.CloseAsync();
        }

        PlaywrightInstance?.Dispose();
    }
}
