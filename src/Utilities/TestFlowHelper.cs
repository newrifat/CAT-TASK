using OpenQA.Selenium;
using DarazAutomation.Config;
using DarazAutomation.Pages;
using DarazAutomation.Data;
using NUnit.Framework;

namespace DarazAutomation.Utilities
{
    /// <summary>
    /// Provides reusable helper methods for common test flows and verification steps.
    /// This class encapsulates common test operations to promote code reusability and maintainability.
    /// </summary>
    public class TestFlowHelper
    {
        private readonly IWebDriver _driver;
        private readonly HomePage _homePage;
        private readonly LoginPage _loginPage;
        private readonly CategoryPage _categoryPage;
        private readonly CartPage _cartPage;

        /// <summary>
        /// Initializes a new instance of TestFlowHelper with the provided WebDriver.
        /// </summary>
        /// <param name="driver">The WebDriver instance to use for browser interactions.</param>
        public TestFlowHelper(IWebDriver driver)
        {
            _driver = driver;
            _homePage = new HomePage(driver);
            _loginPage = new LoginPage(driver);
            _categoryPage = new CategoryPage(driver);
            _cartPage = new CartPage(driver);
        }

        /// <summary>
        /// Ensures that the user is logged in. If not logged in, performs login automatically.
        /// Handles various scenarios: already logged in, on login page, or on other pages.
        /// </summary>
        /// <exception cref="Exception">Thrown when login verification fails.</exception>
        public void EnsureUserIsLoggedIn()
        {
            try
            {
                string currentUrl = _driver.Url.ToLower();
                bool isOnDarazPage = currentUrl.Contains(ConfigurationManager.Domain);
                
                if (!isOnDarazPage)
                {
                    _homePage.NavigateToHomePage();
                }
                
                bool isOnLoginPage = currentUrl.Contains(ConfigurationManager.LoginPagePattern) 
                    || currentUrl.Contains(ConfigurationManager.MemberPagePattern);
                bool isLoggedIn = !isOnLoginPage && _loginPage.IsUserLoggedIn();
                
                if (isLoggedIn) return;
                
                if (isOnLoginPage)
                {
                    _homePage.NavigateToHomePage();
                }
                
                _loginPage.OpenLoginPopup();
                
                if (_loginPage.IsLoginPopupDisplayed())
                {
                    string email = ConfigurationManager.LoginEmail;
                    string password = ConfigurationManager.LoginPassword;
                    _loginPage.PerformLogin(email, password);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Login verification failed: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Navigates to the home page and verifies that the user is logged in.
        /// </summary>
        public void NavigateToHomeAndVerifyLogin()
        {
            EnsureUserIsLoggedIn();
            Assert.That(_homePage.IsPageLoaded(), Is.True, "Home page should be loaded");
            Assert.That(_loginPage.IsUserLoggedIn(), Is.True, "User should be logged in");
        }

        /// <summary>
        /// Navigates to a category page by clicking the Daraz logo and selecting the first category.
        /// Scrolls to the top of the page after navigation.
        /// </summary>
        public void NavigateToCategoryPage()
        {
            _homePage.ClickDarazLogo();
            _homePage.ClickCategoryByIndex(0);
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, 0);");
        }

        /// <summary>
        /// Verifies that the main categories navigation is displayed on the category page.
        /// </summary>
        public void VerifyCategoryNavigationDisplayed()
        {
            Assert.That(_categoryPage.IsMainCategoriesNavDisplayed(), Is.True, 
                "Main categories navigation should be displayed");
        }

        /// <summary>
        /// Selects cart items and verifies that the order summary subtotal matches the calculated total.
        /// </summary>
        /// <param name="itemsToSelect">Number of items to select (1 or 2).</param>
        public void SelectCartItemsAndVerifySubtotal(int itemsToSelect)
        {
            if (itemsToSelect == 1)
            {
                _cartPage.SelectCartItems(0);
            }
            else if (itemsToSelect >= 2)
            {
                _cartPage.SelectCartItems(0, 1);
            }

            decimal calculatedTotal = (itemsToSelect == 1) 
                ? _cartPage.CalculateSelectedItemsTotal(0)
                : _cartPage.CalculateSelectedItemsTotal(0, 1);

            bool subtotalMatches = _cartPage.VerifyOrderSummarySubtotal(calculatedTotal);
            Assert.That(subtotalMatches, Is.True, 
                $"Order summary subtotal should match calculated total of ৳ {calculatedTotal}");
        }

        /// <summary>
        /// Verifies that all expected elements are present on the checkout page.
        /// </summary>
        public void VerifyCheckoutPageElements()
        {
            string shippingBillingHeader = _cartPage.GetShippingAndBillingHeaderText();
            Assert.That(shippingBillingHeader, Is.Not.Empty, "'Shipping & Billing' header should be present");
            Assert.That(shippingBillingHeader.Contains("Shipping") || shippingBillingHeader.Contains("Billing"), 
                Is.True, $"Header should contain 'Shipping' or 'Billing'. Actual: '{shippingBillingHeader}'");

            Assert.That(_cartPage.IsProceedToPayButtonDisplayed(), Is.True, 
                "'Proceed to Pay' button should be displayed on checkout page");
            
            string proceedToPayText = _cartPage.GetProceedToPayButtonText();
            Assert.That(proceedToPayText, Is.Not.Empty, "'Proceed to Pay' button text should not be empty");
            Assert.That(proceedToPayText.Contains("Proceed to Pay"), Is.True,
                $"Button text should contain 'Proceed to Pay'. Actual: '{proceedToPayText}'");
        }

        /// <summary>
        /// Verifies that the payment voucher promotional message is displayed on the payment page.
        /// </summary>
        public void VerifyPaymentVoucherMessage()
        {
            Assert.That(_cartPage.IsPaymentVoucherMessageDisplayed(), Is.True, 
                "Payment voucher promotional message should be displayed on payment page");

            string voucherMessage = _cartPage.GetPaymentVoucherMessageText();
            Assert.That(voucherMessage, Is.Not.Empty, "Payment voucher message text should not be empty");

            bool voucherMessageMatches = _cartPage.VerifyPaymentVoucherMessage(ConfigurationManager.PaymentVoucherMessage);
            Assert.That(voucherMessageMatches, Is.True,
                $"Payment voucher message should match: '{ConfigurationManager.PaymentVoucherMessage}'");
        }

        /// <summary>
        /// Navigates to a specific category using the three-level menu navigation.
        /// Verifies that the URL contains the expected keyword after navigation.
        /// </summary>
        /// <param name="level1">First level category name.</param>
        /// <param name="level2">Second level category name.</param>
        /// <param name="level3">Third level category name.</param>
        /// <param name="expectedUrlKeyword">Expected keyword in the URL after navigation.</param>
        public void NavigateToCategory(string level1, string level2, string level3, string expectedUrlKeyword)
        {
            _homePage.NavigateToHomePage();
            Assert.That(_homePage.IsPageLoaded(), Is.True, "Home page should be loaded");
            Assert.That(_loginPage.IsLoginSuccessful(), Is.True, "User should be logged in");

            _homePage.ClickDarazLogo();
            _homePage.ClickCategoryByIndex(0);
            ((IJavaScriptExecutor)_driver).ExecuteScript("window.scrollTo(0, 0);");

            _categoryPage.NavigateThreeLevelMenu(level1, level2, level3);

            string currentUrl = _categoryPage.GetCurrentUrl();
            Assert.That(currentUrl.ToLower().Contains(expectedUrlKeyword.ToLower()),
                Is.True, $"URL should contain '{expectedUrlKeyword}'. Current URL: {currentUrl}");
        }

        /// <summary>
        /// Verifies that products are available on the current category page.
        /// </summary>
        /// <param name="categoryName">The name of the category for error messages.</param>
        /// <returns>The number of products found on the page.</returns>
        public int VerifyProductsAvailable(string categoryName)
        {
            int productCount = _categoryPage.GetProductCount();
            Assert.That(productCount, Is.GreaterThan(0), $"Should have products displayed on {categoryName} page");
            return productCount;
        }

        /// <summary>
        /// Adds a product to the cart and verifies the addition was successful.
        /// This method clicks the product from the category listing and adds it directly.
        /// </summary>
        /// <param name="productDescription">Description of the product for logging purposes.</param>
        /// <param name="selector">Optional product selector. If null, selects the first product.</param>
        public void AddProductToCartAndVerify(string productDescription, CategoryNavigationTestData.ProductSelector? selector = null)
        {
            int initialCartCount = _categoryPage.GetCartItemCount();
            
            // Click the product based on selector
            if (selector != null)
            {
                _categoryPage.ClickProductBySelector(selector);
            }
            else
            {
                _categoryPage.ClickProductByIndex(0);
            }
            
            // Initialize ProductPage after navigating to product detail page
            var productPage = new ProductPage(_driver);
            productPage.WaitForPageToLoad(15);
            
            // Handle variant selection if required
            if (productPage.IsVariantSelectionRequired())
            {
                productPage.SelectFirstAvailableVariant();
            }
            
            // Add to cart from the product detail page
            productPage.ClickAddToCart();

            // Verify success using CategoryPage (cart dialog should appear)
            Assert.That(_categoryPage.IsCartSuccessMessageDisplayed(), Is.True,
                "Success message 'Added to cart successfully!' should be displayed");

            string successMessage = _categoryPage.GetCartSuccessMessageText();
            Assert.That(successMessage, Does.Contain("Added to cart successfully"),
                "Message should contain 'Added to cart successfully'");

            _categoryPage.CloseCartDialog();

            int finalCartCount = _categoryPage.GetCartItemCount();
            Assert.That(finalCartCount, Is.GreaterThan(initialCartCount),
                "Cart count should increase after adding product");
        }

        /// <summary>
        /// Adds a product to the cart via the product detail page.
        /// This method navigates to the product page before adding to cart.
        /// </summary>
        /// <param name="categoryDescription">Description of the category for logging purposes.</param>
        /// <param name="selector">Optional product selector. If null, selects the first product.</param>
        public void AddProductViaProductPage(string categoryDescription, CategoryNavigationTestData.ProductSelector? selector = null)
        {
            int initialCartCount = _categoryPage.GetCartItemCount();
            
            if (selector != null)
            {
                _categoryPage.ClickProductBySelector(selector);
            }
            else
            {
                string productTitle = _categoryPage.GetProductTitleByIndex(0);
                _categoryPage.ClickProductByIndex(0);
            }
            
            // Initialize ProductPage after navigating to product detail page
            var productPage = new ProductPage(_driver);
            productPage.WaitForPageToLoad(15);
            
            // Handle variant selection if required
            if (productPage.IsVariantSelectionRequired())
            {
                productPage.SelectFirstAvailableVariant();
            }
            
            // Add to cart from the product detail page
            productPage.ClickAddToCart();

            // Verify success using CategoryPage (cart dialog should appear)
            Assert.That(_categoryPage.IsCartSuccessMessageDisplayed(), Is.True,
                "Success message 'Added to cart successfully!' should be displayed");

            string successMessage = _categoryPage.GetCartSuccessMessageText();
            Assert.That(successMessage, Does.Contain("Added to cart successfully"),
                "Message should contain 'Added to cart successfully'");

            _categoryPage.CloseCartDialog();

            int finalCartCount = _categoryPage.GetCartItemCount();
            Assert.That(finalCartCount, Is.GreaterThan(initialCartCount),
                "Cart count should increase after adding product");
        }

        /// <summary>
        /// Data-driven helper to navigate to a category and add a product to cart.
        /// Handles complete flow: login, navigation, product verification, and cart addition.
        /// </summary>
        /// <param name="level1">First level category (e.g., "Women's & Girls' Fashion").</param>
        /// <param name="level2">Second level category (e.g., "Bags").</param>
        /// <param name="level3">Third level category (e.g., "Wallets").</param>
        /// <param name="urlKeyword">Keyword to verify in URL (e.g., "wallets").</param>
        /// <param name="selector">Product selector (by index, name, or keyword). If null, selects first product.</param>
        /// <param name="useProductPage">If true, navigates to product page before adding. If false, adds from category listing.</param>
        /// <example>
        /// Usage:
        /// <code>
        /// // By index
        /// helper.NavigateAndAddProductToCart("Women's & Girls' Fashion", "Bags", "Wallets", "wallets", CategoryNavigationTestData.ProductSelector.FirstProduct(), false);
        /// 
        /// // By name
        /// helper.NavigateAndAddProductToCart("Men's & Boys' Fashion", "Shoes", "Shoes Accessories", "shoes", CategoryNavigationTestData.ProductSelector.ByName("Nike"), true);
        /// 
        /// // By keyword
        /// helper.NavigateAndAddProductToCart("Electronic Devices", "Mobiles", "Smartphones", "smartphones", CategoryNavigationTestData.ProductSelector.ByKeyword("Samsung"), false);
        /// </code>
        /// </example>
        public void NavigateAndAddProductToCart(string level1, string level2, string level3, string urlKeyword, 
            CategoryNavigationTestData.ProductSelector? selector = null, bool useProductPage = false)
        {
            EnsureUserIsLoggedIn();
            NavigateToCategory(level1, level2, level3, urlKeyword);
            VerifyProductsAvailable(level3);
            
            if (useProductPage)
            {
                AddProductViaProductPage($"{level1} > {level2} > {level3}", selector);
            }
            else
            {
                AddProductToCartAndVerify($"product from {level3}", selector);
            }
            
            Assert.That(_loginPage.IsUserLoggedIn(), Is.True, 
                $"User should remain logged in after adding product from {level3}");
        }

        /// <summary>
        /// Performs a complete cart checkout flow: navigate to cart, select items, verify subtotal,
        /// proceed to checkout, verify checkout elements, proceed to payment, and verify payment page.
        /// </summary>
        public void PerformCompleteCheckoutFlow()
        {
            EnsureUserIsLoggedIn();
            
            _cartPage.NavigateToCart();
            Assert.That(_cartPage.IsCartPageLoaded(), Is.True, "Should be on the cart page");

            int cartItemsCount = _cartPage.GetCartItemsCount();
            Assert.That(cartItemsCount, Is.GreaterThan(0), "Cart should display items on the page");

            int itemsToSelect = Math.Min(cartItemsCount, 2);
            SelectCartItemsAndVerifySubtotal(itemsToSelect);

            _cartPage.ClickProceedToCheckout();
            Assert.That(_cartPage.IsOnCheckoutPage(), Is.True, "Should navigate to checkout page");

            VerifyCheckoutPageElements();

            _cartPage.ClickProceedToPay();
            Assert.That(_cartPage.IsOnPaymentPage(), Is.True, "Should navigate to payment page after clicking Proceed to Pay");

            VerifyPaymentVoucherMessage();
        }
    }
}
