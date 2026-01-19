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
- [Future Improvements](#-future-improvements)

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
| **Daraz Account** | - | **Required: Registered account with home address configured** |

> ⚠️ **Important:** You must have a registered Daraz account with a complete home address set up for checkout tests to work properly.

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

## 🛠 Troubleshooting

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

User registration (signup) cannot be automated due to:

**1. OTP Requirement**
- Daraz requires SMS-based OTP verification for all registrations
- No programmatic way to retrieve OTP without physical device access

**2. Phone Number Restriction**
- Each phone number can only be registered once
- Cannot reuse numbers for repeated test runs

**3. Twilio/SMS Service Limitations**
- Third-party phone numbers (Twilio) also face one-time registration limit
- Cost-prohibitive to purchase new numbers for each test run
- Complex OTP retrieval infrastructure required (webhooks, parsing, timing)

**4. Security by Design**
- OTP is a security feature to prevent automated bot registrations
- Bypassing would violate terms of service

**Recommended Approach:**
- Use pre-registered test accounts (created manually)
- Store credentials securely in `appsettings.json`
- Focus on testing login and post-login flows (95% of user journeys)

---

## 🚀 Future Improvements

Planned enhancements to make the framework more robust:

### 1. Environment Variable Support
- Add support for environment variable overrides (e.g., `DARAZ_LOGIN_EMAIL`, `DARAZ_LOGIN_PASSWORD`)
- Enable hierarchical configuration: Environment Variables → appsettings.json → defaults
- Allow credentials to be injected from CI/CD pipelines without changing config files
- Support for GitHub Secrets, Azure Key Vault, AWS Secrets Manager

### 2. CI/CD Pipeline Integration
- **GitHub Actions** - Add `.github/workflows/test.yml` for automated test execution
- **Azure DevOps** - Create pipeline YAML for Azure Pipelines
- **Jenkins** - Add Jenkinsfile for continuous testing
- Store credentials securely in CI/CD secrets and pass via environment variables

### 3. Enhanced Reporting
- Video recording for failed tests
- Integration with test management tools (TestRail, Zephyr, qTest)
- Real-time dashboard for test execution monitoring

### 4. Parallel Execution
- Enable parallel test execution for faster runs
- Implement proper test isolation strategies
- Use thread-safe WebDriver instances

### 5. Cross-Browser Testing
- Cloud-based testing with BrowserStack or Sauce Labs
- Automated cross-browser compatibility matrix
- Mobile browser testing (iOS Safari, Android Chrome)

### 6. API Testing Integration
- Add API test layer for backend validation
- Combine UI + API tests for complete coverage
- Use API calls to set up test data faster
