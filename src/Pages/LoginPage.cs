using OpenQA.Selenium;
using DarazAutomation.Base;

namespace DarazAutomation.Pages
{
    public class LoginPage : BasePage
    {
        private readonly By _loginPopup = By.CssSelector(".lzd-member-loginsign-popup-content, [class*='loginWrapper']");
        private readonly By _emailPhoneInput = By.XPath("//input[@placeholder='Please enter your Phone or Email' or contains(@placeholder, 'Phone or Email')]");
        private readonly By _passwordInput = By.XPath("//input[@type='password' or @type='text'][contains(@placeholder, 'password')]");
        private readonly By _loginButton = By.XPath("//button[contains(@class, 'loginButton') or contains(text(), 'LOGIN')]");
        private readonly By _loginLinkHeader = By.CssSelector("#anonLogin a[data-spm-click]");
        private readonly By _loginLinkAlt = By.XPath("//div[@id='anonLogin']//a");
        private readonly By _userAccountDropdown = By.CssSelector("#myAccountTrigger, .account-user, [data-spm='account']");

        public LoginPage(IWebDriver driver) : base(driver)
        {
        }

        public LoginPage OpenLoginPopup()
        {
            WaitForPageLoad(); // Only wait for page load, not full ready
            
            try
            {
                var loginLink = WaitForElementClickable(_loginLinkHeader, 5);
                ScrollToElement(loginLink);
                loginLink.Click();
            }
            catch
            {
                try
                {
                    var loginLinkAlt = WaitForElementClickable(_loginLinkAlt, 5);
                    loginLinkAlt.Click();
                }
                catch
                {
                    ClickWithJavaScript(_loginLinkHeader);
                }
            }
            
            WaitForLoginPopup();
            return this;
        }

        public bool IsLoginPopupDisplayed()
        {
            try
            {
                return IsElementDisplayed(_loginPopup, 5);
            }
            catch
            {
                return false;
            }
        }

        public LoginPage WaitForLoginPopup()
        {
            WaitForElementVisible(_loginPopup, 10);
            return this;
        }

        public LoginPage EnterEmail(string email)
        {
            var emailField = WaitForElementVisible(_emailPhoneInput, 10);
            emailField.Clear();
            emailField.SendKeys(email);
            WaitForCondition(d => emailField.GetAttribute("value").Length > 0, 3);
            return this;
        }

        public LoginPage EnterPassword(string password)
        {
            var passwordField = WaitForElementVisible(_passwordInput, 10);
            passwordField.Clear();
            passwordField.SendKeys(password);
            WaitForCondition(d => passwordField.GetAttribute("value").Length > 0, 3);
            return this;
        }

        public LoginPage ClickLoginButton()
        {
            var loginButton = WaitForElementClickable(_loginButton, 10);
            loginButton.Click();
            WaitForPageLoad(); // Only wait for page load, not full ready
            return this;
        }

        public LoginPage PerformLogin(string email, string password)
        {
            WaitForLoginPopup();
            EnterEmail(email);
            EnterPassword(password);
            ClickLoginButton();
            return this;
        }

        public bool IsLoginSuccessful()
        {
            try
            {
                return IsElementDisplayed(_userAccountDropdown, 10);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Enhanced login verification that checks multiple indicators of logged-in state.
        /// More robust than IsLoginSuccessful() as it tries multiple selectors and approaches.
        /// </summary>
        /// <returns>True if user is logged in based on any valid indicator.</returns>
        public bool IsUserLoggedIn()
        {
            try
            {
                var loginIndicators = new[]
                {
                    By.CssSelector("#myAccountTrigger"),
                    By.CssSelector(".account-user"),
                    By.CssSelector("[data-spm='account']")
                };

                foreach (var selector in loginIndicators)
                {
                    try
                    {
                        var element = Driver.FindElement(selector);
                        if (element.Displayed)
                        {
                            string text = element.Text.ToLower();
                            if (text.Contains("login") && !text.Contains("my account"))
                            {
                                continue;
                            }
                            return true;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }

                try
                {
                    var loginLink = Driver.FindElement(_loginLinkHeader);
                    if (!loginLink.Displayed)
                    {
                        return true;
                    }
                }
                catch
                {
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
