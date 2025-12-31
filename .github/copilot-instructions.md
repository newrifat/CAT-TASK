<!-- Use this file to provide workspace-specific custom instructions to Copilot. For more details, visit https://code.visualstudio.com/docs/copilot/copilot-customization#_use-a-githubcopilotinstructionsmd-file -->

# Daraz Automation Project - Copilot Instructions

## Project Overview
This is a Selenium WebDriver automation framework built with .NET C# and NUnit for testing the Daraz.com.bd e-commerce website.

## Technology Stack
- **Language**: C# (.NET 9.0)
- **Test Framework**: NUnit 4.x
- **Browser Automation**: Selenium WebDriver 4.x
- **Reporting**: ExtentReports 5.x
- **Design Pattern**: Page Object Model (POM)

## Project Structure
- `Base/` - Base classes for pages and tests
- `Pages/` - Page Object classes
- `Tests/` - Test classes
- `Data/` - Test data models and providers
- `Config/` - Configuration management
- `Utilities/` - Helper classes (Wait, Screenshot, etc.)
- `Drivers/` - WebDriver factory
- `Reports/` - Test execution reports (generated)

## Coding Standards
1. Follow Page Object Model pattern for all page interactions
2. Use descriptive method names and XML documentation
3. Implement proper wait strategies (explicit waits preferred)
4. Handle exceptions gracefully with meaningful messages
5. Use data-driven testing for parameterized tests
6. Capture screenshots on test failures

## Key Classes
- `BasePage` - Common page methods (navigation, waits, clicks)
- `BaseTest` - Test lifecycle management and logging
- `HomePage` - Daraz home page interactions
- `ConfigurationManager` - Settings from appsettings.json
- `DriverFactory` - Browser driver creation

## Running Tests
```bash
dotnet test                                    # Run all tests
dotnet test --filter "Category=SmokeTest"     # Run smoke tests
dotnet test --filter "FullyQualifiedName~LanguageChangeTests"  # Run specific test class
```
