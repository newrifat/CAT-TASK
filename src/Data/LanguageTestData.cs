namespace DarazAutomation.Data
{
    /// <summary>
    /// Test data model for language change tests
    /// </summary>
    public class LanguageTestData
    {
        public string SourceLanguage { get; set; } = string.Empty;
        public string TargetLanguage { get; set; } = string.Empty;
        public string ExpectedVerificationText { get; set; } = string.Empty;
        public string TestDescription { get; set; } = string.Empty;
    }
}
