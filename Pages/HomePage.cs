using Microsoft.Playwright;
using PlaywrightNUnitDemo.Utilities;

namespace PlaywrightNUnitDemo.Pages;

public class HomePage
{
    private readonly IPage _page;

    public HomePage(IPage page)
    {
        _page = page;
    }

    public async Task OpenAsync(string url)
    {
        await PageHelper.WaitForNavigationAsync(_page, () => _page.GotoAsync(url));
    }

    public async Task<string> GetTitleAsync()
    {
        return await PageHelper.WaitForValueAsync(_page, () => _page.TitleAsync());
    }
}
