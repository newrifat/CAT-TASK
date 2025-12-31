using DarazAutomation.Config;
using NUnit.Framework;

namespace DarazAutomation.Data
{
    /// <summary>
    /// Provides test data for data-driven testing
    /// </summary>
    public static class TestDataProvider
    {
        /// <summary>
        /// Provides test data for language change scenarios
        /// </summary>
        public static IEnumerable<TestCaseData> LanguageChangeTestData()
        {
            yield return new TestCaseData(
                new LanguageTestData
                {
                    SourceLanguage = ConfigurationManager.EnglishText,
                    TargetLanguage = ConfigurationManager.BanglaText,
                    ExpectedVerificationText = ConfigurationManager.BanglaVerificationText,
                    TestDescription = "Change language from English to Bangla"
                }
            ).SetName("ChangeLanguage_EnglishToBangla");

            yield return new TestCaseData(
                new LanguageTestData
                {
                    SourceLanguage = ConfigurationManager.BanglaText,
                    TargetLanguage = ConfigurationManager.EnglishText,
                    ExpectedVerificationText = ConfigurationManager.EnglishVerificationText,
                    TestDescription = "Change language from Bangla to English"
                }
            ).SetName("ChangeLanguage_BanglaToEnglish");
        }

        /// <summary>
        /// Provides sequential language change test data for full cycle test
        /// </summary>
        public static IEnumerable<LanguageTestData> GetLanguageCycleTestData()
        {
            return new List<LanguageTestData>
            {
                new LanguageTestData
                {
                    SourceLanguage = ConfigurationManager.EnglishText,
                    TargetLanguage = ConfigurationManager.BanglaText,
                    ExpectedVerificationText = ConfigurationManager.BanglaVerificationText,
                    TestDescription = "Step 1: Change from English to Bangla"
                },
                new LanguageTestData
                {
                    SourceLanguage = ConfigurationManager.BanglaText,
                    TargetLanguage = ConfigurationManager.EnglishText,
                    ExpectedVerificationText = ConfigurationManager.EnglishVerificationText,
                    TestDescription = "Step 2: Change from Bangla back to English"
                }
            };
        }
    }
}
