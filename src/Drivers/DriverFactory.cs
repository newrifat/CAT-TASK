using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using DarazAutomation.Config;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace DarazAutomation.Drivers
{
    /// <summary>
    /// Factory class for creating WebDriver instances
    /// Supports multiple browsers and configurations
    /// Uses WebDriverManager for automatic driver management
    /// </summary>
    public static class DriverFactory
    {
        /// <summary>
        /// Creates a new WebDriver instance based on configuration
        /// </summary>
        public static IWebDriver CreateDriver()
        {
            return CreateDriver(ConfigurationManager.Browser);
        }

        /// <summary>
        /// Creates a new WebDriver instance for the specified browser
        /// </summary>
        public static IWebDriver CreateDriver(string browserName)
        {
            IWebDriver driver = browserName.ToLower() switch
            {
                "chrome" => CreateChromeDriver(),
                _ => throw new ArgumentException($"Browser '{browserName}' is not supported.")
            };

            ConfigureDriver(driver);
            return driver;
        }

        private static IWebDriver CreateChromeDriver()
        {
            // Automatically download and setup ChromeDriver matching the installed Chrome version
            new DriverManager().SetUpDriver(new ChromeConfig());

            var options = new ChromeOptions();
            
            if (ConfigurationManager.Headless)
            {
                options.AddArgument("--headless=new");
            }
            
            // Use incognito mode to ensure clean session (no cookies/cache from previous runs)
            options.AddArgument("--incognito");
            
            // Common Chrome options for stability
            options.AddArgument("--disable-gpu");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-extensions");
            options.AddArgument("--disable-popup-blocking");
            options.AddArgument("--start-maximized");
            options.AddArgument("--disable-blink-features=AutomationControlled");
            options.AddExcludedArgument("enable-automation");
            options.AddUserProfilePreference("credentials_enable_service", false);
            options.AddUserProfilePreference("profile.password_manager_enabled", false);
            
            // Set language preference
            options.AddArgument("--lang=en-US");
            options.AddUserProfilePreference("intl.accept_languages", "en-US,en");

            return new ChromeDriver(options);
        }

        private static void ConfigureDriver(IWebDriver driver)
        {
            // Set timeouts
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(ConfigurationManager.ImplicitWaitSeconds);
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(ConfigurationManager.PageLoadTimeoutSeconds);
            
            // Maximize window if configured
            if (ConfigurationManager.MaximizeWindow)
            {
                driver.Manage().Window.Maximize();
            }
        }
    }
}
