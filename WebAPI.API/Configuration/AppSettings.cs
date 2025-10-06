namespace WebAPI.API.Configuration;

public class ApiSettings
{
    public const string SectionName = "ApiSettings";

    public string Title { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ContactName { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string LicenseName { get; set; } = string.Empty;
    public bool EnableSwagger { get; set; } = true;
    public bool EnableCors { get; set; } = true;
    public string CorsPolicy { get; set; } = "AllowAll";
}

public class DatabaseSettings
{
    public const string SectionName = "DatabaseSettings";

    public int CommandTimeout { get; set; } = 30;
    public bool EnableSensitiveDataLogging { get; set; } = false;
    public bool EnableDetailedErrors { get; set; } = false;
    public int MaxRetryCount { get; set; } = 3;
    public TimeSpan MaxRetryDelay { get; set; } = TimeSpan.FromSeconds(30);
}