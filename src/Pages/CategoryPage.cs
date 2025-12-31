using OpenQA.Selenium;
using DarazAutomation.Base;
using System.Threading;

namespace DarazAutomation.Pages
{
    /// <summary>
    /// Page Object for the main Categories navigation menu.
    /// </summary>
    public class CategoryPage : BasePage
    {
        private readonly By _mainCategoriesNav = By.CssSelector(".lzd-site-menu-nav-category");
        private readonly By _categoriesLabel = By.CssSelector(".lzd-site-menu-nav-category-text");
        private readonly By _categoriesMenu = By.CssSelector(".lzd-site-menu-nav-menu");
        private readonly By _categoryRootItems = By.CssSelector(".lzd-site-menu-root-item");
        private readonly By _categoryRootItemLinks = By.CssSelector(".lzd-site-menu-root-item .lzd-site-menu-root-item-link span");
        private readonly By _subMenuContainer = By.CssSelector(".lzd-site-menu-sub");
        private readonly By _subMenuItems = By.CssSelector(".lzd-site-menu-sub-item");
        private readonly By _subCategoryLinks = By.CssSelector(".lzd-site-menu-sub-item a span");
        private readonly By _grandMenuItems = By.CssSelector(".lzd-site-menu-grand-item");
        private readonly By _grandCategoryLinks = By.CssSelector(".lzd-site-menu-grand-item a span");
        private readonly By _productItems = By.CssSelector("[data-qa-locator='product-item']");
        private readonly By _productLinks = By.CssSelector("[data-qa-locator='product-item'] a");
        private readonly By _productTitles = By.CssSelector("[data-qa-locator='product-item'] .title");
        private readonly By _cartSuccessDialog = By.CssSelector(".cart-dialog.next-dialog");
        private readonly By _cartSuccessMessage = By.CssSelector(".cart-message-text");
        private readonly By _cartDialogCloseButton = By.CssSelector(".next-dialog-close");

        public CategoryPage(IWebDriver driver) : base(driver)
        {
        }

        /// <summary>
        /// Verifies that the main Categories navigation container is displayed.
        /// </summary>
        /// <returns>True if the main categories navigation is visible.</returns>
        public bool IsMainCategoriesNavDisplayed()
        {
            try
            {
                WaitForElementVisible(_mainCategoriesNav, 10);
                return IsElementDisplayed(_mainCategoriesNav, 3);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Verifies that the "Categories" label/text is displayed in the navigation.
        /// </summary>
        /// <returns>True if the categories label is visible.</returns>
        public bool IsCategoriesLabelDisplayed()
        {
            try
            {
                return IsElementDisplayed(_categoriesLabel, 10);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the text of the Categories label (e.g., "Categories" or "বিভাগ" for Bangla).
        /// </summary>
        /// <returns>The text of the categories label.</returns>
        public string GetCategoriesLabelText()
        {
            try
            {
                var labelElement = WaitForElementVisible(_categoriesLabel, 10);
                return labelElement.Text.Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the count of root-level category items in the navigation menu.
        /// </summary>
        /// <returns>The number of category root items.</returns>
        public int GetCategoryRootItemsCount()
        {
            try
            {
                var rootItems = Driver.FindElements(_categoryRootItems);
                return rootItems.Count;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets all the category names from the root-level category items.
        /// </summary>
        /// <returns>A list of category names.</returns>
        public List<string> GetAllCategoryNames()
        {
            try
            {
                var categoryLinks = Driver.FindElements(_categoryRootItemLinks);
                return categoryLinks.Select(link => link.Text.Trim()).Where(text => !string.IsNullOrWhiteSpace(text)).ToList();
            }
            catch
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// Hovers over the main Categories navigation to reveal the dropdown menu.
        /// </summary>
        public CategoryPage HoverOverCategories()
        {
            try
            {
                var categoriesNav = WaitForElementVisible(_mainCategoriesNav, 10);
                HoverOverElement(categoriesNav);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to hover over Categories navigation: {ex.Message}");
            }

            return this;
        }

        /// <summary>
        /// Clicks on a category by its name/text.
        /// </summary>
        /// <param name="categoryText">The text of the category to click.</param>
        public CategoryPage ClickCategoryByName(string categoryText)
        {
            try
            {
                HoverOverCategories(); // First hover to reveal the menu
                
                var categoryLinks = Driver.FindElements(_categoryRootItemLinks);
                var targetLink = categoryLinks.FirstOrDefault(link => 
                    link.Text.Trim().Equals(categoryText, StringComparison.OrdinalIgnoreCase) ||
                    link.Text.Contains(categoryText, StringComparison.OrdinalIgnoreCase));

                if (targetLink != null)
                {
                    WaitForElementToBeClickable(targetLink, 5);
                    targetLink.Click();
                    WaitForPageLoad(); // Only wait for page load, not full ready
                }
                else
                {
                    throw new Exception($"Category with text '{categoryText}' not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to click category '{categoryText}': {ex.Message}");
            }

            return this;
        }

        /// <summary>
        /// Verifies that the main categories navigation is fully loaded and functional.
        /// </summary>
        /// <returns>True if all elements are present and loaded.</returns>
        public bool VerifyMainCategoriesNavigation()
        {
            try
            {
                WaitForPageLoad();
                bool isNavDisplayed = IsMainCategoriesNavDisplayed();
                bool isLabelDisplayed = IsCategoriesLabelDisplayed();
                bool hasCategories = GetCategoryRootItemsCount() > 0;

                return isNavDisplayed && isLabelDisplayed && hasCategories;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Verifies that a specific category exists in the navigation menu.
        /// </summary>
        /// <param name="categoryName">The name of the category to verify.</param>
        /// <returns>True if the category exists in the menu.</returns>
        public bool VerifyCategoryExists(string categoryName)
        {
            try
            {
                var categories = GetAllCategoryNames();
                return categories.Any(cat => cat.Equals(categoryName, StringComparison.OrdinalIgnoreCase) ||
                                            cat.Contains(categoryName, StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Hovers over a specific root-level category by name to reveal its submenu.
        /// This does NOT click the category, only hovers to show subcategories.
        /// </summary>
        /// <param name="categoryName">The name of the category to hover over.</param>
        /// <returns>CategoryPage instance for method chaining.</returns>
        public CategoryPage HoverOverCategoryByName(string categoryName)
        {
            try
            {
                // First hover over the main Categories nav to open the menu
                HoverOverCategories();
                
                var categoryLinks = Driver.FindElements(_categoryRootItemLinks);
                var targetCategory = categoryLinks.FirstOrDefault(link => 
                    link.Text.Trim().Equals(categoryName, StringComparison.OrdinalIgnoreCase) ||
                    link.Text.Contains(categoryName, StringComparison.OrdinalIgnoreCase));

                if (targetCategory != null)
                {
                    HoverOverElement(targetCategory);
                    WaitForDomStable(); // Wait for submenu to appear with reduced timeout
                }
                else
                {
                    throw new Exception($"Category '{categoryName}' not found in navigation");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to hover over category '{categoryName}': {ex.Message}");
            }

            return this;
        }

        /// <summary>
        /// Navigates through a three-level category menu: hover root → hover sub → click grand category
        /// Example: "Women's & Girls' Fashion" → "Bags" → "Wallets"
        /// </summary>
        /// <param name="rootCategory">The root category to hover (e.g., "Women's & Girls' Fashion")</param>
        /// <param name="subCategory">The subcategory to hover (e.g., "Bags")</param>
        /// <param name="grandCategory">The grand category to click (e.g., "Wallets")</param>
        public void NavigateThreeLevelMenu(string rootCategory, string subCategory, string grandCategory)
        {
            try
            {
                HoverOverCategories();
                
                var rootLinks = Driver.FindElements(_categoryRootItemLinks);
                var rootItem = rootLinks.FirstOrDefault(e => 
                    e.Text.Trim().Equals(rootCategory, StringComparison.OrdinalIgnoreCase) ||
                    e.Text.Contains(rootCategory, StringComparison.OrdinalIgnoreCase));
                
                if (rootItem == null)
                {
                    var availableRoots = rootLinks.Select(l => l.Text.Trim()).Take(12).ToList();
                    throw new Exception($"Root category '{rootCategory}' not found. Available: {string.Join(", ", availableRoots)}");
                }
                
                HoverOverElement(rootItem);
                WaitForCondition(d => Driver.FindElements(_subCategoryLinks).Count > 0, 2);
                
                var subLinks = Driver.FindElements(_subCategoryLinks);
                var subItem = subLinks.FirstOrDefault(e => 
                    e.Text.Trim().Equals(subCategory, StringComparison.OrdinalIgnoreCase) ||
                    e.Text.Contains(subCategory, StringComparison.OrdinalIgnoreCase));
                
                if (subItem == null)
                {
                    var availableSubs = subLinks.Select(l => l.Text.Trim()).Where(t => !string.IsNullOrWhiteSpace(t)).Take(15).ToList();
                    throw new Exception($"Subcategory '{subCategory}' not found under '{rootCategory}'. Available: {string.Join(", ", availableSubs)}");
                }
                
                HoverOverElement(subItem);
                WaitForCondition(d => Driver.FindElements(_grandCategoryLinks).Count > 0, 2);
                
                var grandLinks = Driver.FindElements(_grandCategoryLinks);
                var grandItem = grandLinks.FirstOrDefault(e => 
                    e.Text.Trim().Equals(grandCategory, StringComparison.OrdinalIgnoreCase) ||
                    e.Text.Contains(grandCategory, StringComparison.OrdinalIgnoreCase));
                
                if (grandItem == null)
                {
                    var availableGrands = grandLinks.Select(l => l.Text.Trim()).Take(10).ToList();
                    throw new Exception($"Grand category '{grandCategory}' not found under '{subCategory}'. Available: {string.Join(", ", availableGrands)}");
                }
                
                grandItem.Click();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to navigate three-level menu: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the count of products displayed on the current category page.
        /// </summary>
        /// <returns>The number of product items found.</returns>
        public int GetProductCount()
        {
            try
            {
                var products = Driver.FindElements(_productItems);
                return products.Count;
            }
            catch
            {
                return 0;
            }
        }

        public IWebElement GetProductByIndex(int index)
        {
            try
            {
                var products = Driver.FindElements(_productItems);
                
                if (index < 0 || index >= products.Count)
                {
                    throw new Exception($"Product index {index} is out of range. Total products: {products.Count}");
                }
                
                return products[index];
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get product at index {index}: {ex.Message}");
            }
        }

        public string GetProductTitleByIndex(int index)
        {
            try
            {
                var product = GetProductByIndex(index);
                var titleElement = product.FindElement(By.CssSelector(".title, [title], .title-wrapper, .name"));
                string title = titleElement.Text.Trim();
                
                if (string.IsNullOrWhiteSpace(title))
                {
                    title = titleElement.GetAttribute("title");
                }
                
                return title;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get product title at index {index}: {ex.Message}");
            }
        }

        /// <summary>
        /// Clicks on a product at the specified index to navigate to its detail page.
        /// </summary>
        /// <param name="index">The zero-based index of the product (0 = first product).</param>
        /// <returns>CategoryPage instance for method chaining.</returns>
        public CategoryPage ClickProductByIndex(int index)
        {
            int maxRetries = 2;
            int retryCount = 0;
            
            while (retryCount <= maxRetries)
            {
                try
                {
                    var product = GetProductByIndex(index);
                    var productLink = product.FindElement(By.CssSelector("a"));
                    string productUrl = productLink.GetAttribute("href");
                    
                    if (!string.IsNullOrWhiteSpace(productUrl))
                    {
                        // Set a longer page load timeout for product pages (they can be slow)
                        Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(120);
                        
                        try
                        {
                            Console.WriteLine($"Navigating to product URL: {productUrl} (Attempt {retryCount + 1}/{maxRetries + 1})");
                            Driver.Navigate().GoToUrl(productUrl);
                            // Wait for page to be ready
                            WaitForPageLoad(20);
                            Console.WriteLine("Product page loaded successfully");
                            break; // Success - exit retry loop
                        }
                        catch (WebDriverTimeoutException timeoutEx)
                        {
                            retryCount++;
                            if (retryCount > maxRetries)
                            {
                                throw new Exception($"Product page failed to load after {maxRetries + 1} attempts: {timeoutEx.Message}");
                            }
                            Console.WriteLine($"Timeout on attempt {retryCount}. Retrying...");
                            Thread.Sleep(2000); // Wait before retry
                        }
                        finally
                        {
                            // Reset timeout to default
                            Driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(60);
                        }
                    }
                    else
                    {
                        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", productLink);
                        WaitForPageLoad(15);
                        break; // Success - exit retry loop
                    }
                }
                catch (Exception ex) when (retryCount <= maxRetries && ex is not WebDriverTimeoutException)
                {
                    retryCount++;
                    if (retryCount > maxRetries)
                    {
                        throw new Exception($"Failed to click product at index {index} after {maxRetries + 1} attempts: {ex.Message}");
                    }
                    Console.WriteLine($"Error on attempt {retryCount}: {ex.Message}. Retrying...");
                    Thread.Sleep(2000); // Wait before retry
                }
            }

            return this;
        }

        /// <summary>
        /// Adds the currently displayed product to the cart.
        /// This should be called after navigating to a product detail page.
        /// </summary>
        /// <returns>CategoryPage instance for method chaining.</returns>
        public CategoryPage AddProductToCart()
        {
            try
            {
                var addToCartBtn = WaitForElementClickable(By.CssSelector("button.add-to-cart-buy-now-btn.pdp-button_theme_orange"), 5);
                
                if (addToCartBtn == null || !addToCartBtn.Displayed)
                {
                    throw new Exception("Add to Cart button not visible");
                }
                
                try
                {
                    addToCartBtn.Click();
                }
                catch
                {
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", addToCartBtn);
                }
                
                WaitForCondition(d => d.FindElements(_cartSuccessDialog).Count > 0, 3);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add product to cart: {ex.Message}");
            }

            return this;
        }

        /// <summary>
        /// Adds the first product (index 0) from the Wallets category to the cart.
        /// This is a convenience method that combines product selection and adding to cart.
        /// </summary>
        /// <returns>CategoryPage instance for method chaining.</returns>
        public CategoryPage AddFirstWalletProductToCart()
        {
            try
            {
                int productCount = GetProductCount();
                if (productCount == 0)
                {
                    throw new Exception("No products found on the page");
                }
                
                ClickProductByIndex(0);
                AddProductToCart();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to add first wallet product to cart: {ex.Message}");
            }

            return this;
        }

        public int GetCartItemCount()
        {
            try
            {
                var cartCounterSelectors = new[]
                {
                    By.CssSelector(".cart-num"),
                    By.CssSelector(".cart-icon .num"),
                    By.CssSelector("[data-qa-locator='cart-counter']"),
                    By.CssSelector(".cartIcon .num")
                };

                foreach (var selector in cartCounterSelectors)
                {
                    try
                    {
                        var counterElement = Driver.FindElement(selector);
                        if (counterElement.Displayed)
                        {
                            string countText = counterElement.Text.Trim();
                            if (int.TryParse(countText, out int count))
                            {
                                return count;
                            }
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public bool IsCartSuccessMessageDisplayed()
        {
            try
            {
                var messageElement = WaitForElementVisible(_cartSuccessMessage, 10);
                if (messageElement != null && messageElement.Displayed)
                {
                    string messageText = messageElement.Text.Trim();
                    return messageText.Contains("Added to cart successfully");
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public string GetCartSuccessMessageText()
        {
            try
            {
                var messageElement = WaitForElementVisible(_cartSuccessMessage, 10);
                return messageElement != null ? messageElement.Text.Trim() : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        public CategoryPage CloseCartDialog()
        {
            try
            {
                var closeButton = WaitForElementClickable(_cartDialogCloseButton, 5);
                if (closeButton != null && closeButton.Displayed)
                {
                    try
                    {
                        closeButton.Click();
                    }
                    catch
                    {
                        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", closeButton);
                    }
                }
                else
                {
                    throw new Exception("Close button not found or not visible");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to close cart dialog: {ex.Message}");
            }

            return this;
        }

        public bool VerifyAndCloseCartSuccessDialog(string expectedMessage = "Added to cart successfully!")
        {
            try
            {
                bool messageDisplayed = IsCartSuccessMessageDisplayed();
                if (!messageDisplayed)
                {
                    return false;
                }

                string actualMessage = GetCartSuccessMessageText();
                bool messageMatches = actualMessage.Contains(expectedMessage);
                
                CloseCartDialog();

                return messageMatches;
            }
            catch
            {
                return false;
            }
        }

        public int FindProductIndexByName(string nameContains)
        {
            try
            {
                var products = Driver.FindElements(_productItems);
                
                for (int i = 0; i < products.Count; i++)
                {
                    try
                    {
                        var titleElement = products[i].FindElement(By.CssSelector(".title, [title], .title-wrapper, .name"));
                        string title = titleElement.Text.Trim();
                        
                        if (string.IsNullOrWhiteSpace(title))
                        {
                            title = titleElement.GetAttribute("title") ?? "";
                        }
                        
                        if (title.Contains(nameContains, StringComparison.OrdinalIgnoreCase))
                        {
                            return i;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
                
                throw new Exception($"No product found containing '{nameContains}' in title");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to find product by name '{nameContains}': {ex.Message}");
            }
        }

        public CategoryPage ClickProductByName(string nameContains)
        {
            try
            {
                int index = FindProductIndexByName(nameContains);
                return ClickProductByIndex(index);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to click product by name '{nameContains}': {ex.Message}");
            }
        }

        public CategoryPage ClickProductBySelector(Data.CategoryNavigationTestData.ProductSelector selector)
        {
            try
            {
                int productIndex;
                
                if (selector.Index.HasValue)
                {
                    productIndex = selector.Index.Value;
                }
                else if (!string.IsNullOrEmpty(selector.NameContains))
                {
                    productIndex = FindProductIndexByName(selector.NameContains);
                }
                else if (!string.IsNullOrEmpty(selector.Keyword))
                {
                    productIndex = FindProductIndexByName(selector.Keyword);
                }
                else
                {
                    productIndex = 0;
                }
                
                return ClickProductByIndex(productIndex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to click product using selector: {ex.Message}");
            }
        }
    }
}
