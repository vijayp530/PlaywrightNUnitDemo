using Microsoft.Playwright;
using PlaywrightNUnitDemo.Utilities;

namespace PlaywrightNUnitDemo.Pages;

public class LoginPage
{
    private readonly IPage _page;

    public LoginPage(IPage page)
    {
        _page = page;
    }

    public async Task OpenHomePageAsync(string url)
    {
        await PageHelper.WaitForNavigationAsync(_page, () => _page.GotoAsync(url));
    }

    public async Task<string> GetPageTitleAsync()
    {
        return await PageHelper.WaitForValueAsync(_page, () => _page.TitleAsync());
    }
}
