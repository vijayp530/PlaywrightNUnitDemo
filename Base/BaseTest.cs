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

        await Context.Tracing.StartAsync(new()
            {
                Screenshots = true,
                Snapshots = true,
                Sources = true
            });  

        Page = await Context.NewPageAsync();
    }

    [TearDown]
    public async Task TearDown()
    {
        if (Context is null)
            return;

        var currentContext = TestContext.CurrentContext;
        var status = currentContext.Result.Outcome.Status;

        if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {   
            Directory.CreateDirectory("test-results");
           await Context.Tracing.StopAsync(new()
            {
                Path = Path.Combine("test-results",
                $"{currentContext.Test.Name}-trace.zip")
            });
        }
        else
        {
            await Context.Tracing.StopAsync();
        }

        await Context.CloseAsync();
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
