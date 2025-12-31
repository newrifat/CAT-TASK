# 🛒 Daraz Automation Framework

[![.NET](https://img.shields.io/badge/.NET-9.0-blue)](https://dotnet.microsoft.com/)
[![Selenium](https://img.shields.io/badge/Selenium-4.x-green)](https://www.selenium.dev/)
[![NUnit](https://img.shields.io/badge/NUnit-4.x-brightgreen)](https://nunit.org/)

A modern Selenium WebDriver automation framework for end-to-end testing of [Daraz Bangladesh](https://www.daraz.com.bd/), built with .NET 9.0, C#, and NUnit. Implements Page Object Model, data-driven testing, and ExtentReports.

---

## 📑 Table of Contents

- [Features](#-features)
- [Quick Start](#-quick-start)
- [Installation](#-installation)
- [Configuration](#-configuration)
- [Running Tests](#-running-tests)
- [Project Structure](#-project-structure)
- [Test Reports](#-test-reports)
- [Troubleshooting](#-troubleshooting)
- [Limitations](#-limitations)

---

## ✨ Features

- ✅ **Page Object Model (POM)** - Clean, maintainable architecture
- ✅ **Data-Driven Testing** - External test data with NUnit TestCaseSource
- ✅ **Secure Credentials** - Gitignored configuration files
- ✅ **Explicit Waits** - Production-grade wait strategies
- ✅ **ExtentReports** - Beautiful HTML reports with screenshots
- ✅ **Session Persistence** - Maintain login state across tests
- ✅ **CI/CD Ready** - Easy integration with pipelines

---

## 🚀 Quick Start

```bash
# 1. Clone and navigate
git clone <your-repo-url>
cd CANT-TASK

# 2. Restore dependencies
dotnet restore

# 3. Configure credentials
cp src/Config/appsettings.example.json src/Config/appsettings.json
# Edit src/Config/appsettings.json with your Daraz credentials

# 4. Run tests
dotnet test

# 5. View reports
open Reports/ExtentReport_*.html
```

---

## 📦 Installation

### Prerequisites

| Requirement | Version | Download |
|------------|---------|----------|
| .NET SDK | 9.0+ | [Download](https://dotnet.microsoft.com/download) |
| Chrome | Latest | [Download](https://www.google.com/chrome/) |
| Git | Latest | [Download](https://git-scm.com/) |

### Verify Installation

```bash
dotnet --version  # Should show 9.0.x or higher
```

---

## ⚙️ Configuration

### Setup Steps

**1. Copy configuration template:**
```bash
cp src/Config/appsettings.example.json src/Config/appsettings.json
```

**2. Edit `src/Config/appsettings.json` with your credentials:**

```json
{
  "TestSettings": {
    "BaseUrl": "https://www.daraz.com.bd/",
    "Browser": "Chrome",
    "Headless": false
  },
  "Credentials": {
    "LoginEmail": "your-email@example.com",
    "LoginPassword": "YourPassword123"
  }
}
```

> ⚠️ **Security:** `appsettings.json` is gitignored. Never commit credentials!

### Key Configuration Options

| Setting | Default | Description |
|---------|---------|-------------|
| `Browser` | Chrome | Browser: Chrome, Firefox, Edge |
| `Headless` | false | Run without GUI |
| `BaseUrl` | https://www.daraz.com.bd/ | Application URL |
| `LoginEmail` | - | Your Daraz email (required) |
| `LoginPassword` | - | Your Daraz password (required) |

---

## 🧪 Running Tests

### Basic Commands

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run by category
dotnet test --filter "Category=Critical"

# Run specific test
dotnet test --filter "FullyQualifiedName~Test1_LanguageSwitch"
```

### Test Modes

**Headless Mode (No Browser GUI):**
Set `"Headless": true` in `appsettings.json`

**Different Browser:**
Set `"Browser": "Firefox"` or `"Edge"` in `appsettings.json`

---

## 📁 Project Structure

```
CANT-TASK/
├── src/
│   ├── Base/                    # Base classes
│   │   ├── BasePage.cs          # Common page methods
│   │   └── BaseTest.cs          # Test lifecycle management
│   ├── Config/                  # Configuration
│   │   ├── appsettings.example.json
│   │   ├── appsettings.json     (gitignored)
│   │   └── ConfigurationManager.cs
│   ├── Data/                    # Test data providers
│   │   ├── CategoryNavigationTestData.cs
│   │   └── LanguageTestData.cs
│   ├── Drivers/                 # WebDriver factory
│   │   └── DriverFactory.cs
│   ├── Pages/                   # Page Object Model
│   │   ├── HomePage.cs
│   │   ├── LoginPage.cs
│   │   ├── CategoryPage.cs
│   │   ├── ProductPage.cs
│   │   └── CartPage.cs
│   ├── Tests/                   # Test classes
│   │   └── IndependentTests/
│   │       └── SequentialTests.cs
│   └── Utilities/               # Helper classes
│       ├── ExtentReportManager.cs
│       ├── ScreenshotHelper.cs
│       └── TestFlowHelper.cs
└── Reports/                     # Test reports (gitignored)
```

### Test Scenarios

The framework includes 8 sequential tests that maintain login state:

1. **Language Switch** - Toggle between English/Bangla
2. **Login** - User authentication
3. **Verify Session** - Confirm login persists
4. **Category Navigation** - Browse categories while logged in
5. **Multi-Level Navigation** - Navigate to Women's Wallets
6. **Add Product 1** - Add wallet to cart
7. **Add Product 2** - Add shoes/accessories to cart
8. **Checkout Flow** - Complete cart and checkout

---

## 📊 Test Reports

HTML reports are automatically generated in `Reports/` directory using ExtentReports.

**View Reports:**
```bash
# macOS
open Reports/ExtentReport_*.html

# Linux
xdg-open Reports/ExtentReport_*.html

# Windows
start Reports/ExtentReport_*.html
```

**Report Features:**
- ✅ Test execution status (Pass/Fail/Skip)
- ✅ Step-by-step logs with timestamps
- ✅ Screenshots on failure
- ✅ System information
- ✅ Execution statistics

---

## � Troubleshooting

### Common Issues

**ChromeDriver version mismatch:**
```bash
dotnet add package Selenium.WebDriver.ChromeDriver --version <latest>
```

**Element not found:**
- Verify locators are correct
- Add explicit waits
- Check if element is in an iframe

**Tests fail in headless mode:**
Set `"Headless": false` temporarily to debug

**Missing appsettings.json:**
```bash
cp src/Config/appsettings.example.json src/Config/appsettings.json
```

**Slow tests:**
- Reduce wait timeouts in config
- Use headless mode
- Check network connectivity

---

## ⚠️ Limitations

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
