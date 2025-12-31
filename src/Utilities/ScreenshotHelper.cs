using OpenQA.Selenium;

namespace DarazAutomation.Utilities
{
    /// <summary>
    /// Helper class for capturing screenshots
    /// </summary>
    public static class ScreenshotHelper
    {
        private static readonly string ScreenshotsFolder = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Reports", "Screenshots");

        /// <summary>
        /// Captures a screenshot and saves it to the screenshots folder
        /// </summary>
        public static string CaptureScreenshot(IWebDriver driver, string testName)
        {
            try
            {
                // Ensure screenshots directory exists
                if (!Directory.Exists(ScreenshotsFolder))
                {
                    Directory.CreateDirectory(ScreenshotsFolder);
                }

                // Generate unique filename
                var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var sanitizedTestName = SanitizeFileName(testName);
                var fileName = $"{sanitizedTestName}_{timestamp}.png";
                var filePath = Path.Combine(ScreenshotsFolder, fileName);

                // Capture screenshot
                var screenshot = ((ITakesScreenshot)driver).GetScreenshot();
                screenshot.SaveAsFile(filePath);

                return filePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to capture screenshot: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Sanitizes the filename by removing invalid characters
        /// </summary>
        private static string SanitizeFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            foreach (var c in invalidChars)
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName;
        }
    }
}
