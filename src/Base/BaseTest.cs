using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using DarazAutomation.Config;
using DarazAutomation.Drivers;
using DarazAutomation.Utilities;

namespace DarazAutomation.Base
{
    /// <summary>
    /// Base class for all test classes
    /// Handles WebDriver lifecycle, setup, and teardown
    /// Loads .env file for environment variable configuration
    /// </summary>
    [TestFixture]
    public abstract class BaseTest
    {
        protected static IWebDriver? SharedDriver { get; private set; }
        protected IWebDriver Driver { get; private set; } = null!;
        protected ExtentReportManager ReportManager { get; private set; } = null!;
        protected virtual bool ReuseSession => false;

        /// <summary>
        /// Static constructor to load .env file before any tests run
        /// </summary>
        static BaseTest()
        {
            // Load .env file if it exists (optional, falls back to environment variables or appsettings.json)
            DotEnvLoader.Load();
        }

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            ReportManager = new ExtentReportManager();
            ReportManager.InitializeReport(GetType().Name);
            
            if (ReuseSession && SharedDriver == null)
            {
                SharedDriver = DriverFactory.CreateDriver();
                LogInfo($"Shared browser session created: {ConfigurationManager.Browser}");
            }
        }

        [SetUp]
        public virtual void SetUp()
        {
            ReportManager.CreateTest(TestContext.CurrentContext.Test.Name);
            
            if (ReuseSession && SharedDriver != null)
            {
                Driver = SharedDriver;
                LogInfo($"Reusing existing browser session");
            }
            else
            {
                Driver = DriverFactory.CreateDriver();
                LogInfo($"Browser launched: {ConfigurationManager.Browser}");
            }
            
            // Always navigate to base URL at the start of each test
            // This ensures a consistent starting point for all tests
            NavigateToBaseUrl();
        }

        [TearDown]
        public virtual void TearDown()
        {
            var testStatus = TestContext.CurrentContext.Result.Outcome.Status;
            var testMessage = TestContext.CurrentContext.Result.Message;

            switch (testStatus)
            {
                case TestStatus.Failed:
                    LogFail($"Test Failed: {testMessage}");
                    if (ConfigurationManager.ScreenshotOnFailureOnly && Driver != null)
                    {
                        var screenshotPath = ScreenshotHelper.CaptureScreenshot(Driver, TestContext.CurrentContext.Test.Name);
                        ReportManager.AddScreenshotToReport(screenshotPath);
                    }
                    break;
                case TestStatus.Passed:
                    LogPass("Test Passed");
                    break;
                case TestStatus.Skipped:
                    LogWarning($"Test Skipped: {testMessage}");
                    break;
            }

            if (!ReuseSession && Driver != null)
            {
                Driver.Quit();
                Driver.Dispose();
            }
        }

        [OneTimeTearDown]
        public virtual void OneTimeTearDown()
        {
            ReportManager.FlushReport();
            
            if (ReuseSession && SharedDriver != null)
            {
                SharedDriver.Quit();
                SharedDriver.Dispose();
                SharedDriver = null;
                LogInfo("Shared browser session closed");
            }
        }

        #region Logging Helper Methods

        protected void LogInfo(string message)
        {
            TestContext.WriteLine($"[INFO] {DateTime.Now:HH:mm:ss} - {message}");
            ReportManager.LogInfo(message);
        }

        protected void LogPass(string message)
        {
            TestContext.WriteLine($"[PASS] {DateTime.Now:HH:mm:ss} - {message}");
            ReportManager.LogPass(message);
        }

        protected void LogFail(string message)
        {
            TestContext.WriteLine($"[FAIL] {DateTime.Now:HH:mm:ss} - {message}");
            ReportManager.LogFail(message);
        }

        protected void LogWarning(string message)
        {
            TestContext.WriteLine($"[WARN] {DateTime.Now:HH:mm:ss} - {message}");
            ReportManager.LogWarning(message);
        }

        protected void LogStep(string stepDescription)
        {
            TestContext.WriteLine($"[STEP] {DateTime.Now:HH:mm:ss} - {stepDescription}");
            ReportManager.LogInfo($"Step: {stepDescription}");
        }

        #endregion

        #region Navigation Helper

        protected void NavigateToBaseUrl()
        {
            Driver.Navigate().GoToUrl(ConfigurationManager.BaseUrl);
            LogInfo($"Navigated to {ConfigurationManager.BaseUrl}");
        }

        #endregion
    }
}
