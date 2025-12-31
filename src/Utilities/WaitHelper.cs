using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace DarazAutomation.Utilities
{
    /// <summary>
    /// Utility class providing common wait helper methods
    /// </summary>
    public static class WaitHelper
    {
        /// <summary>
        /// Waits for a specified duration
        /// </summary>
        public static void Wait(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        /// <summary>
        /// Waits for a specific condition to be true
        /// </summary>
        public static bool WaitForCondition(Func<bool> condition, int timeoutSeconds = 30, int pollingIntervalMs = 500)
        {
            var endTime = DateTime.Now.AddSeconds(timeoutSeconds);
            
            while (DateTime.Now < endTime)
            {
                try
                {
                    if (condition())
                        return true;
                }
                catch
                {
                    // Ignore exceptions and continue waiting
                }
                
                Thread.Sleep(pollingIntervalMs);
            }
            
            return false;
        }

        /// <summary>
        /// Retry an action until it succeeds or timeout
        /// </summary>
        public static T RetryUntilSuccess<T>(Func<T> action, int maxRetries = 3, int delayBetweenRetriesMs = 1000)
        {
            Exception? lastException = null;
            
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    return action();
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    Thread.Sleep(delayBetweenRetriesMs);
                }
            }
            
            throw new Exception($"Action failed after {maxRetries} retries", lastException);
        }

        /// <summary>
        /// Retry an action that returns void
        /// </summary>
        public static void RetryAction(Action action, int maxRetries = 3, int delayBetweenRetriesMs = 1000)
        {
            Exception? lastException = null;
            
            for (int i = 0; i < maxRetries; i++)
            {
                try
                {
                    action();
                    return;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    Thread.Sleep(delayBetweenRetriesMs);
                }
            }
            
            throw new Exception($"Action failed after {maxRetries} retries", lastException);
        }
    }
}
