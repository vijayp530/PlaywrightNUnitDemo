using Microsoft.Playwright;

namespace PlaywrightNUnitDemo.Utilities;

public static class PageHelper
{
    public static async Task WaitForNavigationAsync(IPage page, Func<Task> action)
    {
        var timeoutSeconds = ConfigReader.GetTimeoutSeconds();
        await page.WaitForLoadStateAsync(LoadState.Load, new PageWaitForLoadStateOptions { Timeout = timeoutSeconds });
        await action();
        await page.WaitForLoadStateAsync(LoadState.Load, new PageWaitForLoadStateOptions { Timeout = timeoutSeconds });
    }

    public static async Task<T> WaitForValueAsync<T>(IPage page, Func<Task<T>> action)
    {
        var timeoutSeconds = ConfigReader.GetTimeoutSeconds();
        await page.WaitForLoadStateAsync(LoadState.Load, new PageWaitForLoadStateOptions { Timeout = timeoutSeconds });
        return await action();
    }
}
