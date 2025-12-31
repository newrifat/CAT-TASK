using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;

namespace DarazAutomation.Utilities
{
    /// <summary>
    /// Manages Extent Reports for test reporting
    /// </summary>
    public class ExtentReportManager
    {
        private ExtentReports _extent = null!;
        private ExtentTest _test = null!;
        private static readonly string ReportsFolder = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Reports");

        /// <summary>
        /// Initializes the Extent Report
        /// </summary>
        public void InitializeReport(string reportName)
        {
            // Ensure reports directory exists
            if (!Directory.Exists(ReportsFolder))
            {
                Directory.CreateDirectory(ReportsFolder);
            }

            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var reportPath = Path.Combine(ReportsFolder, $"{reportName}_{timestamp}.html");

            var htmlReporter = new ExtentSparkReporter(reportPath);
            
            // Configure report
            htmlReporter.Config.DocumentTitle = "Daraz Automation Test Report";
            htmlReporter.Config.ReportName = "Language Change Test Report";
            htmlReporter.Config.Theme = Theme.Standard;
            htmlReporter.Config.Encoding = "UTF-8";

            _extent = new ExtentReports();
            _extent.AttachReporter(htmlReporter);
            
            // Add system information
            _extent.AddSystemInfo("Environment", "Production");
            _extent.AddSystemInfo("Browser", Config.ConfigurationManager.Browser);
            _extent.AddSystemInfo("Base URL", Config.ConfigurationManager.BaseUrl);
            _extent.AddSystemInfo("OS", Environment.OSVersion.ToString());
            _extent.AddSystemInfo(".NET Version", Environment.Version.ToString());
            _extent.AddSystemInfo("Machine Name", Environment.MachineName);
            _extent.AddSystemInfo("Test Execution Time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        /// <summary>
        /// Creates a new test entry in the report
        /// </summary>
        public void CreateTest(string testName)
        {
            _test = _extent.CreateTest(testName);
        }

        /// <summary>
        /// Logs info message
        /// </summary>
        public void LogInfo(string message)
        {
            _test?.Info(message);
        }

        /// <summary>
        /// Logs pass message
        /// </summary>
        public void LogPass(string message)
        {
            _test?.Pass(message);
        }

        /// <summary>
        /// Logs fail message
        /// </summary>
        public void LogFail(string message)
        {
            _test?.Fail(message);
        }

        /// <summary>
        /// Logs warning message
        /// </summary>
        public void LogWarning(string message)
        {
            _test?.Warning(message);
        }

        /// <summary>
        /// Adds screenshot to the report
        /// </summary>
        public void AddScreenshotToReport(string screenshotPath)
        {
            if (!string.IsNullOrEmpty(screenshotPath) && File.Exists(screenshotPath))
            {
                _test?.AddScreenCaptureFromPath(screenshotPath);
            }
        }

        /// <summary>
        /// Flushes the report to write all data
        /// </summary>
        public void FlushReport()
        {
            _extent?.Flush();
        }
    }
}
