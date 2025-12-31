using Microsoft.Extensions.Configuration;

namespace DarazAutomation.Config
{
    /// <summary>
    /// Configuration manager with environment variable support and hierarchical configuration structure.
    /// </summary>
    public static class ConfigurationManager
    {
        private static IConfiguration? _configuration;
        
        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
                    
                    _configuration = new ConfigurationBuilder()
                        .SetBasePath(configPath)
                        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .Build();
                }
                return _configuration;
            }
        }

        #region Application Settings
        
        public static string ApplicationName => GetConfigValue("ApplicationSettings:ApplicationName", "Daraz E-Commerce Automation Suite");
        public static string Version => GetConfigValue("ApplicationSettings:Version", "1.0.0");
        public static string Environment => GetConfigValue("ApplicationSettings:Environment", "Test");
        
        #endregion

        #region WebDriver Settings
        
        public static string Browser => GetConfigValue("WebDriver:Browser", "Chrome");
        public static string BrowserVersion => GetConfigValue("WebDriver:BrowserVersion", "latest");
        public static bool Headless => GetBoolValue("WebDriver:Headless", false);
        
        public static int WindowWidth => GetIntValue("WebDriver:WindowSize:Width", 1920);
        public static int WindowHeight => GetIntValue("WebDriver:WindowSize:Height", 1080);
        public static bool MaximizeWindow => GetBoolValue("WebDriver:WindowSize:Maximize", true);
        
        public static int ImplicitWaitSeconds => GetIntValue("WebDriver:Timeouts:ImplicitWait", 10);
        public static int ExplicitWaitSeconds => GetIntValue("WebDriver:Timeouts:ExplicitWait", 30);
        public static int PageLoadTimeoutSeconds => GetIntValue("WebDriver:Timeouts:PageLoad", 90);
        public static int ScriptTimeoutSeconds => GetIntValue("WebDriver:Timeouts:Script", 30);
        
        public static string[] BrowserArguments => GetStringArray("WebDriver:Arguments");
        
        #endregion

        #region Application URLs
        
        public static string BaseUrl => GetConfigValue("Application:BaseUrl", "https://www.daraz.com.bd");
        public static string HomeEndpoint => GetConfigValue("Application:Endpoints:Home", "/");
        public static string LoginEndpoint => GetConfigValue("Application:Endpoints:Login", "/customer/account/login");
        public static string CartEndpoint => GetConfigValue("Application:Endpoints:Cart", "/cart");
        public static string CheckoutEndpoint => GetConfigValue("Application:Endpoints:Checkout", "/checkout/shipping");
        
        public static string HomeUrl => $"{BaseUrl}{HomeEndpoint}";
        public static string LoginUrl => $"{BaseUrl}{LoginEndpoint}";
        public static string CartUrl => $"{BaseUrl}{CartEndpoint}";
        public static string CheckoutUrl => $"{BaseUrl}{CheckoutEndpoint}";
        
        #endregion

        #region Test Execution Settings
        
        public static int RetryCount => GetIntValue("TestExecution:RetryCount", 0);
        public static bool ParallelExecution => GetBoolValue("TestExecution:ParallelExecution", false);
        public static int MaxDegreeOfParallelism => GetIntValue("TestExecution:MaxDegreeOfParallelism", 1);
        public static bool CaptureScreenshots => GetBoolValue("TestExecution:CaptureScreenshots", true);
        public static bool ScreenshotOnFailureOnly => GetBoolValue("TestExecution:ScreenshotOnFailureOnly", true);
        public static bool VideoRecording => GetBoolValue("TestExecution:VideoRecording", false);
        public static bool DetailedLogging => GetBoolValue("TestExecution:DetailedLogging", true);
        
        #endregion

        #region Reporting Settings
        
        public static string ReportsPath => GetConfigValue("Reporting:ReportsPath", "Reports");
        public static bool ExtentReportsEnabled => GetBoolValue("Reporting:ExtentReports:Enabled", true);
        public static string ExtentReportName => GetConfigValue("Reporting:ExtentReports:ReportName", "DarazAutomationReport");
        public static string ExtentReportTitle => GetConfigValue("Reporting:ExtentReports:DocumentTitle", "Daraz E-Commerce Test Results");
        public static string ExtentReportTheme => GetConfigValue("Reporting:ExtentReports:Theme", "dark");
        
        public static string ScreenshotsPath => GetConfigValue("Reporting:Screenshots:Path", "Reports/Screenshots");
        public static string ScreenshotFormat => GetConfigValue("Reporting:Screenshots:Format", "png");
        public static int ScreenshotQuality => GetIntValue("Reporting:Screenshots:Quality", 90);
        
        #endregion

        #region Localization Settings
        
        public static string DefaultLanguage => GetConfigValue("Localization:DefaultLanguage", "English");
        
        public static string EnglishText => GetConfigValue("Localization:Languages:English:DisplayName", "English");
        public static string EnglishCode => GetConfigValue("Localization:Languages:English:Code", "en");
        public static string EnglishVerificationText => GetConfigValue("Localization:Languages:English:VerificationText", "Categories");
        public static string EnglishLocale => GetConfigValue("Localization:Languages:English:LocaleCode", "en-BD");
        
        public static string BanglaText => GetConfigValue("Localization:Languages:Bangla:DisplayName", "বাংলা");
        public static string BanglaCode => GetConfigValue("Localization:Languages:Bangla:Code", "bn");
        public static string BanglaVerificationText => GetConfigValue("Localization:Languages:Bangla:VerificationText", "বিভাগসমূহ");
        public static string BanglaLocale => GetConfigValue("Localization:Languages:Bangla:LocaleCode", "bn-BD");
        
        #endregion

        #region Authentication Settings
        
        public static string LoginMethod => GetConfigValue("Authentication:Method", "Password");
        
        public static string LoginEmail => 
            System.Environment.GetEnvironmentVariable("DARAZ_LOGIN_EMAIL") 
            ?? GetConfigValue("Authentication:Credentials:Email", "your-email@example.com");
        
        public static string LoginPhone => 
            System.Environment.GetEnvironmentVariable("DARAZ_LOGIN_PHONE") 
            ?? GetConfigValue("Authentication:Credentials:Phone", "+880170000000");
        
        public static string LoginPassword => 
            System.Environment.GetEnvironmentVariable("DARAZ_LOGIN_PASSWORD") 
            ?? GetConfigValue("Authentication:Credentials:Password", "YourSecurePassword123!");
        
        public static bool ReuseSession => GetBoolValue("Authentication:SessionManagement:ReuseSession", true);
        public static int SessionTimeout => GetIntValue("Authentication:SessionManagement:SessionTimeout", 3600);
        
        #endregion

        #region Logging Settings
        
        public static string LogLevel => GetConfigValue("Logging:LogLevel:Default", "Information");
        public static bool ConsoleLoggingEnabled => GetBoolValue("Logging:Console:Enabled", true);
        public static bool IncludeTimestamp => GetBoolValue("Logging:Console:IncludeTimestamp", true);
        public static bool FileLoggingEnabled => GetBoolValue("Logging:File:Enabled", true);
        public static string LogPath => GetConfigValue("Logging:File:Path", "Logs");
        public static string LogFileName => GetConfigValue("Logging:File:FileName", "test-execution-{Date}.log");
        public static string LogRollingInterval => GetConfigValue("Logging:File:RollingInterval", "Day");
        public static int LogRetainedFileCount => GetIntValue("Logging:File:RetainedFileCountLimit", 7);
        
        #endregion

        #region Helper Methods
        
        /// <summary>
        /// Gets configuration value with environment variable override support.
        /// Format: DARAZ_{SECTION}_{KEY} (e.g., DARAZ_WEBDRIVER_BROWSER)
        /// </summary>
        private static string GetConfigValue(string key, string defaultValue)
        {
            string envKey = $"DARAZ_{key.Replace(":", "_").Replace(".", "_").ToUpper()}";
            string? envValue = System.Environment.GetEnvironmentVariable(envKey);
            
            if (!string.IsNullOrEmpty(envValue))
            {
                return envValue;
            }
            
            return Configuration[key] ?? defaultValue;
        }
        
        private static int GetIntValue(string key, int defaultValue)
        {
            var value = Configuration[key];
            return int.TryParse(value, out int result) ? result : defaultValue;
        }
        
        private static bool GetBoolValue(string key, bool defaultValue)
        {
            var value = Configuration[key];
            return bool.TryParse(value, out bool result) ? result : defaultValue;
        }
        
        private static string[] GetStringArray(string key)
        {
            var section = Configuration.GetSection(key);
            if (!section.Exists())
            {
                return Array.Empty<string>();
            }
            
            var list = new List<string>();
            foreach (var child in section.GetChildren())
            {
                var value = child.Value;
                if (!string.IsNullOrEmpty(value))
                {
                    list.Add(value);
                }
            }
            return list.ToArray();
        }
        
        public static void ReloadConfiguration()
        {
            _configuration = null;
        }
        
        #endregion
    }
}
