using NUnit.Framework;
using OpenQA.Selenium;
using DarazAutomation.Base;
using DarazAutomation.Config;
using DarazAutomation.Pages;
using DarazAutomation.Data;
using DarazAutomation.Utilities;

namespace DarazAutomation.Tests.IndependentTests
{
    [TestFixture]
    [Category("Sequential")]
    [Category("SessionPersistent")]
    [Order(1)]
    public class SequentialTests : BaseTest
    {
        protected override bool ReuseSession => true;

        private HomePage _homePage = null!;
        private LoginPage _loginPage = null!;
        private CategoryPage _categoryPage = null!;
        private CartPage _cartPage = null!;
        private TestFlowHelper _testFlowHelper = null!;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _homePage = new HomePage(Driver);
            _loginPage = new LoginPage(Driver);
            _categoryPage = new CategoryPage(Driver);
            _cartPage = new CartPage(Driver);
            _testFlowHelper = new TestFlowHelper(Driver);
        }

        [Test, Order(1)]
        [Category("Critical")]
        public void Test1_LanguageSwitch()
        {
            _homePage.NavigateToHomePage();
            Assert.That(_homePage.IsPageLoaded(), Is.True, "Home page should be loaded");

            string currentLang = _homePage.GetCurrentLanguage();
            if (currentLang.Contains("বাংলা") || currentLang.ToLower().Contains("bangla"))
            {
                _homePage.ChangeLanguageTo("English");
            }

            _homePage.ChangeLanguageTo("Bangla");
            bool isBanglaVerified = _homePage.VerifyLanguageChanged(ConfigurationManager.BanglaVerificationText);
            Assert.That(isBanglaVerified, Is.True, "Language should be changed to Bangla");

            _homePage.ChangeLanguageTo("English");
            bool isEnglishVerified = _homePage.VerifyLanguageChanged(ConfigurationManager.EnglishVerificationText);
            Assert.That(isEnglishVerified, Is.True, "Language should be changed back to English");
        }

        [Test, Order(2)]
        [Category("Critical")]
        public void Test2_Login()
        {
            _loginPage.OpenLoginPopup();
            Assert.That(_loginPage.IsLoginPopupDisplayed(), Is.True, "Login popup should be displayed");

            string email = ConfigurationManager.LoginEmail;
            string password = ConfigurationManager.LoginPassword;

            _loginPage.PerformLogin(email, password);

            bool isLoggedIn = _loginPage.IsLoginSuccessful();
            Assert.That(isLoggedIn, Is.True, "User should be logged in successfully");
        }

        [Test, Order(3)]
        [Category("Critical")]
        public void Test3_VerifySessionPersisted()
        {
            bool isStillLoggedIn = _loginPage.IsLoginSuccessful();
            Assert.That(isStillLoggedIn, Is.True, "User session should be persisted from previous test");
        }

        [Test, Order(4)]
        [Category("Critical")]
        public void Test4_NavigateToCategoryWhileLoggedIn()
        {
            _testFlowHelper.EnsureUserIsLoggedIn();
            _homePage.NavigateToHomePage();
            Assert.That(_homePage.IsPageLoaded(), Is.True, "Home page should be loaded");

            _homePage.ClickDarazLogo();
            Assert.That(_loginPage.IsLoginSuccessful(), Is.True, "User should still be logged in after clicking Daraz logo");

            _homePage.ClickCategoryByIndex(0);
            ((IJavaScriptExecutor)Driver).ExecuteScript("window.scrollTo(0, 0);");

            Assert.That(_categoryPage.IsMainCategoriesNavDisplayed(), Is.True, "Main categories navigation should be displayed");
            Assert.That(_categoryPage.IsCategoriesLabelDisplayed(), Is.True, "Categories label should be displayed");
            Assert.That(_categoryPage.GetCategoryRootItemsCount(), Is.GreaterThan(0), "Category items should be available");
            Assert.That(_categoryPage.VerifyMainCategoriesNavigation(), Is.True, "Main categories navigation should be fully loaded");
            Assert.That(_loginPage.IsLoginSuccessful(), Is.True, "User should still be logged in after category verification");
        }

        [Test, Order(5)]
        [Category("Critical")]
        public void Test5_MultiLevelCategoryNavigation_WomensWallets()
        {
            _testFlowHelper.EnsureUserIsLoggedIn();
            _testFlowHelper.NavigateToCategory("Women's & Girls' Fashion", "Bags", "Wallets", "wallets");
            Assert.That(_loginPage.IsLoginSuccessful(), Is.True, "User should remain logged in after category navigation");
        }

        [Test, Order(6)]
        [Category("Critical")]
        public void Test6_AddFirstWalletToCart()
        {
            var testData = CategoryNavigationTestData.GetTestCase(0);
            _testFlowHelper.NavigateAndAddProductToCart(
                level1: testData.Level1,
                level2: testData.Level2,
                level3: testData.Level3,
                urlKeyword: testData.UrlKeyword,
                selector: testData.Selector,
                useProductPage: testData.UseProductPage
            );
        }

        [Test, Order(7)]
        [Category("Critical")]
        public void Test7_AddSecondProductFromMensShoesAccessories()
        {
            var testData = CategoryNavigationTestData.GetTestCase(1);
            _testFlowHelper.NavigateAndAddProductToCart(
                level1: testData.Level1,
                level2: testData.Level2,
                level3: testData.Level3,
                urlKeyword: testData.UrlKeyword,
                selector: testData.Selector,
                useProductPage: testData.UseProductPage
            );
        }

        [Test, Order(8)]
        [Category("Critical")]
        public void Test8_NavigateToCartSelectItemsAndCheckout()
        {
            _testFlowHelper.PerformCompleteCheckoutFlow();
        }

        // ===============================================================================
        // OPTIONAL: Data-Driven Test Example using [TestCaseSource]
        // ===============================================================================
        // Uncomment the test below to run all test cases from CategoryNavigationTestData
        // This demonstrates how to extend the framework for multiple category tests
        // ===============================================================================
        
        /*
        /// <summary>
        /// Data-driven test that runs for all test cases defined in CategoryNavigationTestData.
        /// This test can be used to add products from multiple categories in a single parameterized test.
        /// Uncomment and adjust [Order] attribute as needed.
        /// </summary>
        [Test]
        [Category("DataDriven")]
        [TestCaseSource(typeof(CategoryNavigationTestData), nameof(CategoryNavigationTestData.AddProductToCartTestCases))]
        public void AddProductToCart_DataDriven(string level1, string level2, string level3, string urlKeyword, CategoryNavigationTestData.ProductSelector selector, bool useProductPage)
        {
            _testFlowHelper.NavigateAndAddProductToCart(level1, level2, level3, urlKeyword, selector, useProductPage);
        }

        /// <summary>
        /// Data-driven test for category navigation only (without adding products).
        /// Useful for smoke testing category navigation across multiple categories.
        /// </summary>
        [Test]
        [Category("DataDriven")]
        [Category("Navigation")]
        [TestCaseSource(typeof(CategoryNavigationTestData), nameof(CategoryNavigationTestData.CategoryNavigationTestCases))]
        public void NavigateToCategory_DataDriven(string level1, string level2, string level3, string urlKeyword)
        {
            _testFlowHelper.EnsureUserIsLoggedIn();
            _testFlowHelper.NavigateToCategory(level1, level2, level3, urlKeyword);
            _testFlowHelper.VerifyProductsAvailable(level3);
            Assert.That(_loginPage.IsUserLoggedIn(), Is.True, 
                $"User should remain logged in after navigating to {level3}");
        }
        */
    }
}
