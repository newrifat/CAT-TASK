using NUnit.Framework;

namespace DarazAutomation.Data
{
    /// <summary>
    /// Test data provider for category navigation and product addition tests.
    /// </summary>
    public class CategoryNavigationTestData
    {
        /// <summary>
        /// Product selection criteria for identifying products in category listings.
        /// </summary>
        public class ProductSelector
        {
            public int? Index { get; set; }
            public string? NameContains { get; set; }
            public string? Keyword { get; set; }

            public ProductSelector(int index)
            {
                Index = index;
            }

            public ProductSelector(string nameOrKeyword, bool isExactName = false)
            {
                if (isExactName)
                    NameContains = nameOrKeyword;
                else
                    Keyword = nameOrKeyword;
            }

            public static ProductSelector FirstProduct() => new ProductSelector(0);
            public static ProductSelector ByName(string name) => new ProductSelector(name, isExactName: true);
            public static ProductSelector ByKeyword(string keyword) => new ProductSelector(keyword, isExactName: false);
        }

        /// <summary>
        /// Test data for adding products from different categories.
        /// </summary>
        public static IEnumerable<TestCaseData> AddProductToCartTestCases
        {
            get
            {
                yield return new TestCaseData(
                    "Women's & Girls' Fashion",
                    "Bags",
                    "Wallets",
                    "wallets",
                    ProductSelector.ByKeyword("High-quality Black PU Leather Fashionable Wallet"),
                    false
                )
                .SetName("AddProduct_WomensWallets_FromListing")
                .SetDescription("Add wallet from Women's Wallets category");

                yield return new TestCaseData(
                    "Men's & Boys' Fashion",
                    "Shoes",
                    "Shoes Accessories",
                    "shoes",
                    ProductSelector.ByKeyword("Foot Care Protector High Heel Shoe Insole Cushion"),
                    true
                )
                .SetName("AddProduct_MensShoesAccessories_ViaProductPage")
                .SetDescription("Add shoe accessory from Men's Shoes Accessories via product page");
            }
        }

        /// <summary>
        /// Test data for category navigation.
        /// </summary>
        public static IEnumerable<TestCaseData> CategoryNavigationTestCases
        {
            get
            {
                yield return new TestCaseData(
                    "Women's & Girls' Fashion",
                    "Bags",
                    "Wallets",
                    "wallets"
                )
                .SetName("Navigate_WomensWallets")
                .SetDescription("Navigate to Women's Wallets category");

                yield return new TestCaseData(
                    "Men's & Boys' Fashion",
                    "Shoes",
                    "Shoes Accessories",
                    "shoes"
                )
                .SetName("Navigate_MensShoesAccessories")
                .SetDescription("Navigate to Men's Shoes Accessories category");
            }
        }

        /// <summary>
        /// Returns a specific test case by index for sequential tests.
        /// </summary>
        public static (string Level1, string Level2, string Level3, string UrlKeyword, ProductSelector Selector, bool UseProductPage) GetTestCase(int index)
        {
            var testCases = new[]
            {
                (
                    Level1: "Women's & Girls' Fashion",
                    Level2: "Bags",
                    Level3: "Wallets",
                    UrlKeyword: "wallets",
                    Selector: ProductSelector.ByKeyword("High-quality Black PU Leather Fashionable Wallet for Men"),
                    UseProductPage: false
                ),
                (
                    Level1: "Men's & Boys' Fashion",
                    Level2: "Shoes",
                    Level3: "Shoes Accessories",
                    UrlKeyword: "shoes",
                    Selector: ProductSelector.ByKeyword("Foot Care Protector High Heel Shoe Insole Cushion"),
                    UseProductPage: true
                )
            };

            if (index >= 0 && index < testCases.Length)
            {
                return testCases[index];
            }

            throw new ArgumentOutOfRangeException(nameof(index), $"Test case index {index} is out of range. Valid range: 0-{testCases.Length - 1}");
        }
    }
}
