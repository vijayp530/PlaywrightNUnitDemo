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
    Console.WriteLine("====== TearDown Started ======");

    if (Context is null)
    {
        Console.WriteLine("Context is null");
        return;
    }

    var currentContext = TestContext.CurrentContext;
    var status = currentContext.Result.Outcome.Status;

    Console.WriteLine($"Status : {status}");
    Console.WriteLine($"Working Directory : {Directory.GetCurrentDirectory()}");

    try
    {
        if (status == NUnit.Framework.Interfaces.TestStatus.Failed)
        {
            var folder = Path.Combine(Directory.GetCurrentDirectory(), "test-results");

            Directory.CreateDirectory(folder);

            var traceFile = Path.Combine(folder, $"{currentContext.Test.Name}-trace.zip");

            Console.WriteLine($"Saving trace : {traceFile}");

            await Context.Tracing.StopAsync(new()
            {
                Path = traceFile
            });

            Console.WriteLine("Trace saved successfully");
        }
        else
        {
            await Context.Tracing.StopAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
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
