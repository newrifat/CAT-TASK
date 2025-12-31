using OpenQA.Selenium;
using DarazAutomation.Base;

namespace DarazAutomation.Pages
{
    /// <summary>
    /// Page Object for Daraz Product Detail Page
    /// Contains all locators and methods for interacting with product details and cart actions
    /// </summary>
    public class ProductPage : BasePage
    {
        #region Locators

        private readonly By _productTitle = By.CssSelector(".pdp-mod-product-badge-title, h1.pdp-product-title");
        private readonly By _productPrice = By.CssSelector(".pdp-price .pdp-price_type_normal, .pdp-product-price span");
        private readonly By _originalPrice = By.CssSelector(".pdp-price .pdp-price_type_deleted");
        private readonly By _discountBadge = By.CssSelector(".pdp-product-discount");
        private readonly By _productRating = By.CssSelector(".pdp-review-summary .score");
        private readonly By _sellerName = By.CssSelector(".pdp-seller-name");
        private readonly By _productMainImage = By.CssSelector(".pdp-mod-common-image img, .gallery-preview-panel img");
        private readonly By _sizeOptions = By.CssSelector(".sku-selector .sku-variable");
        private readonly By _selectedVariant = By.CssSelector(".sku-selector .sku-variable.selected");
        private readonly By _variantError = By.CssSelector(".sku-selector .error, .pdp-sku-error");
        private readonly By _quantityInput = By.CssSelector(".next-number-picker-input input");
        private readonly By _increaseQuantityBtn = By.CssSelector(".next-number-picker-handler-up-inner");
        private readonly By _decreaseQuantityBtn = By.CssSelector(".next-number-picker-handler-down-inner");
        private readonly By _stockStatus = By.CssSelector(".pdp-stock");
        private readonly By _addToCartButton = By.CssSelector("button.add-to-cart-buy-now-btn.pdp-button_theme_orange");
        private readonly By _buyNowButton = By.CssSelector("button.pdp-button_theme_red");
        private readonly By _addToCartSuccess = By.CssSelector(".next-message-success, .add-to-cart-success");
        private readonly By _cartCountBadge = By.CssSelector(".cart-num, .cart-count, #anonCart span");
        private readonly By _loginRequiredPopup = By.CssSelector(".login-popup, .next-dialog-body");
        private readonly By _outOfStockMessage = By.CssSelector(".out-of-stock, .stock-unavailable");
        private readonly By _errorMessage = By.CssSelector(".error-message, .pdp-error");
        private readonly By _cartIcon = By.CssSelector("#anonCart, #myCart, [data-spm='cart']");

        #endregion

        #region Constructor

        public ProductPage(IWebDriver driver) : base(driver)
        {
        }

        #endregion

        #region Page Verification Methods

        /// <summary>
        /// Verifies if the product page is loaded
        /// </summary>
        public bool IsPageLoaded()
        {
            try
            {
                WaitForPageLoad();
                return IsElementDisplayed(_productTitle, 10) || 
                       IsElementDisplayed(_addToCartButton, 10);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Waits for the product page to fully load
        /// </summary>
        public ProductPage WaitForPageToLoad(int timeoutSeconds = 15)
        {
            try
            {
                WaitForPageLoad();
                WaitForElementVisible(_productTitle, timeoutSeconds);
            }
            catch
            {
            }
            return this;
        }

        #endregion

        #region Product Information Methods

        /// <summary>
        /// Gets the product title/name
        /// </summary>
        public string GetProductTitle()
        {
            try
            {
                var title = WaitForElementVisible(_productTitle, 10);
                return title.Text.Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the current/sale price
        /// </summary>
        public string GetProductPrice()
        {
            try
            {
                var price = WaitForElementVisible(_productPrice, 5);
                return price.Text.Trim();
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the original price (before discount)
        /// </summary>
        public string GetOriginalPrice()
        {
            try
            {
                if (IsElementDisplayed(_originalPrice, 3))
                {
                    var price = Driver.FindElement(_originalPrice);
                    return price.Text.Trim();
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the discount percentage
        /// </summary>
        public string GetDiscountPercentage()
        {
            try
            {
                if (IsElementDisplayed(_discountBadge, 3))
                {
                    var discount = Driver.FindElement(_discountBadge);
                    return discount.Text.Trim();
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the product rating
        /// </summary>
        public string GetProductRating()
        {
            try
            {
                if (IsElementDisplayed(_productRating, 3))
                {
                    var rating = Driver.FindElement(_productRating);
                    return rating.Text.Trim();
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Gets the seller/shop name
        /// </summary>
        public string GetSellerName()
        {
            try
            {
                if (IsElementDisplayed(_sellerName, 3))
                {
                    var seller = Driver.FindElement(_sellerName);
                    return seller.Text.Trim();
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Checks if the product is in stock
        /// </summary>
        public bool IsInStock()
        {
            try
            {
                if (IsElementDisplayed(_outOfStockMessage, 2))
                    return false;
                
                return IsElementDisplayed(_addToCartButton, 3);
            }
            catch
            {
                return true;
            }
        }

        #endregion

        #region Variant Selection Methods

        /// <summary>
        /// Selects a size option by text
        /// </summary>
        public ProductPage SelectSize(string size)
        {
            try
            {
                By sizeLocator = By.XPath($"//*[contains(@class,'sku-variable') and contains(text(),'{size}')]");
                var sizeOption = WaitForElementClickable(sizeLocator, 5);
                sizeOption.Click();
            }
            catch
            {
            }
            return this;
        }

        public ProductPage SelectColor(string color)
        {
            try
            {
                By colorLocator = By.XPath($"//*[contains(@class,'sku-variable') and contains(@title,'{color}')]");
                var colorOption = WaitForElementClickable(colorLocator, 5);
                colorOption.Click();
            }
            catch
            {
            }
            return this;
        }

        public ProductPage SelectFirstAvailableVariant()
        {
            try
            {
                if (IsElementDisplayed(_sizeOptions, 3))
                {
                    var options = Driver.FindElements(_sizeOptions);
                    if (options.Count > 0)
                    {
                        options.First().Click();
                    }
                }
            }
            catch
            {
            }
            return this;
        }

        /// <summary>
        /// Checks if variant selection is required
        /// </summary>
        public bool IsVariantSelectionRequired()
        {
            try
            {
                return IsElementDisplayed(_sizeOptions, 3);
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Quantity Methods

        /// <summary>
        /// Sets the quantity
        /// </summary>
        public ProductPage SetQuantity(int quantity)
        {
            try
            {
                var input = WaitForElementVisible(_quantityInput, 5);
                input.Clear();
                input.SendKeys(quantity.ToString());
            }
            catch
            {
            }
            return this;
        }

        public ProductPage IncreaseQuantity()
        {
            try
            {
                var btn = WaitForElementClickable(_increaseQuantityBtn, 5);
                btn.Click();
            }
            catch
            {
            }
            return this;
        }

        public ProductPage DecreaseQuantity()
        {
            try
            {
                var btn = WaitForElementClickable(_decreaseQuantityBtn, 5);
                btn.Click();
            }
            catch
            {
            }
            return this;
        }

        /// <summary>
        /// Gets the current quantity value
        /// </summary>
        public int GetQuantity()
        {
            try
            {
                var input = WaitForElementVisible(_quantityInput, 5);
                string value = input.GetAttribute("value") ?? "1";
                return int.TryParse(value, out int qty) ? qty : 1;
            }
            catch
            {
                return 1;
            }
        }

        #endregion

        #region Cart Actions

        /// <summary>
        /// Checks if Add to Cart button is displayed on the page
        /// </summary>
        /// <returns>True if button is visible, false otherwise</returns>
        public bool IsAddToCartButtonDisplayed()
        {
            try
            {
                return IsElementDisplayed(_addToCartButton, 5);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Clicks the Add to Cart button
        /// </summary>
        public ProductPage ClickAddToCart()
        {
            try
            {
                if (IsVariantSelectionRequired())
                {
                    SelectFirstAvailableVariant();
                }
                
                var addToCartBtn = WaitForElementClickable(_addToCartButton, 10);
                ScrollToElement(_addToCartButton);
                addToCartBtn.Click();
            }
            catch
            {
                try
                {
                    var addToCartBtnAlt = WaitForElementClickable(By.XPath("//button[contains(text(),'Add to Cart') or contains(text(),'কার্টে যোগ করুন')]"), 5);
                    addToCartBtnAlt.Click();
                }
                catch
                {
                    ClickWithJavaScript(_addToCartButton);
                }
            }
            
            WaitForPageLoad();
            return this;
        }

        public ProductPage ClickBuyNow()
        {
            try
            {
                if (IsVariantSelectionRequired())
                {
                    SelectFirstAvailableVariant();
                }
                
                var buyNowBtn = WaitForElementClickable(_buyNowButton, 10);
                ScrollToElement(_buyNowButton);
                buyNowBtn.Click();
            }
            catch
            {
                try
                {
                    var buyNowBtnAlt = WaitForElementClickable(By.XPath("//button[contains(text(),'Buy Now') or contains(text(),'এখনই কিনুন')]"), 5);
                    buyNowBtnAlt.Click();
                }
                catch
                {
                    ClickWithJavaScript(_buyNowButton);
                }
            }
            
            WaitForPageLoad();
            return this;
        }

        public bool IsBuyNowButtonDisplayed()
        {
            try
            {
                return IsElementDisplayed(_buyNowButton, 5);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Verifies if Add to Cart was successful
        /// </summary>
        public bool IsAddToCartSuccessful()
        {
            try
            {
                if (IsElementDisplayed(_addToCartSuccess, 5))
                    return true;
                
                int cartCount = GetCartCount();
                return cartCount > 0;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the cart count from header
        /// </summary>
        public int GetCartCount()
        {
            try
            {
                var cartBadge = WaitForElementVisible(_cartCountBadge, 5);
                string countText = cartBadge.Text.Trim();
                return int.TryParse(countText, out int count) ? count : 0;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Clicks the View Cart button (if available after adding)
        /// </summary>
        public ProductPage ClickCartIcon()
        {
            try
            {
                var cartIcon = WaitForElementClickable(_cartIcon, 5);
                cartIcon.Click();
                WaitForPageLoad();
            }
            catch
            {
                ClickWithJavaScript(_cartIcon);
                WaitForPageLoad();
            }
            return this;
        }

        #endregion

        #region Error Handling

        /// <summary>
        /// Checks if login is required
        /// </summary>
        public bool IsLoginRequired()
        {
            try
            {
                return IsElementDisplayed(_loginRequiredPopup, 3);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if there's a variant selection error
        /// </summary>
        public bool HasVariantError()
        {
            try
            {
                return IsElementDisplayed(_variantError, 3);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Gets any error message displayed
        /// </summary>
        public string GetErrorMessage()
        {
            try
            {
                if (IsElementDisplayed(_errorMessage, 3))
                {
                    var error = Driver.FindElement(_errorMessage);
                    return error.Text.Trim();
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
