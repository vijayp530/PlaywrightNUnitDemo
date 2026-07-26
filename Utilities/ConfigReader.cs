using System.Text.Json;

namespace PlaywrightNUnitDemo.Utilities;

public static class ConfigReader
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public static string GetBaseUrl()
    {
        var envUrl = Environment.GetEnvironmentVariable("BASE_URL");
        if (!string.IsNullOrWhiteSpace(envUrl))
        {
            return envUrl;
        }

        return GetSettings().BaseUrl;
    }

    public static string GetBrowser() => GetSettings().Browser;

    public static bool GetHeadless() => GetSettings().Headless;

    public static bool GetStartMaximized() => GetSettings().StartMaximized;

    public static int GetViewportWidth() => GetSettings().ViewportWidth;

    public static int GetViewportHeight() => GetSettings().ViewportHeight;

    public static int GetTimeoutSeconds() => GetSettings().TimeoutSeconds;

    private static TestSettings GetSettings()
    {
        var configPath = FindConfigFile();
        if (string.IsNullOrWhiteSpace(configPath))
        {
            return new TestSettings();
        }

        using var stream = File.OpenRead(configPath);
        var settings = JsonSerializer.Deserialize<TestSettings>(stream, JsonOptions);
        return settings ?? new TestSettings();
    }

    private static string? FindConfigFile()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null)
        {
            var candidate = Path.Combine(directory.FullName, "TestData", "appsettings.json");
            if (File.Exists(candidate))
            {
                return candidate;
            }

            directory = directory.Parent;
        }

        return null;
    }

    private sealed class TestSettings
    {
        public string BaseUrl { get; set; } = "https://www.google.com";
        public string Browser { get; set; } = "chromium";
        public bool Headless { get; set; }
        public bool StartMaximized { get; set; } = true;
        public int ViewportWidth { get; set; } = 1920;
        public int ViewportHeight { get; set; } = 1080;
        public int TimeoutSeconds { get; set; } = 60;
    }
}
