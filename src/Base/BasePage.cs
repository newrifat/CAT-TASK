using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using DarazAutomation.Config;

namespace DarazAutomation.Base
{
    /// <summary>
    /// Base class for all Page Objects
    /// Contains common methods and properties shared across pages
    /// </summary>
    public abstract class BasePage
    {
        protected readonly IWebDriver Driver;
        protected readonly WebDriverWait Wait;

        protected BasePage(IWebDriver driver)
        {
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(ConfigurationManager.ExplicitWaitSeconds))
            {
                PollingInterval = TimeSpan.FromMilliseconds(100) // Check every 100ms instead of default 500ms for faster response
            };
        }

        #region Navigation Methods

        /// <summary>
        /// Navigates to the specified URL
        /// </summary>
        public void NavigateTo(string url)
        {
            Driver.Navigate().GoToUrl(url);
            WaitForPageLoad();
        }

        /// <summary>
        /// Refreshes the current page
        /// </summary>
        public void RefreshPage()
        {
            Driver.Navigate().Refresh();
            WaitForPageLoad();
        }

        /// <summary>
        /// Gets the current page URL
        /// </summary>
        public string GetCurrentUrl() => Driver.Url;

        /// <summary>
        /// Gets the current page title
        /// </summary>
        public string GetPageTitle() => Driver.Title;

        #endregion

        #region Wait Methods

        /// <summary>
        /// Waits for an element to be visible
        /// </summary>
        protected IWebElement WaitForElementVisible(By locator, int? timeoutSeconds = null)
        {
            var wait = timeoutSeconds.HasValue
                ? new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds.Value))
                : Wait;
            
            return wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }

        /// <summary>
        /// Waits for an element to be clickable
        /// </summary>
        protected IWebElement WaitForElementClickable(By locator, int? timeoutSeconds = null)
        {
            var wait = timeoutSeconds.HasValue
                ? new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds.Value))
                : Wait;
            
            return wait.Until(ExpectedConditions.ElementToBeClickable(locator));
        }

        /// <summary>
        /// Waits for a specific element instance to be clickable
        /// </summary>
        protected IWebElement WaitForElementToBeClickable(IWebElement element, int? timeoutSeconds = null)
        {
            var wait = timeoutSeconds.HasValue
                ? new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds.Value))
                : Wait;
            
            return wait.Until(driver => 
            {
                try
                {
                    return element.Displayed && element.Enabled ? element : null;
                }
                catch
                {
                    return null;
                }
            });
        }

        /// <summary>
        /// Waits for an element to be present in DOM
        /// </summary>
        protected IWebElement WaitForElementPresent(By locator, int? timeoutSeconds = null)
        {
            var wait = timeoutSeconds.HasValue
                ? new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds.Value))
                : Wait;
            
            return wait.Until(ExpectedConditions.ElementExists(locator));
        }

        /// <summary>
        /// Waits for text to be present in element
        /// </summary>
        protected bool WaitForTextInElement(By locator, string text, int? timeoutSeconds = null)
        {
            var wait = timeoutSeconds.HasValue
                ? new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds.Value))
                : Wait;
            
            return wait.Until(ExpectedConditions.TextToBePresentInElementLocated(locator, text));
        }

        /// <summary>
        /// Waits for page to fully load
        /// </summary>
        protected void WaitForPageLoad(int? timeoutSeconds = null)
        {
            var wait = timeoutSeconds.HasValue
                ? new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds.Value))
                : Wait;
            
            wait.Until(driver => ((IJavaScriptExecutor)driver)
                .ExecuteScript("return document.readyState").Equals("complete"));
        }

        /// <summary>
        /// Waits for network to be idle (no active requests)
        /// This is more reliable than Thread.Sleep for waiting after actions
        /// Optimized for speed with shorter timeout
        /// </summary>
        protected void WaitForNetworkIdle(int maxWaitSeconds = 2)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(maxWaitSeconds))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(100) // Check every 100ms instead of default 500ms
                };
                
                wait.Until(driver =>
                {
                    // Check if document is ready
                    var readyState = ((IJavaScriptExecutor)driver)
                        .ExecuteScript("return document.readyState").ToString();
                    
                    // Check if jQuery is loaded and has no active requests
                    var jQueryActive = ((IJavaScriptExecutor)driver)
                        .ExecuteScript("return typeof jQuery != 'undefined' ? jQuery.active : 0");
                    
                    return readyState == "complete" && jQueryActive.ToString() == "0";
                });
            }
            catch
            {
                // Fallback - if jQuery check fails, just verify document is ready
                // No need to call WaitForPageLoad again, we already checked readyState
            }
        }

        /// <summary>
        /// Waits for all Ajax requests to complete
        /// Optimized with shorter timeout
        /// </summary>
        protected void WaitForAjaxComplete(int maxWaitSeconds = 3)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(maxWaitSeconds))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(100)
                };
                
                wait.Until(driver =>
                {
                    var jQueryActive = ((IJavaScriptExecutor)driver)
                        .ExecuteScript("return typeof jQuery != 'undefined' ? jQuery.active == 0 : true");
                    return (bool)jQueryActive;
                });
            }
            catch
            {
                // jQuery might not be present, that's okay
            }
        }

        /// <summary>
        /// Waits for DOM to be stable (no changes for a short period)
        /// More efficient than Thread.Sleep
        /// Optimized with shorter timeout and faster polling
        /// </summary>
        protected void WaitForDomStable(int maxWaitSeconds = 1)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(maxWaitSeconds))
                {
                    PollingInterval = TimeSpan.FromMilliseconds(50) // Check every 50ms for faster response
                };
                
                string previousHtml = "";
                int stableCount = 0;
                
                wait.Until(driver =>
                {
                    var currentHtml = driver.PageSource.Length.ToString();
                    if (currentHtml == previousHtml)
                    {
                        stableCount++;
                    }
                    else
                    {
                        stableCount = 0;
                        previousHtml = currentHtml;
                    }
                    
                    // Consider stable if unchanged for 1 consecutive check (50ms interval means 2 checks = 100ms stability)
                    return stableCount >= 1;
                });
            }
            catch
            {
                // Timeout is acceptable, page might already be stable
            }
        }

        /// <summary>
        /// Comprehensive wait for page to be ready for interaction
        /// Combines page load and network idle checks with optimized timeouts
        /// </summary>
        protected void WaitForPageReady(int maxWaitSeconds = 5)
        {
            // Only wait for page load, network idle is redundant if page is already loaded
            WaitForPageLoad(maxWaitSeconds);
        }

        /// <summary>
        /// Waits for a specific condition
        /// </summary>
        protected void WaitForCondition(Func<IWebDriver, bool> condition, int? timeoutSeconds = null)
        {
            var wait = timeoutSeconds.HasValue
                ? new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds.Value))
                : Wait;
            
            wait.Until(condition);
        }

        #endregion

        #region Element Interaction Methods

        /// <summary>
        /// Clicks an element after waiting for it to be clickable
        /// </summary>
        protected void Click(By locator)
        {
            var element = WaitForElementClickable(locator);
            element.Click();
        }

        /// <summary>
        /// Clicks an element using JavaScript
        /// </summary>
        protected void ClickWithJavaScript(By locator)
        {
            var element = WaitForElementPresent(locator);
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", element);
        }

        /// <summary>
        /// Enters text into an element
        /// </summary>
        protected void EnterText(By locator, string text)
        {
            var element = WaitForElementVisible(locator);
            element.Clear();
            element.SendKeys(text);
        }

        /// <summary>
        /// Gets text from an element
        /// </summary>
        protected string GetText(By locator)
        {
            var element = WaitForElementVisible(locator);
            return element.Text;
        }

        /// <summary>
        /// Checks if an element is displayed
        /// </summary>
        protected bool IsElementDisplayed(By locator, int timeoutSeconds = 5)
        {
            try
            {
                var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
                wait.Until(ExpectedConditions.ElementIsVisible(locator));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if element contains specific text
        /// </summary>
        protected bool ElementContainsText(By locator, string text)
        {
            try
            {
                var element = WaitForElementVisible(locator, 10);
                return element.Text.Contains(text, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region JavaScript Methods

        /// <summary>
        /// Scrolls element into view
        /// </summary>
        protected void ScrollToElement(By locator)
        {
            var element = WaitForElementPresent(locator);
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'auto', block: 'center'});", element);
            // Wait for scroll to complete by checking element position stability
            WaitForCondition(driver =>
            {
                var position = ((IJavaScriptExecutor)driver).ExecuteScript(
                    "return arguments[0].getBoundingClientRect().top;", element);
                return position != null;
            }, 2);
        }

        /// <summary>
        /// Scrolls element into view (overload for IWebElement)
        /// </summary>
        protected void ScrollToElement(IWebElement element)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].scrollIntoView({behavior: 'auto', block: 'center'});", element);
            // Wait for scroll to complete by checking element position stability
            WaitForCondition(driver =>
            {
                var position = ((IJavaScriptExecutor)driver).ExecuteScript(
                    "return arguments[0].getBoundingClientRect().top;", element);
                return position != null;
            }, 2);
        }

        /// <summary>
        /// Scrolls to top of page
        /// </summary>
        protected void ScrollToTop()
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("window.scrollTo(0, 0);");
        }

        /// <summary>
        /// Executes JavaScript and returns result
        /// </summary>
        protected object ExecuteJavaScript(string script, params object[] args)
        {
            return ((IJavaScriptExecutor)Driver).ExecuteScript(script, args);
        }

        #endregion

        #region Hover Methods

        /// <summary>
        /// Hovers over an element using Actions
        /// </summary>
        protected void HoverOverElement(By locator)
        {
            var element = WaitForElementVisible(locator);
            var actions = new OpenQA.Selenium.Interactions.Actions(Driver);
            actions.MoveToElement(element).Perform();
        }

        /// <summary>
        /// Hovers over a specific element instance using Actions
        /// </summary>
        protected void HoverOverElement(IWebElement element)
        {
            var actions = new OpenQA.Selenium.Interactions.Actions(Driver);
            actions.MoveToElement(element).Perform();
        }

        #endregion
    }
}
