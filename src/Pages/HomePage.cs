using OpenQA.Selenium;
using DarazAutomation.Base;
using DarazAutomation.Config;

namespace DarazAutomation.Pages
{
    public class HomePage : BasePage
    {
        private readonly By _languageSwitcherContainer = By.Id("topActionSwitchLang");
        private readonly By _currentLanguageLabel = By.CssSelector("#topActionSwitchLang > span");
        private readonly By _languageDropdownContent = By.CssSelector("#lzdSwitchPop .lzd-switch-content");
        private readonly By _currentlySelectedLanguage = By.CssSelector(".lzd-switch-item.currentSelected");
        private readonly By _headerSection = By.CssSelector(".lzd-header, .header-content");
        private readonly By _darazLogo = By.CssSelector("a[data-spm='dhome']");
        private readonly By _categoriesSection = By.Id("js_categories");
        private readonly By _categoryItems = By.CssSelector(".card-categories-li");
        
        private By GetLanguageOptionByCode(string langCode) =>
            By.CssSelector($".lzd-switch-item[data-lang='{langCode}']");

        public HomePage(IWebDriver driver) : base(driver)
        {
        }

        public HomePage NavigateToHomePage()
        {
            NavigateTo(ConfigurationManager.BaseUrl);
            WaitForPageLoad();
            return this;
        }

        public bool IsPageLoaded()
        {
            try
            {
                WaitForPageLoad();
                return IsElementDisplayed(_headerSection, 10);
            }
            catch
            {
                return false;
            }
        }

        public HomePage ClickOnLanguageSwitcher()
        {
            try
            {
                var languageLabel = WaitForElementClickable(_currentLanguageLabel, 5);
                languageLabel.Click();
            }
            catch
            {
                ClickWithJavaScript(_languageSwitcherContainer);
            }
            
            WaitForElementVisible(_languageDropdownContent, 5);
            return this;
        }

        public HomePage ChangeLanguageTo(string targetLanguage)
        {
            ScrollToTop();
            string langCode = GetLanguageCode(targetLanguage);
            
            if (IsCurrentlySelectedLanguage(langCode))
            {
                return this;
            }

            ClickOnLanguageSwitcher();
            By languageOptionLocator = GetLanguageOptionByCode(langCode);
            
            try
            {
                var languageOption = WaitForElementClickable(languageOptionLocator, 3);
                languageOption.Click();
            }
            catch
            {
                ClickWithJavaScript(languageOptionLocator);
            }

            WaitForPageLoad();
            return this;
        }

        private string GetLanguageCode(string languageText)
        {
            string lowerText = languageText.ToLower().Trim();
            
            if (lowerText.Contains("বাংলা") || lowerText.Contains("bengali") || 
                lowerText.Contains("bangla") || lowerText == "bn")
            {
                return "bn";
            }
            
            return "en";
        }

        private bool IsCurrentlySelectedLanguage(string langCode)
        {
            try
            {
                var currentlySelected = Driver.FindElement(_currentlySelectedLanguage);
                string dataLang = currentlySelected.GetAttribute("data-lang");
                return dataLang?.Equals(langCode, StringComparison.OrdinalIgnoreCase) ?? false;
            }
            catch
            {
                return false;
            }
        }

        public string GetCurrentLanguage()
        {
            try
            {
                var languageLabel = WaitForElementVisible(_currentLanguageLabel, 10);
                return languageLabel.Text.Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        public bool VerifyLanguageChanged(string expectedText)
        {
            WaitForPageLoad();
            
            try
            {
                var body = WaitForElementVisible(By.TagName("body"), 5);
                var bodyText = body.Text;
                return bodyText.Contains(expectedText, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        public HomePage ClickDarazLogo()
        {
            try
            {
                var logo = WaitForElementClickable(_darazLogo, 10);
                logo.Click();
            }
            catch
            {
                ClickWithJavaScript(_darazLogo);
            }
            
            WaitForPageLoad();
            return this;
        }

        public HomePage ScrollToCategoriesSection()
        {
            try
            {
                var categoriesSection = WaitForElementVisible(_categoriesSection, 10);
                ScrollToElement(categoriesSection);
                WaitForDomStable();
            }
            catch
            {
                ((IJavaScriptExecutor)Driver).ExecuteScript("window.scrollTo(0, 800);");
                WaitForDomStable();
            }
            return this;
        }

        public HomePage ClickCategoryByIndex(int index)
        {
            ScrollToCategoriesSection();
            
            var categories = Driver.FindElements(_categoryItems);
            if (index >= 0 && index < categories.Count)
            {
                try
                {
                    ScrollToElement(categories[index]);
                    WaitForElementToBeClickable(categories[index], 3);
                    categories[index].Click();
                }
                catch
                {
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", categories[index]);
                }
                
                WaitForPageLoad();
            }
            else
            {
                throw new ArgumentOutOfRangeException($"Category index {index} is out of range. Total: {categories.Count}");
            }
            
            return this;
        }

        public int GetCategoryCount()
        {
            ScrollToCategoriesSection();
            var categories = Driver.FindElements(_categoryItems);
            return categories.Count;
        }
    }
}
