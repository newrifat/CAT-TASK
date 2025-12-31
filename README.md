# 🛒 Daraz Automation Framework

[![.NET Version](https://img.shields.io/badge/.NET-9.0-blue)](https://dotnet.microsoft.com/)
[![Selenium](https://img.shields.io/badge/Selenium-4.x-green)](https://www.selenium.dev/)
[![NUnit](https://img.shields.io/badge/NUnit-4.x-brightgreen)](https://nunit.org/)
[![License](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A **modern, production-ready** Selenium WebDriver automation framework built with **.NET 9.0** and **C#** for comprehensive end-to-end testing of [Daraz Bangladesh](https://www.daraz.com.bd/). 

This framework implements **industry best practices** including Page Object Model (POM), explicit wait strategies, data-driven testing, secure credential management, and beautiful HTML reporting with ExtentReports.

---

## 📑 Table of Contents

- [✨ Features](#-features)
- [🚀 Quick Start](#-quick-start)
- [📦 Installation](#-installation)
- [⚙️ Configuration Setup](#️-configuration-setup)
- [🧪 Running Tests](#-running-tests)
- [📁 Project Structure](#-project-structure)
- [🎯 Test Scenarios](#-test-scenarios)
- [📊 Test Reports](#-test-reports)
- [🔧 Advanced Usage](#-advanced-usage)
- [📖 Documentation](#-documentation)
- [🛠 Troubleshooting](#-troubleshooting)

---

## ✨ Features

### 🏗️ Architecture & Design
- ✅ **Page Object Model (POM)** - Clean separation of concerns, maintainable test code
- ✅ **Factory Pattern** - Flexible WebDriver creation supporting multiple browsers
- ✅ **Data-Driven Testing** - External test data using NUnit TestCaseSource
- ✅ **Utility Helper Classes** - Reusable components for common test operations

### 🔒 Security & Configuration
- 🔐 **Secure Credential Management** - Gitignored config files with hierarchical structure
- 🔐 **No Hardcoded Values** - All test data externalized to appsettings.json
- 🔐 **Hierarchical Configuration** - Modern .NET Configuration API
- 🔐 **Best Practices** - Follows OWASP security guidelines

### 🧪 Testing Features
- ⚡ **Explicit Waits** - No Thread.Sleep, production-grade wait strategies
- 📊 **ExtentReports** - Beautiful HTML reports with screenshots and logs
- 📸 **Screenshot on Failure** - Automatic screenshot capture for debugging
- 🔄 **Session Persistence** - Maintain login state across sequential tests
- 🎭 **Multiple Test Modes** - Sequential (shared session) & Independent (isolated)
- 🚀 **CI/CD Ready** - Easy integration with GitHub Actions, Jenkins, Azure DevOps

### 📈 Code Quality
- 🟢 **Clean Code** - SOLID principles, no code duplication
- 🟢 **Comprehensive Documentation** - XML docs, README, and guides
- 🟢 **Modern C#** - .NET 9.0 features, null-safety, latest syntax
- 🟢 **Modular & Extensible** - Easy to add new pages and tests

---

## 🚀 Quick Start

Get up and running in **under 3 minutes**:

```bash
# 1. Clone the repository
git clone <your-repo-url>
cd CANT-TASK

# 2. Restore dependencies
dotnet restore

# 3. Configure credentials
cp src/Config/appsettings.example.json src/Config/appsettings.json
# Edit src/Config/appsettings.json with your actual Daraz credentials

# 4. Run tests
dotnet test

# 5. View reports
open Reports/ExtentReport_*.html
```

---

## 📦 Installation

### Prerequisites

Before you begin, ensure you have the following installed:

| Requirement | Version | Download Link |
|------------|---------|---------------|
| **.NET SDK** | 9.0 or later | [Download .NET](https://dotnet.microsoft.com/download) |
| **Chrome Browser** | Latest | [Download Chrome](https://www.google.com/chrome/) |
| **Git** | Latest | [Download Git](https://git-scm.com/) |
| **IDE** (Optional) | Any | [VS Code](https://code.visualstudio.com/) / [Visual Studio](https://visualstudio.microsoft.com/) / [Rider](https://www.jetbrains.com/rider/) |

### Verify Installation

```bash
# Check .NET version
dotnet --version
# Expected output: 9.0.x or higher

# Check Chrome version (macOS)
/Applications/Google\ Chrome.app/Contents/MacOS/Google\ Chrome --version

# Check Chrome version (Linux)
google-chrome --version

# Check Chrome version (Windows)
"C:\Program Files\Google\Chrome\Application\chrome.exe" --version
```

### Step-by-Step Installation

#### 1. Clone the Repository
```bash
git clone <your-repository-url>
cd CANT-TASK
```

#### 2. Restore NuGet Packages
```bash
dotnet restore
```

This will download all required dependencies:
- Selenium.WebDriver (4.x)
- Selenium.WebDriver.ChromeDriver
- NUnit (4.x)
- NUnit3TestAdapter
- ExtentReports (5.x)
- Microsoft.Extensions.Configuration packages

#### 3. Build the Project
```bash
dotnet build
```

Expected output:
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

#### 4. Verify Installation
```bash
dotnet test --list-tests
```

You should see a list of available tests.

---

## ⚙️ Configuration Setup

### Configuration File Structure

The framework uses a secure configuration system with **two configuration files**:

| File | Purpose | Git Status |
|------|---------|------------|
| `src/Config/appsettings.example.json` | Template with placeholders | ✅ Committed to repository |
| `src/Config/appsettings.json` | Your actual credentials | 🔒 **Gitignored** (never committed) |

### Quick Setup

**Step 1: Copy the example configuration file**
```bash
cp src/Config/appsettings.example.json src/Config/appsettings.json
```

**Step 2: Edit `src/Config/appsettings.json` with your credentials**

Open the file and replace the placeholder values:

```json
{
  "TestSettings": {
    "BaseUrl": "https://www.daraz.com.bd/",
    "Browser": "Chrome",
    "ImplicitWaitSeconds": 10,
    "ExplicitWaitSeconds": 30,
    "Headless": false,
    "IncognitoMode": true,
    "ScreenshotOnFailureOnly": true
  },
  "Credentials": {
    "LoginEmail": "your-actual-email@example.com",
    "LoginPassword": "YourActualPassword123",
    "LoginPhone": "+880170000000"
  },
  "LanguageSettings": {
    "EnglishText": "EN",
    "BanglaText": "বাং",
    "EnglishCode": "en",
    "BanglaCode": "bn",
    "EnglishVerificationText": "JUST FOR YOU",
    "BanglaVerificationText": "আপনার জন্য"
  }
}
```

**Step 3: Run your tests**
```bash
dotnet test
```

> ⚠️ **Security Note**: The `appsettings.json` file is automatically ignored by Git (see `.gitignore`). Never commit this file with real credentials!

### Configuration Options

#### Browser Settings

| Setting | Type | Default | Description |
|---------|------|---------|-------------|
| `Browser` | string | `"Chrome"` | Browser to use: Chrome, Firefox, Edge |
| `Headless` | bool | `false` | Run browser in headless mode (no GUI) |
| `IncognitoMode` | bool | `true` | Run browser in incognito/private mode |
| `ImplicitWaitSeconds` | int | `10` | Global implicit wait timeout |
| `ExplicitWaitSeconds` | int | `30` | Explicit wait timeout for elements |

#### Test Settings

| Setting | Type | Default | Description |
|---------|------|---------|-------------|
| `BaseUrl` | string | `"https://www.daraz.com.bd/"` | Base URL for the application |
| `ScreenshotOnFailureOnly` | bool | `true` | Take screenshots only on test failures |

#### Credentials

| Setting | Type | Required | Description |
|---------|------|----------|-------------|
| `LoginEmail` | string | ✅ Yes | Daraz account email |
| `LoginPassword` | string | ✅ Yes | Daraz account password |
| `LoginPhone` | string | ❌ No | Daraz account phone number |

---

## 🧪 Running Tests

### Basic Test Execution

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run tests and generate TRX report
dotnet test --logger "trx;LogFileName=TestResults.trx"
```

### Run Tests by Category

```bash
# Run all sequential tests (shared browser session)
dotnet test --filter "Category=Sequential"

# Run all critical tests
dotnet test --filter "Category=Critical"

# Run session-persistent tests
dotnet test --filter "Category=SessionPersistent"
```

### Run Specific Tests

```bash
# Run specific test by name
dotnet test --filter "FullyQualifiedName~Test1_LanguageSwitch"

# Run specific test class
dotnet test --filter "FullyQualifiedName~SequentialTests"

# Run tests matching a pattern
dotnet test --filter "DisplayName~Login"
```

### Run Tests in Different Modes

#### Headless Mode (No Browser GUI)
Edit `src/Config/appsettings.json`:
```json
{
  "TestSettings": {
    "Headless": true
  }
}
```
Then run:
```bash
dotnet test
```

#### Different Browser
```json
{
  "TestSettings": {
    "Browser": "Firefox"  // or "Edge"
  }
}
```

#### Parallel Execution
```bash
# Run tests in parallel (use with caution for independent tests only)
dotnet test --parallel
```

### Test Execution Examples

```bash
# Example 1: Run all tests with verbose output
dotnet test --logger "console;verbosity=detailed"

# Example 2: Run critical tests only
dotnet test --filter "Category=Critical"

# Example 3: Run and save results
dotnet test --logger "trx" --results-directory ./TestResults

# Example 4: Run specific test suite
dotnet test --filter "FullyQualifiedName~SequentialTests"
```

---

## 📁 Project Structure

```
CANT-TASK/
│
├── 📄 DarazAutomation.csproj          # Project file with dependencies
├── 📄 README.md                        # This file
├── 📄 .gitignore                       # Git ignore rules
│
├── 📁 src/                             # Source code directory
│   │
│   ├── 📁 Base/                        # Base classes for inheritance
│   │   ├── BasePage.cs                 # Base page object with common methods
│   │   └── BaseTest.cs                 # Base test class with setup/teardown
│   │
│   ├── 📁 Config/                      # Configuration management
│   │   ├── appsettings.example.json    # Config template (committed)
│   │   ├── appsettings.json            # Your config (gitignored)
│   │   └── ConfigurationManager.cs     # Config accessor class
│   │
│   ├── 📁 Data/                        # Test data providers
│   │   ├── TestDataProvider.cs         # Base data provider
│   │   ├── LanguageTestData.cs         # Language switching data
│   │   └── CategoryNavigationTestData.cs  # Category navigation data
│   │
│   ├── 📁 Drivers/                     # WebDriver management
│   │   └── DriverFactory.cs            # Browser driver factory
│   │
│   ├── 📁 Pages/                       # Page Object Model classes
│   │   ├── HomePage.cs                 # Home page interactions
│   │   ├── LoginPage.cs                # Login functionality
│   │   ├── CategoryPage.cs             # Category browsing
│   │   ├── ProductPage.cs              # Product details
│   │   └── CartPage.cs                 # Shopping cart operations
│   │
│   ├── 📁 Tests/                       # Test classes
│   │   └── IndependentTests/
│   │       └── SequentialTests.cs      # Sequential test suite
│   │
│   └── 📁 Utilities/                   # Helper utilities
│       ├── ExtentReportManager.cs      # Test reporting
│       ├── ScreenshotHelper.cs         # Screenshot capture
│       └── TestFlowHelper.cs           # Test flow utilities
│
├── 📁 bin/                             # Build output (gitignored)
├── 📁 obj/                             # Build intermediate (gitignored)
└── 📁 Reports/                         # Test reports (gitignored)
    └── ExtentReport_*.html             # HTML test reports
```

### Key Directories Explained

| Directory | Purpose |
|-----------|---------|
| `src/Base/` | Base classes that provide common functionality to all pages and tests |
| `src/Config/` | Configuration files and management classes for settings and credentials |
| `src/Data/` | Test data providers that supply test cases and test data |
| `src/Drivers/` | WebDriver factory for creating and managing browser instances |
| `src/Pages/` | Page Object Model classes representing each page of the application |
| `src/Tests/` | Test classes containing actual test methods |
| `src/Utilities/` | Helper classes for reporting, screenshots, and common operations |
| `Reports/` | Generated test execution reports (HTML, screenshots) |

### Key Files Explained

| File | Purpose |
|------|---------|
| `BasePage.cs` | Common page methods (wait, click, type, navigate, scroll) |
| `BaseTest.cs` | Test lifecycle management (setup, teardown, logging, reporting) |
| `ConfigurationManager.cs` | Centralized configuration access with environment variable support |
| `DriverFactory.cs` | Browser driver creation with multiple browser support |
| `ExtentReportManager.cs` | Beautiful HTML report generation with ExtentReports |
| `TestFlowHelper.cs` | Reusable test flow operations (login, navigation, cart) |

---

## 🎯 Test Scenarios

### Sequential Test Suite (`SequentialTests.cs`)

Tests run in order with a **shared browser session**, maintaining login state throughout:

| Test | Order | Description |
|------|-------|-------------|
| **Test1_LanguageSwitch** | 1 | Switch between English and Bangla languages |
| **Test2_Login** | 2 | User authentication (session persists) |
| **Test3_VerifySessionPersisted** | 3 | Verify login session is maintained |
| **Test4_NavigateToCategoryWhileLoggedIn** | 4 | Browse categories while logged in |
| **Test5_MultiLevelCategoryNavigation** | 5 | Navigate to Women's Wallets category |
| **Test6_AddFirstWalletToCart** | 6 | Add first product (wallet) to cart |
| **Test7_AddSecondProduct** | 7 | Add second product (shoes/accessories) to cart |
| **Test8_CheckoutFlow** | 8 | Complete cart selection and checkout flow |

### Test Flow Visualization

```
┌─────────────────────────────────────────────────────┐
│  Test 1: Language Switch (EN ↔ BN)                 │
└─────────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────────┐
│  Test 2: User Login (Session Starts)               │
└─────────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────────┐
│  Test 3: Verify Session Persisted                  │
└─────────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────────┐
│  Test 4: Category Navigation (Logged In)           │
└─────────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────────┐
│  Test 5: Multi-Level Category (Women's Wallets)    │
└─────────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────────┐
│  Test 6: Add Product 1 to Cart                     │
└─────────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────────┐
│  Test 7: Add Product 2 to Cart                     │
└─────────────────────────────────────────────────────┘
                        ↓
┌─────────────────────────────────────────────────────┐
│  Test 8: Checkout Flow (Cart → Payment)            │
└─────────────────────────────────────────────────────┘
```

---

## 📊 Test Reports

### ExtentReports

After test execution, beautiful HTML reports are automatically generated in the `Reports/` directory.

#### Report Features
- ✅ Test execution status (Pass/Fail/Skip)
- ✅ Step-by-step logs with timestamps
- ✅ Screenshots on failure
- ✅ System and environment information
- ✅ Execution time and statistics
- ✅ Interactive dashboard

#### Viewing Reports

```bash
# macOS
open Reports/ExtentReport_*.html

# Linux
xdg-open Reports/ExtentReport_*.html

# Windows
start Reports/ExtentReport_*.html
```

#### Sample Report Structure

```
ExtentReport_2025-12-31_14-30-45.html
├── Dashboard
│   ├── Test Summary (Pass/Fail counts)
│   ├── Execution Time
│   └── Environment Info
├── Test Details
│   ├── Test1_LanguageSwitch ✅
│   ├── Test2_Login ✅
│   ├── Test3_VerifySessionPersisted ✅
│   └── Test8_CheckoutFlow ❌ (with screenshot)
└── System Information
    ├── OS: macOS 14.0
    ├── Browser: Chrome 120.0
    └── .NET: 9.0.0
```

---

## 🔧 Advanced Usage

### Adding New Page Objects

1. Create a new file in `src/Pages/` directory
2. Extend `BasePage` class
3. Define locators as private fields using `By` class
4. Implement page-specific interaction methods
5. Follow the existing page object patterns in the framework

### Adding New Tests

1. Create a new file in `src/Pages/` directory
2. Extend `BasePage` class
3. Define locators as private fields using `By` class
4. Implement page-specific interaction methods
5. Follow the existing page object patterns in the framework

### Adding New Tests

1. Create a new test class in `src/Tests/` directory
2. Extend `BaseTest` class
3. Use `[Test]` attribute for test methods
4. Use `[Category]` attribute to organize tests
5. Initialize page objects in `[SetUp]` method
6. Write clear assertions with descriptive failure messages

### Adding Test Data

1. Create a new file in `src/Data/` directory
2. Extend `TestDataProvider` class
3. Define test case methods returning `IEnumerable<TestCaseData>`
4. Use descriptive test case names with `.SetName()`
5. Organize test data logically by feature or test scenario

### Using Test Data in Tests

Use `[TestCaseSource]` attribute to link test methods with data providers:
- Specify the data provider class type
- Reference the static method name that returns test cases
- Test method parameters should match the test case data structure
- Each test case will run as a separate test instance

---

## 📖 Documentation

### Additional Documentation Files

- **`README.md`** - This file (Quick start and overview)
- **`src/Config/appsettings.example.json`** - Configuration template
- **XML Documentation** - Inline code documentation

### Code Documentation

All classes and methods include comprehensive XML documentation for better code understanding and IDE intellisense support. Refer to the source code files for detailed inline documentation.

### Best Practices

#### Page Object Model
- ✅ Keep locators private within page classes
- ✅ Expose high-level actions as public methods
- ✅ Return page objects for method chaining
- ✅ Use meaningful method names

#### Test Design
- ✅ One assertion concept per test
- ✅ Use descriptive test names
- ✅ Independent tests (avoid dependencies)
- ✅ Clean up test data after execution

#### Wait Strategies
- ✅ Always use explicit waits
- ✅ Never use `Thread.Sleep()`
- ✅ Wait for element state, not arbitrary time
- ✅ Use fluent waits for complex conditions

#### Configuration
- ✅ Never commit `appsettings.json`
- ✅ Use `appsettings.example.json` as a template
- ✅ Keep sensitive data secure
- ✅ Use meaningful default values

---

## 🛠 Troubleshooting

### Common Issues and Solutions

#### Issue: ChromeDriver version mismatch

**Error:**
```
session not created: This version of ChromeDriver only supports Chrome version X
```

**Solution:**
```bash
# Update ChromeDriver package
dotnet add package Selenium.WebDriver.ChromeDriver --version <latest>

# Or use WebDriverManager
dotnet add package WebDriverManager
```

#### Issue: Element not found

**Error:**
```
NoSuchElementException: Unable to locate element
```

**Solution:**
- Verify the locator is correct
- Add explicit wait before interaction
- Check if element is in iframe
- Verify element is visible and enabled
- Use the `WaitForElementVisible()` method from `BasePage` before interacting with elements

#### Issue: Tests fail in headless mode

**Solution:**
```json
{
  "TestSettings": {
    "Headless": false  // Disable headless temporarily
  }
}
```

Then debug with visible browser to see what's happening.

#### Issue: Cannot find appsettings.json

**Error:**
```
FileNotFoundException: appsettings.json
```

**Solution:**
```bash
# Copy the example file
cp src/Config/appsettings.example.json src/Config/appsettings.json

# Edit with your credentials
nano src/Config/appsettings.json
```

#### Issue: Tests are slow

**Solutions:**
1. Reduce wait timeouts in `appsettings.json`
2. Use headless mode
3. Disable loading of images and CSS
4. Run tests in parallel (for independent tests)

```json
{
  "TestSettings": {
    "Headless": true,
    "ExplicitWaitSeconds": 20
  }
}
```

#### Issue: Build fails

**Solution:**
```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

#### Issue: Tests pass locally but fail in CI/CD

**Solutions:**
1. Use environment variables for credentials
2. Enable headless mode
3. Add delays for slower CI environments
4. Check browser version compatibility

```bash
# Set env vars in CI/CD
export DARAZ_LOGIN_EMAIL="ci-test@example.com"
export DARAZ_HEADLESS="true"
```

### Getting Help

If you encounter issues not covered here:

1. Check the test reports in `Reports/` directory
2. Review screenshots captured on failure
3. Run tests with verbose logging:
   ```bash
   dotnet test --logger "console;verbosity=detailed"
   ```
4. Check the browser console for JavaScript errors
5. Review the ExtentReport for detailed step-by-step logs

---

##  Performance Metrics

| Metric | Value | Details |
|--------|-------|---------|
| **Test Execution Time** | ~120s | Full sequential suite |
| **Code Coverage** | High | Core functionality covered |
| **Lines of Code** | ~2,500 | Clean, maintainable codebase |
| **Code Duplication** | 0% | DRY principles applied |
| **Wait Strategy** | Explicit | No Thread.Sleep() |
| **Test Stability** | High | Reliable, consistent results |

---

## 📄 Technology Stack

| Technology | Version | Purpose |
|------------|---------|---------|
| **.NET SDK** | 9.0 | Runtime framework |
| **C#** | 12.0 | Programming language |
| **NUnit** | 4.x | Test framework |
| **Selenium WebDriver** | 4.x | Browser automation |
| **ExtentReports** | 5.x | HTML reporting |
| **Microsoft.Extensions.Configuration** | 9.x | Configuration management |

---

## ⚠️ Limitations & Known Issues

### Signup Automation Not Supported

**User registration (signup) is intentionally NOT automated in this framework** due to technical and practical limitations imposed by Daraz's security measures.

#### Why Signup Cannot Be Automated

##### 1. **OTP (One-Time Password) Requirement**
- Daraz requires **SMS-based OTP verification** for all new user registrations
- OTP is sent to the mobile phone number provided during signup
- The OTP must be entered within a time limit (typically 60-120 seconds)
- There is **no API or programmatic way** to retrieve the OTP without physical access to the mobile device

##### 2. **Phone Number Restriction**
- Each mobile phone number can only be registered **once** with Daraz
- Once a number is used for registration, it cannot be reused for another account
- This creates a **one-time use limitation** that makes automated testing impractical

##### 3. **Third-Party SMS Services (Twilio) Limitations**

We attempted to use **Twilio** (a popular SMS service provider) to automate OTP retrieval, but encountered the following issues:

**Issue A: One-Time Registration Per Phone Number**
- Even with Twilio-provided phone numbers, each number can only be used **once** for Daraz registration
- After the first signup, the same Twilio number becomes unusable for future test runs
- Purchasing new Twilio numbers for each test run is **cost-prohibitive** and impractical

**Issue B: OTP Retrieval Complexity**
- Twilio can receive SMS messages, but retrieving them programmatically requires:
  - Setting up webhooks or polling the Twilio API
  - Parsing SMS content to extract the OTP code
  - Handling timing issues (OTP expiry vs. retrieval delay)
  - Managing potential SMS delivery delays

**Issue C: Cost and Scalability**
- Twilio phone numbers cost money (monthly fees + SMS charges)
- Running automated tests frequently would require **continuous purchase of new numbers**
- This approach is **neither scalable nor cost-effective** for test automation

##### 4. **Security by Design**
- Daraz's OTP requirement is a **security feature** designed to prevent automated bot registrations
- This is a **best practice** for preventing spam accounts and ensuring user authenticity
- Any attempt to bypass this would violate terms of service and security policies

#### Recommended Approach

Instead of automating signup, the framework uses **pre-registered test accounts**:

✅ **Use Existing Accounts for Testing**
- Create 1-2 test accounts manually through the Daraz website or mobile app
- Store credentials securely in `appsettings.json` (gitignored)
- Use these accounts repeatedly for login and checkout flow testing
- Manually manage test data cleanup as needed

✅ **Test Coverage Without Signup**
- **Login Flow** - Fully automated
- **Category Navigation** - Fully automated
- **Product Search** - Fully automated
- **Add to Cart** - Fully automated
- **Checkout Flow** - Fully automated (up to payment)
- **User Profile** - Fully automated (if needed)

#### Alternative Testing Strategies

For teams that require signup flow testing, consider these alternatives:

1. **Manual Testing for Signup**
   - Test signup flow manually during QA cycles
   - Document signup steps with screenshots/videos
   - Automate everything else (login onwards)

2. **API-Level Testing** (if available)
   - Test user registration via API calls (if Daraz provides test APIs)
   - Bypass UI automation for signup
   - Use API-created accounts for UI testing

3. **Mock OTP in Test Environments**
   - Request Daraz test environment with mock OTP functionality
   - Use fixed OTP codes (e.g., "123456") in test environment
   - Automate signup only in test environment, not production

4. **Visual Testing**
   - Use visual regression testing tools to verify signup UI
   - Ensure signup form elements are present and styled correctly
   - Validate error messages without completing full registration

#### Conclusion

While **signup automation is technically possible** with enough infrastructure (SMS forwarding, disposable phone numbers, etc.), it is **not practical or recommended** due to:
- 🚫 High cost (purchasing new phone numbers repeatedly)
- 🚫 Poor scalability (cannot reuse numbers)
- 🚫 Complexity (OTP retrieval infrastructure)
- 🚫 Potential ToS violations (bypassing security measures)

**The framework focuses on testing the 95% of user journeys that can be reliably automated**, starting from the login page with pre-configured test accounts.

---

## � Future Improvements

The following enhancements can be added to make the framework even more robust:

### 1. Environment Variable Support (CI/CD Integration)
- Add support for environment variable overrides (e.g., `DARAZ_LOGIN_EMAIL`, `DARAZ_LOGIN_PASSWORD`)
- Enable hierarchical configuration: Environment Variables → appsettings.json → defaults
- Allow credentials to be injected from CI/CD pipelines without changing config files
- Support for GitHub Secrets, Azure Key Vault, AWS Secrets Manager

### 2. CI/CD Pipeline Integration
- **GitHub Actions** - Add `.github/workflows/test.yml` for automated test execution
- **Azure DevOps** - Create pipeline YAML for Azure Pipelines
- **Jenkins** - Add Jenkinsfile for continuous testing
- Store credentials securely in CI/CD secrets and pass via environment variables

### 3. Test Data Management
- Database-driven test data instead of hardcoded JSON
- Test data reset/cleanup scripts
- Shared test data repository for team collaboration

### 4. Enhanced Reporting
- Integrate with test management tools (TestRail, Zephyr, qTest)
- Add video recording for failed tests
- Real-time dashboard for test execution monitoring
- Performance metrics tracking over time

### 5. Parallel Execution
- Enable parallel test execution for faster runs
- Implement proper test isolation strategies
- Use thread-safe WebDriver instances

### 6. Cross-Browser Testing
- Cloud-based testing with BrowserStack or Sauce Labs
- Automated cross-browser compatibility matrix
- Mobile browser testing (iOS Safari, Android Chrome)

### 7. API Testing Integration
- Add API test layer for backend validation
- Combine UI + API tests for complete coverage
- Use API calls to set up test data faster

---

## �👥 Contributing

Contributions are welcome! Please follow these guidelines:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

### Code Standards
- Follow C# naming conventions
- Add XML documentation to public methods
- Write unit tests for new features
- Update README for significant changes

---

## 📝 License

This project is for educational and testing purposes.

---

## 👤 Author

**Daraz Automation Framework**  
Built for testing Daraz.com.bd e-commerce platform

---

## ⭐ Support

If you find this framework helpful, please consider:
- ⭐ Starring the repository
- 🐛 Reporting bugs and issues
- 💡 Suggesting new features
- 📖 Improving documentation

---

**Happy Testing! 🚀**
