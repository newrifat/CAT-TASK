using OpenQA.Selenium;
using DarazAutomation.Base;

namespace DarazAutomation.Pages
{
    /// <summary>
    /// Page Object for the Shopping Cart page.
    /// Handles cart navigation, viewing cart items, and cart operations.
    /// </summary>
    public class CartPage : BasePage
    {
        // Cart Icon and Navigation
        private readonly By _cartIcon = By.CssSelector(".lzd-nav-cart a, .cart-icon-daraz");
        private readonly By _cartIconLink = By.CssSelector(".lzd-nav-cart a[href*='cart']");
        private readonly By _cartCounter = By.CssSelector(".cart-num, #topActionCartNumber");
        
        // Cart Page Elements (Updated based on actual HTML)
        private readonly By _cartPageContainer = By.CssSelector("#container_C, .container");
        private readonly By _cartItems = By.CssSelector(".cart-item");
        private readonly By _cartItemTitles = By.CssSelector(".cart-item .title, a.automation-link-from-title-to-prod");
        private readonly By _cartItemPrices = By.CssSelector(".cart-item .current-price");
        private readonly By _cartItemCheckboxes = By.CssSelector(".cart-item-checkbox input[type='checkbox']");
        private readonly By _emptyCartMessage = By.XPath("//*[contains(text(), 'cart is empty') or contains(text(), 'No items')]");
        private readonly By _selectAllCheckbox = By.XPath("//label[contains(text(), 'Select All')]");
        private readonly By _selectAllLabel = By.XPath("//label[contains(text(), 'Select All')]");
        
        // Cart Summary (Updated based on actual HTML)
        private readonly By _orderSummary = By.CssSelector(".summary-section, .checkout-summary");
        private readonly By _orderSummaryHeading = By.CssSelector(".summary-section-heading");
        private readonly By _subtotalLabel = By.XPath("//div[contains(text(), 'Subtotal')]");
        private readonly By _subtotalValue = By.XPath("//div[contains(text(), 'Subtotal')]/following-sibling::div//span");
        private readonly By _totalLabel = By.CssSelector(".checkout-order-total-title");
        private readonly By _totalValue = By.CssSelector(".checkout-order-total-fee");
        
        // Cart Actions (Updated based on actual HTML)
        private readonly By _checkoutButton = By.CssSelector("button.checkout-order-total-button, .automation-checkout-order-total-button-button");
        private readonly By _deleteButtons = By.CssSelector(".automation-btn-delete");
        private readonly By _voucherInput = By.CssSelector("#automation-voucher-input");
        private readonly By _voucherApplyButton = By.CssSelector("#automation-voucher-input-button");

        // Payment Page Locators
        private readonly By _proceedToPayButton = By.CssSelector("button[type='submit'].next-btn-primary, .next-btn-primary[type='submit'], button:has-text('Proceed to Pay')");
        private readonly By _paymentVoucherMessage = By.XPath("//div[contains(@class, 'next-feedback') and contains(., 'payment voucher')]");
        private readonly By _paymentVoucherMessageTitle = By.CssSelector(".next-feedback-title");

        public CartPage(IWebDriver driver) : base(driver)
        {
        }

        /// <summary>
        /// Navigates to the cart page by clicking the cart icon in the header.
        /// </summary>
        /// <returns>CartPage instance for method chaining.</returns>
        public CartPage NavigateToCart()
        {
            try
            {
                string currentUrl = Driver.Url.ToLower();
                if (!currentUrl.Contains("daraz.com.bd"))
                {
                    Driver.Navigate().GoToUrl("https://www.daraz.com.bd/");
                    WaitForPageLoad();
                }

                By[] cartIconSelectors = new[]
                {
                    By.Id("anonCartIcon"),
                    By.CssSelector("a[href*='/cart']"),
                    By.XPath("//a[contains(@href, '/cart')]"),
                    By.CssSelector(".cart-icon")
                };

                IWebElement? cartLink = null;
                foreach (var selector in cartIconSelectors)
                {
                    try
                    {
                        cartLink = Wait.Until(d => d.FindElement(selector));
                        if (cartLink != null && cartLink.Displayed) break;
                    }
                    catch { }
                }

                if (cartLink != null && cartLink.Displayed)
                {
                    string cartUrl = cartLink.GetAttribute("href");
                    
                    if (!string.IsNullOrEmpty(cartUrl))
                    {
                        Driver.Navigate().GoToUrl(cartUrl);
                    }
                    else
                    {
                        try
                        {
                            cartLink.Click();
                        }
                        catch
                        {
                            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", cartLink);
                        }
                    }
                    
                    WaitForPageLoad();
                    
                    if (!Driver.Url.ToLower().Contains("cart"))
                    {
                        Driver.Navigate().GoToUrl("https://cart.daraz.com.bd/cart");
                        WaitForPageLoad();
                    }
                }
                else
                {
                    Driver.Navigate().GoToUrl("https://cart.daraz.com.bd/cart");
                    WaitForPageLoad();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Driver.Navigate().GoToUrl("https://cart.daraz.com.bd/cart");
                    WaitForPageLoad();
                }
                catch (Exception fallbackEx)
                {
                    throw new Exception($"Failed to navigate to cart: {fallbackEx.Message}");
                }
            }

            return this;
        }

        /// <summary>
        /// Verifies that the cart page is loaded.
        /// </summary>
        /// <returns>True if cart page is loaded successfully.</returns>
        public bool IsCartPageLoaded()
        {
            try
            {
                WaitForPageLoad();
                return Driver.Url.ToLower().Contains("cart");
            }
            catch
            {
                return false;
            }
        }

        public int GetCartCounterValue()
        {
            try
            {
                var counterElement = WaitForElementVisible(_cartCounter, 5);
                if (counterElement != null && counterElement.Displayed)
                {
                    string countText = counterElement.Text.Trim();
                    if (int.TryParse(countText, out int count))
                    {
                        return count;
                    }
                }
                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public int GetCartItemsCount()
        {
            try
            {
                WaitForPageReady();
                return Driver.FindElements(_cartItems).Count;
            }
            catch
            {
                return 0;
            }
        }

        public bool IsCartEmpty()
        {
            try
            {
                WaitForPageReady();
                bool hasEmptyMessage = IsElementDisplayed(_emptyCartMessage, 5);
                if (hasEmptyMessage) return true;
                
                return GetCartItemsCount() == 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the titles of all items in the cart.
        /// </summary>
        /// <returns>A list of product titles in the cart.</returns>
        public List<string> GetCartItemTitles()
        {
            var titles = new List<string>();
            try
            {
                WaitForPageReady();
                
                var titleElements = Driver.FindElements(_cartItemTitles);
                foreach (var element in titleElements)
                {
                    try
                    {
                        string title = element.Text.Trim();
                        if (!string.IsNullOrWhiteSpace(title))
                        {
                            titles.Add(title);
                            Console.WriteLine($"Cart item: {title}");
                        }
                    }
                    catch { }
                }
                
                Console.WriteLine($"Total cart items retrieved: {titles.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting cart item titles: {ex.Message}");
            }
            
            return titles;
        }

        /// <summary>
        /// Verifies that the cart contains a specific number of items.
        /// </summary>
        /// <param name="expectedCount">The expected number of items.</param>
        /// <returns>True if the cart has the expected number of items.</returns>
        public bool VerifyCartItemCount(int expectedCount)
        {
            try
            {
                int actualCount = GetCartItemsCount();
                bool matches = actualCount == expectedCount;
                
                if (matches)
                {
                    Console.WriteLine($"✓ Cart item count verified: {actualCount} items");
                }
                else
                {
                    Console.WriteLine($"✗ Cart item count mismatch. Expected: {expectedCount}, Actual: {actualCount}");
                }
                
                return matches;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying cart item count: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Checks if the checkout button is displayed and enabled.
        /// </summary>
        /// <returns>True if checkout button is available.</returns>
        public bool IsCheckoutButtonAvailable()
        {
            try
            {
                var checkoutBtn = WaitForElementVisible(_checkoutButton, 10);
                if (checkoutBtn != null && checkoutBtn.Displayed && checkoutBtn.Enabled)
                {
                    Console.WriteLine("Checkout button is available");
                    return true;
                }
                Console.WriteLine("Checkout button not available");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking checkout button: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets the text from the checkout button (includes item count).
        /// </summary>
        /// <returns>The checkout button text, e.g., "PROCEED TO CHECKOUT (9)"</returns>
        public string GetCheckoutButtonText()
        {
            try
            {
                var checkoutBtn = WaitForElementVisible(_checkoutButton, 10);
                if (checkoutBtn != null)
                {
                    string buttonText = checkoutBtn.Text.Trim();
                    Console.WriteLine($"Checkout button text: '{buttonText}'");
                    return buttonText;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting checkout button text: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Verifies that the Order Summary section is displayed.
        /// </summary>
        /// <returns>True if order summary is visible.</returns>
        public bool IsOrderSummaryDisplayed()
        {
            try
            {
                var summaryHeading = WaitForElementVisible(_orderSummaryHeading, 10);
                if (summaryHeading != null && summaryHeading.Displayed)
                {
                    string headingText = summaryHeading.Text.Trim();
                    Console.WriteLine($"Order summary heading: '{headingText}'");
                    return headingText.Contains("Order Summary");
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking order summary: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets the subtotal amount from the order summary.
        /// </summary>
        /// <returns>The subtotal text, e.g., "৳ 732"</returns>
        public string GetSubtotalAmount()
        {
            try
            {
                var subtotalValueElement = WaitForElementVisible(_subtotalValue, 10);
                if (subtotalValueElement != null)
                {
                    string subtotal = subtotalValueElement.Text.Trim();
                    Console.WriteLine($"Subtotal: {subtotal}");
                    return subtotal;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting subtotal: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the total amount from the order summary.
        /// </summary>
        /// <returns>The total amount text, e.g., "৳ 732"</returns>
        public string GetTotalAmount()
        {
            try
            {
                var totalValueElement = WaitForElementVisible(_totalValue, 10);
                if (totalValueElement != null)
                {
                    string total = totalValueElement.Text.Trim();
                    // Extract just the amount (before any additional text)
                    if (total.Contains(" "))
                    {
                        total = total.Split(' ')[0] + " " + total.Split(' ')[1];
                    }
                    Console.WriteLine($"Total: {total}");
                    return total;
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting total: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Clicks on a specific cart item checkbox by index (0-based).
        /// </summary>
        /// <param name="index">The index of the cart item (0 = first item).</param>
        /// <returns>CartPage instance for method chaining.</returns>
        public CartPage SelectCartItemByIndex(int index)
        {
            try
            {
                Console.WriteLine($"Selecting cart item at index {index}...");
                WaitForPageLoad();

                var checkboxes = Driver.FindElements(_cartItemCheckboxes);
                if (index < 0 || index >= checkboxes.Count)
                {
                    throw new Exception($"Cart item index {index} is out of range. Total items: {checkboxes.Count}");
                }

                var checkbox = checkboxes[index];
                
                // Check if already selected
                bool isChecked = checkbox.GetAttribute("aria-checked") == "true" || checkbox.Selected;
                
                if (!isChecked)
                {
                    // Use JavaScript click for reliability (no need to scroll)
                    try
                    {
                        // Try regular click first
                        checkbox.Click();
                    }
                    catch
                    {
                        // Fallback to JavaScript click
                        Console.WriteLine($"Using JavaScript click for checkbox at index {index}");
                        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", checkbox);
                    }
                    
                    WaitForDomStable(); // Wait for selection to update
                    Console.WriteLine($"✓ Selected cart item at index {index}");
                }
                else
                {
                    Console.WriteLine($"Cart item at index {index} is already selected");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to select cart item at index {index}: {ex.Message}");
                throw new Exception($"Failed to select cart item: {ex.Message}");
            }

            return this;
        }

        /// <summary>
        /// Selects multiple cart items by their indices.
        /// </summary>
        /// <param name="indices">Array of indices to select (0-based).</param>
        /// <returns>CartPage instance for method chaining.</returns>
        public CartPage SelectCartItems(params int[] indices)
        {
            foreach (int index in indices)
            {
                SelectCartItemByIndex(index);
            }
            
            // Wait for the order summary to refresh after selecting items
            System.Threading.Thread.Sleep(2000); // Give UI time to update
            Console.WriteLine("Waiting for order summary to refresh after item selection...");
            
            return this;
        }

        /// <summary>
        /// Gets the count of selected items from the "Select All" label text.
        /// Example: "Select All (9 item(s))" returns 9
        /// </summary>
        /// <returns>The number of items shown in the select all label.</returns>
        public int GetSelectAllItemCount()
        {
            try
            {
                var selectAllElement = WaitForElementVisible(_selectAllLabel, 10);
                if (selectAllElement != null)
                {
                    string labelText = selectAllElement.Text.Trim();
                    Console.WriteLine($"Select All label text: '{labelText}'");
                    
                    // Extract number from text like "Select All (9 item(s))"
                    var match = System.Text.RegularExpressions.Regex.Match(labelText, @"\((\d+)\s+item");
                    if (match.Success)
                    {
                        int count = int.Parse(match.Groups[1].Value);
                        Console.WriteLine($"Total items in cart (from Select All): {count}");
                        return count;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting select all item count: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Gets all product prices from the cart items.
        /// </summary>
        /// <returns>A list of price strings, e.g., ["৳ 49", "৳ 83"]</returns>
        public List<string> GetAllCartItemPrices()
        {
            var prices = new List<string>();
            try
            {
                WaitForPageReady();
                
                var priceElements = Driver.FindElements(_cartItemPrices);
                foreach (var element in priceElements)
                {
                    try
                    {
                        string price = element.Text.Trim();
                        if (!string.IsNullOrWhiteSpace(price))
                        {
                            prices.Add(price);
                            Console.WriteLine($"Product price: {price}");
                        }
                    }
                    catch { }
                }
                
                Console.WriteLine($"Total prices retrieved: {prices.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting cart item prices: {ex.Message}");
            }
            
            return prices;
        }

        /// <summary>
        /// Calculates the sum of selected cart item prices.
        /// Assumes prices are in format "৳ 123" or similar.
        /// </summary>
        /// <param name="selectedIndices">The indices of selected items.</param>
        /// <returns>The calculated total price.</returns>
        public decimal CalculateSelectedItemsTotal(params int[] selectedIndices)
        {
            try
            {
                var allPrices = GetAllCartItemPrices();
                decimal total = 0;

                foreach (int index in selectedIndices)
                {
                    if (index >= 0 && index < allPrices.Count)
                    {
                        string priceText = allPrices[index];
                        // Extract numeric value from price string like "৳ 49"
                        string numericPart = System.Text.RegularExpressions.Regex.Match(priceText, @"[\d,]+").Value.Replace(",", "");
                        
                        if (decimal.TryParse(numericPart, out decimal price))
                        {
                            total += price;
                            Console.WriteLine($"Item {index}: {priceText} = {price}");
                        }
                    }
                }

                Console.WriteLine($"Calculated total for selected items: ৳ {total}");
                return total;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error calculating total: {ex.Message}");
                return 0;
            }
        }

        /// <summary>
        /// Verifies that the order summary subtotal matches the expected amount.
        /// Use this method to verify item prices before shipping/fees are added.
        /// Waits for the subtotal to update after item selection.
        /// </summary>
        /// <param name="expectedSubtotal">The expected subtotal amount.</param>
        /// <returns>True if subtotals match.</returns>
        public bool VerifyOrderSummarySubtotal(decimal expectedSubtotal)
        {
            try
            {
                Console.WriteLine($"Waiting for order summary to refresh with expected subtotal: ৳ {expectedSubtotal}");
                
                // Wait for the subtotal to update (up to 10 seconds)
                try
                {
                    WaitForCondition(driver =>
                    {
                        try
                        {
                            string subtotalText = GetSubtotalAmount();
                            string numericPart = System.Text.RegularExpressions.Regex.Match(subtotalText, @"[\d,]+").Value.Replace(",", "");
                            
                            if (decimal.TryParse(numericPart, out decimal currentSubtotal))
                            {
                                Console.WriteLine($"Current subtotal: ৳ {currentSubtotal}, Expected: ৳ {expectedSubtotal}");
                                return currentSubtotal == expectedSubtotal;
                            }
                            return false;
                        }
                        catch
                        {
                            return false;
                        }
                    }, timeoutSeconds: 10);
                }
                catch (WebDriverTimeoutException)
                {
                    Console.WriteLine("Timeout waiting for subtotal to update, proceeding with verification...");
                }
                
                // Get final subtotal for verification
                string subtotalText = GetSubtotalAmount();
                string numericPart = System.Text.RegularExpressions.Regex.Match(subtotalText, @"[\d,]+").Value.Replace(",", "");
                
                if (decimal.TryParse(numericPart, out decimal actualSubtotal))
                {
                    bool matches = actualSubtotal == expectedSubtotal;
                    
                    if (matches)
                    {
                        Console.WriteLine($"✓ Order summary subtotal verified: ৳ {actualSubtotal}");
                    }
                    else
                    {
                        Console.WriteLine($"✗ Order summary subtotal mismatch. Expected: ৳ {expectedSubtotal}, Actual: ৳ {actualSubtotal}");
                    }
                    
                    return matches;
                }
                
                Console.WriteLine($"Could not parse subtotal amount: {subtotalText}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying order summary subtotal: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Verifies that the order summary total matches the expected amount.
        /// </summary>
        /// <param name="expectedTotal">The expected total amount.</param>
        /// <returns>True if totals match.</returns>
        public bool VerifyOrderSummaryTotal(decimal expectedTotal)
        {
            try
            {
                string totalText = GetTotalAmount();
                // Extract numeric value from total string like "৳ 732"
                string numericPart = System.Text.RegularExpressions.Regex.Match(totalText, @"[\d,]+").Value.Replace(",", "");
                
                if (decimal.TryParse(numericPart, out decimal actualTotal))
                {
                    bool matches = actualTotal == expectedTotal;
                    
                    if (matches)
                    {
                        Console.WriteLine($"✓ Order summary total verified: ৳ {actualTotal}");
                    }
                    else
                    {
                        Console.WriteLine($"✗ Order summary mismatch. Expected: ৳ {expectedTotal}, Actual: ৳ {actualTotal}");
                    }
                    
                    return matches;
                }
                
                Console.WriteLine($"Could not parse total amount: {totalText}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying order summary: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Clicks the "PROCEED TO CHECKOUT" button.
        /// </summary>
        /// <returns>CartPage instance for method chaining.</returns>
        public CartPage ClickProceedToCheckout()
        {
            try
            {
                Console.WriteLine("Clicking 'PROCEED TO CHECKOUT' button...");
                
                var checkoutBtn = WaitForElementClickable(_checkoutButton, 10);
                if (checkoutBtn != null && checkoutBtn.Displayed && checkoutBtn.Enabled)
                {
                    string buttonText = checkoutBtn.Text.Trim();
                    Console.WriteLine($"Checkout button text: '{buttonText}'");
                    
                    try
                    {
                        checkoutBtn.Click();
                    }
                    catch
                    {
                        // Fallback to JavaScript click
                        Console.WriteLine("Using JavaScript to click checkout button...");
                        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", checkoutBtn);
                    }
                    
                    // Wait longer for checkout page (different subdomain)
                    WaitForPageLoad(15);
                    System.Threading.Thread.Sleep(2000); // Extra time for page to stabilize
                    
                    Console.WriteLine($"✓ Clicked checkout button. Current URL: {Driver.Url}");
                }
                else
                {
                    throw new Exception("Checkout button not available");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to click checkout button: {ex.Message}");
                throw new Exception($"Failed to proceed to checkout: {ex.Message}");
            }

            return this;
        }

        /// <summary>
        /// Verifies that we have navigated to the checkout page.
        /// Checks for URL pattern: https://checkout.daraz.com.bd/shipping
        /// and presence of "Shipping & Billing" section.
        /// </summary>
        /// <returns>True if on checkout page.</returns>
        public bool IsOnCheckoutPage()
        {
            try
            {
                // Wait longer for checkout page to load (it's on a different subdomain)
                WaitForPageLoad(15);
                
                // Give extra time for the page to stabilize
                System.Threading.Thread.Sleep(2000);
                
                string currentUrl = Driver.Url;
                Console.WriteLine($"Current URL: {currentUrl}");
                
                // Check if URL matches the checkout shipping page pattern
                // URL should be: https://checkout.daraz.com.bd/shipping (with optional query params)
                bool urlMatches = currentUrl.StartsWith("https://checkout.daraz.com.bd/shipping", StringComparison.OrdinalIgnoreCase);
                
                if (urlMatches)
                {
                    Console.WriteLine($"✓ URL matches checkout shipping page pattern - On checkout page!");
                    return true;
                }
                else
                {
                    Console.WriteLine($"✗ URL does not match expected pattern: https://checkout.daraz.com.bd/shipping");
                    Console.WriteLine($"  Actual URL: {currentUrl}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking checkout page: {ex.Message}");
                return false;
            }
        }

        #region Checkout Page Verification Methods

        /// <summary>
        /// Verifies that the "Shipping & Billing" section is displayed on checkout page.
        /// </summary>
        /// <returns>True if shipping & billing section is displayed.</returns>
        public bool IsShippingAndBillingSectionDisplayed()
        {
            try
            {
                // Check for the main address container
                var addressContainer = Wait.Until(d => d.FindElement(By.CssSelector(".v2-checkout-address")));
                bool isDisplayed = addressContainer.Displayed;
                
                if (isDisplayed)
                {
                    Console.WriteLine("✓ Shipping & Billing section is displayed");
                }
                
                return isDisplayed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ Shipping & Billing section not found: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets the shipping & billing header text from checkout page.
        /// </summary>
        /// <returns>The header text (e.g., "Shipping & Billing").</returns>
        public string GetShippingAndBillingHeaderText()
        {
            try
            {
                var headerElement = Wait.Until(d => d.FindElement(By.CssSelector(".v2-address-hat")));
                string headerText = headerElement.Text.Trim();
                Console.WriteLine($"Shipping header: '{headerText}'");
                return headerText;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting shipping header: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the recipient name from the shipping address on checkout page.
        /// </summary>
        /// <returns>The recipient name.</returns>
        public string GetRecipientName()
        {
            try
            {
                var nameElement = Wait.Until(d => d.FindElement(By.CssSelector(".v2-address-title")));
                string name = nameElement.Text.Trim();
                Console.WriteLine($"Recipient name: '{name}'");
                return name;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting recipient name: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the mobile number from the shipping address on checkout page.
        /// </summary>
        /// <returns>The mobile number.</returns>
        public string GetMobileNumber()
        {
            try
            {
                var mobileElement = Wait.Until(d => d.FindElement(By.CssSelector(".v2-mobile")));
                string mobile = mobileElement.Text.Trim();
                Console.WriteLine($"Mobile number: '{mobile}'");
                return mobile;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting mobile number: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the address tag label (e.g., "HOME", "OFFICE") from checkout page.
        /// </summary>
        /// <returns>The address tag label.</returns>
        public string GetAddressTagLabel()
        {
            try
            {
                var tagElement = Wait.Until(d => d.FindElement(By.CssSelector(".v2-address-tag-label")));
                string tag = tagElement.Text.Trim();
                Console.WriteLine($"Address tag: '{tag}'");
                return tag;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting address tag: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the full shipping address from checkout page.
        /// </summary>
        /// <returns>The full address text.</returns>
        public string GetShippingAddress()
        {
            try
            {
                var addressInfoItems = Driver.FindElements(By.CssSelector(".v2-address-info-item"));
                
                if (addressInfoItems.Count > 0)
                {
                    // Get the address text (skip the tag label)
                    var addressSpans = addressInfoItems[0].FindElements(By.TagName("span"));
                    if (addressSpans.Count > 1)
                    {
                        string address = addressSpans[1].Text.Trim();
                        Console.WriteLine($"Shipping address: '{address}'");
                        return address;
                    }
                }
                
                Console.WriteLine("Address not found in expected format");
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting shipping address: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Verifies that the EDIT button is displayed in the shipping & billing section.
        /// </summary>
        /// <returns>True if EDIT button is displayed.</returns>
        public bool IsEditButtonDisplayed()
        {
            try
            {
                var editButton = Wait.Until(d => d.FindElement(By.CssSelector(".v2-title-wrapper-edit")));
                bool isDisplayed = editButton.Displayed;
                
                if (isDisplayed)
                {
                    Console.WriteLine("✓ EDIT button is displayed");
                }
                
                return isDisplayed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ EDIT button not found: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Verifies that the "Proceed to Pay" button is displayed on checkout page.
        /// </summary>
        /// <returns>True if Proceed to Pay button is displayed.</returns>
        public bool IsProceedToPayButtonDisplayed()
        {
            try
            {
                // The button text is "Proceed to Pay"
                var proceedButton = Wait.Until(d => 
                {
                    var buttons = d.FindElements(By.XPath("//div[contains(text(), 'Proceed to Pay')]"));
                    return buttons.FirstOrDefault(b => b.Displayed);
                });
                
                bool isDisplayed = proceedButton != null && proceedButton.Displayed;
                
                if (isDisplayed)
                {
                    Console.WriteLine("✓ 'Proceed to Pay' button is displayed");
                }
                
                return isDisplayed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"✗ 'Proceed to Pay' button not found: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets the text of the "Proceed to Pay" button.
        /// </summary>
        /// <returns>The button text.</returns>
        public string GetProceedToPayButtonText()
        {
            try
            {
                var proceedButton = Wait.Until(d => 
                {
                    var buttons = d.FindElements(By.XPath("//div[contains(text(), 'Proceed to Pay')]"));
                    return buttons.FirstOrDefault(b => b.Displayed);
                });
                
                if (proceedButton != null)
                {
                    string buttonText = proceedButton.Text.Trim();
                    Console.WriteLine($"Proceed to Pay button text: '{buttonText}'");
                    return buttonText;
                }
                
                Console.WriteLine("Proceed to Pay button not found");
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting Proceed to Pay button text: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Clicks the "Proceed to Pay" button on the checkout page to navigate to payment page.
        /// </summary>
        /// <returns>CartPage instance for method chaining.</returns>
        public CartPage ClickProceedToPay()
        {
            try
            {
                Console.WriteLine("Clicking 'Proceed to Pay' button...");
                
                // Try multiple selectors - prioritize the one that works for display check
                By[] buttonSelectors = new[]
                {
                    // First try div elements (the display check uses this and it works)
                    By.XPath("//div[contains(text(), 'Proceed to Pay')]"),
                    By.XPath("//*[contains(text(), 'Proceed to Pay')]"),
                    // Then try button elements
                    By.XPath("//button[contains(text(), 'Proceed to Pay')]"),
                    By.CssSelector("button[type='submit'].next-btn-primary"),
                    By.CssSelector(".next-btn-primary[type='submit']"),
                    By.XPath("//button[@type='submit' and contains(@class, 'primary')]")
                };

                IWebElement? proceedButton = null;
                foreach (var selector in buttonSelectors)
                {
                    try
                    {
                        var elements = Driver.FindElements(selector);
                        proceedButton = elements.FirstOrDefault(e => e.Displayed);
                        
                        if (proceedButton != null)
                        {
                            Console.WriteLine($"Found 'Proceed to Pay' button using selector: {selector}");
                            break;
                        }
                    }
                    catch
                    {
                        // Continue to next selector
                    }
                }

                if (proceedButton != null && proceedButton.Displayed)
                {
                    string buttonText = proceedButton.Text.Trim();
                    Console.WriteLine($"'Proceed to Pay' button text: '{buttonText}'");
                    
                    try
                    {
                        proceedButton.Click();
                        Console.WriteLine("✓ Clicked using regular click");
                    }
                    catch
                    {
                        // Fallback to JavaScript click
                        Console.WriteLine("Using JavaScript to click 'Proceed to Pay' button...");
                        ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", proceedButton);
                    }
                    
                    WaitForPageLoad(10); // Wait for payment page to load
                    System.Threading.Thread.Sleep(1000); // Extra stabilization time
                    Console.WriteLine($"✓ Clicked 'Proceed to Pay'. Current URL: {Driver.Url}");
                }
                else
                {
                    throw new Exception("'Proceed to Pay' button not found or not available");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to click 'Proceed to Pay' button: {ex.Message}");
                throw new Exception($"Failed to click 'Proceed to Pay': {ex.Message}");
            }

            return this;
        }

        /// <summary>
        /// Verifies that the payment voucher promotional message is displayed on the payment page.
        /// Expected message: "Collect payment voucher & get extra savings on your purchase!"
        /// </summary>
        /// <returns>True if the payment voucher message is displayed.</returns>
        public bool IsPaymentVoucherMessageDisplayed()
        {
            try
            {
                Console.WriteLine("Checking for payment voucher message...");
                WaitForPageLoad(); // Only wait for page load
                
                // Try multiple strategies to find the message
                var strategies = new List<By>
                {
                    By.XPath("//div[contains(@class, 'next-feedback') and contains(., 'payment voucher')]"),
                    By.XPath("//div[contains(@class, 'next-feedback-title') and contains(., 'payment voucher')]"),
                    By.XPath("//*[contains(text(), 'Collect payment voucher')]"),
                    By.CssSelector(".next-feedback-title")
                };

                foreach (var strategy in strategies)
                {
                    try
                    {
                        var messageElement = WaitForElementVisible(strategy, 5);
                        if (messageElement != null && messageElement.Displayed)
                        {
                            string text = messageElement.Text.ToLower();
                            if (text.Contains("payment voucher") || text.Contains("extra savings"))
                            {
                                Console.WriteLine($"✓ Payment voucher message found: '{messageElement.Text}'");
                                return true;
                            }
                        }
                    }
                    catch
                    {
                        // Continue to next strategy
                    }
                }

                Console.WriteLine("✗ Payment voucher message not found");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking payment voucher message: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Gets the text of the payment voucher promotional message.
        /// </summary>
        /// <returns>The payment voucher message text, or empty string if not found.</returns>
        public string GetPaymentVoucherMessageText()
        {
            try
            {
                // Try multiple strategies to find the message
                var strategies = new List<By>
                {
                    By.XPath("//div[contains(@class, 'next-feedback-title') and contains(., 'payment voucher')]"),
                    By.XPath("//div[contains(@class, 'next-feedback') and contains(., 'payment voucher')]//div[contains(@class, 'title')]"),
                    By.XPath("//*[contains(text(), 'Collect payment voucher')]")
                };

                foreach (var strategy in strategies)
                {
                    try
                    {
                        var messageElement = WaitForElementVisible(strategy, 5);
                        if (messageElement != null && messageElement.Displayed)
                        {
                            string text = messageElement.Text.Trim();
                            if (text.Contains("payment voucher", StringComparison.OrdinalIgnoreCase))
                            {
                                Console.WriteLine($"Payment voucher message: '{text}'");
                                return text;
                            }
                        }
                    }
                    catch
                    {
                        // Continue to next strategy
                    }
                }

                Console.WriteLine("Payment voucher message not found");
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting payment voucher message: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Verifies that the payment voucher message contains the expected text.
        /// </summary>
        /// <param name="expectedText">The expected text to verify (default: "Collect payment voucher & get extra savings on your purchase!")</param>
        /// <returns>True if the message matches the expected text.</returns>
        public bool VerifyPaymentVoucherMessage(string expectedText = "Collect payment voucher & get extra savings on your purchase!")
        {
            try
            {
                string actualMessage = GetPaymentVoucherMessageText();
                
                if (string.IsNullOrEmpty(actualMessage))
                {
                    Console.WriteLine("✗ Payment voucher message not found");
                    return false;
                }

                bool matches = actualMessage.Equals(expectedText, StringComparison.OrdinalIgnoreCase) ||
                              actualMessage.Contains(expectedText, StringComparison.OrdinalIgnoreCase);

                if (matches)
                {
                    Console.WriteLine($"✓ Payment voucher message verified: '{actualMessage}'");
                }
                else
                {
                    Console.WriteLine($"✗ Message mismatch.");
                    Console.WriteLine($"  Expected: '{expectedText}'");
                    Console.WriteLine($"  Actual: '{actualMessage}'");
                }

                return matches;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error verifying payment voucher message: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Verifies we are on the payment page.
        /// </summary>
        /// <returns>True if on payment page.</returns>
        public bool IsOnPaymentPage()
        {
            try
            {
                string currentUrl = Driver.Url.ToLower();
                bool onPaymentPage = currentUrl.Contains("payment") || 
                                    currentUrl.Contains("/pay") ||
                                    currentUrl.Contains("checkout");
                
                Console.WriteLine($"Current URL: {Driver.Url}");
                Console.WriteLine($"On payment page: {onPaymentPage}");
                
                return onPaymentPage;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking payment page: {ex.Message}");
                return false;
            }
        }

        #endregion

        /// <summary>
        /// Prints a summary of the cart contents.
        /// </summary>
        public void PrintCartSummary()
        {
            try
            {
                Console.WriteLine("\n========== CART SUMMARY ==========");
                Console.WriteLine($"Cart URL: {Driver.Url}");
                
                int counterValue = GetCartCounterValue();
                Console.WriteLine($"Cart Counter: {counterValue} items");
                
                int itemsOnPage = GetCartItemsCount();
                Console.WriteLine($"Items on Cart Page: {itemsOnPage}");
                
                bool isEmpty = IsCartEmpty();
                Console.WriteLine($"Cart is Empty: {isEmpty}");
                
                if (!isEmpty)
                {
                    Console.WriteLine("\nCart Items:");
                    var titles = GetCartItemTitles();
                    for (int i = 0; i < titles.Count; i++)
                    {
                        Console.WriteLine($"  {i + 1}. {titles[i]}");
                    }
                    
                    Console.WriteLine("\nOrder Summary:");
                    bool summaryDisplayed = IsOrderSummaryDisplayed();
                    Console.WriteLine($"  Order Summary Displayed: {summaryDisplayed}");
                    
                    if (summaryDisplayed)
                    {
                        string subtotal = GetSubtotalAmount();
                        string total = GetTotalAmount();
                        Console.WriteLine($"  Subtotal: {subtotal}");
                        Console.WriteLine($"  Total: {total}");
                    }
                    
                    bool checkoutAvailable = IsCheckoutButtonAvailable();
                    Console.WriteLine($"\nCheckout Available: {checkoutAvailable}");
                    
                    if (checkoutAvailable)
                    {
                        string checkoutText = GetCheckoutButtonText();
                        Console.WriteLine($"  Checkout Button: {checkoutText}");
                    }
                }
                
                Console.WriteLine("==================================\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error printing cart summary: {ex.Message}");
            }
        }

        /// <summary>
        /// Debug method to print all cart-related elements on the page.
        /// </summary>
        public void DebugPrintCartElements()
        {
            try
            {
                Console.WriteLine("\n=== DEBUG: Cart Page Elements ===");
                Console.WriteLine($"Current URL: {Driver.Url}");
                
                // Find all elements with 'cart' in class or id
                var cartElements = Driver.FindElements(By.XPath("//*[contains(@id, 'cart') or contains(@class, 'cart')]"));
                Console.WriteLine($"Found {cartElements.Count} elements with 'cart' in id/class");
                
                foreach (var elem in cartElements.Take(15))
                {
                    try
                    {
                        string tag = elem.TagName;
                        string id = elem.GetAttribute("id");
                        string className = elem.GetAttribute("class");
                        string text = elem.Text.Length > 50 ? elem.Text.Substring(0, 50) + "..." : elem.Text;
                        bool displayed = elem.Displayed;
                        
                        Console.WriteLine($"  - Tag: {tag}, ID: {id}, Class: {className}, Text: '{text}', Displayed: {displayed}");
                    }
                    catch { }
                }
                
                Console.WriteLine("=== END DEBUG ===\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Debug failed: {ex.Message}");
            }
        }
    }
}
