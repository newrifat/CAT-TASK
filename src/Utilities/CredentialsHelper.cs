using System.Security;
using System.Runtime.InteropServices;
using DarazAutomation.Config;

namespace DarazAutomation.Utilities
{
    /// <summary>
    /// Helper class for managing test credentials securely
    /// Provides methods for credential handling and masking
    /// </summary>
    public static class CredentialsHelper
    {
        /// <summary>
        /// Get login credentials from configuration
        /// </summary>
        public static (string email, string password, string phone, string method) GetLoginCredentials()
        {
            return (
                ConfigurationManager.LoginEmail,
                ConfigurationManager.LoginPassword,
                ConfigurationManager.LoginPhone,
                ConfigurationManager.LoginMethod
            );
        }

        /// <summary>
        /// Mask password for logging (show only first and last character)
        /// </summary>
        public static string MaskPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return "****";

            if (password.Length <= 2)
                return new string('*', password.Length);

            return $"{password[0]}{new string('*', password.Length - 2)}{password[^1]}";
        }

        /// <summary>
        /// Mask email (show only first 3 chars and domain)
        /// </summary>
        public static string MaskEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return "***@***.***";

            int atIndex = email.IndexOf('@');
            if (atIndex <= 0)
                return "***@***.***";

            string localPart = email.Substring(0, atIndex);
            string domain = email.Substring(atIndex);

            if (localPart.Length <= 3)
                return new string('*', localPart.Length) + domain;

            return localPart.Substring(0, 3) + new string('*', localPart.Length - 3) + domain;
        }

        /// <summary>
        /// Mask phone number (show only last 4 digits)
        /// </summary>
        public static string MaskPhoneNumber(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return "***-***-****";

            if (phone.Length <= 4)
                return new string('*', phone.Length);

            return new string('*', phone.Length - 4) + phone.Substring(phone.Length - 4);
        }

        /// <summary>
        /// Convert string to SecureString for enhanced security
        /// </summary>
        public static SecureString ToSecureString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return new SecureString();

            var secureString = new SecureString();
            foreach (char c in input)
            {
                secureString.AppendChar(c);
            }
            secureString.MakeReadOnly();
            return secureString;
        }

        /// <summary>
        /// Convert SecureString back to plain string
        /// </summary>
        public static string ToPlainString(SecureString secureString)
        {
            if (secureString == null)
                return string.Empty;

            IntPtr ptr = IntPtr.Zero;
            try
            {
                ptr = Marshal.SecureStringToGlobalAllocUnicode(secureString);
                return Marshal.PtrToStringUni(ptr) ?? string.Empty;
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.ZeroFreeGlobalAllocUnicode(ptr);
            }
        }

        /// <summary>
        /// Validate email format
        /// </summary>
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Validate phone number format (basic validation)
        /// </summary>
        public static bool IsValidPhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            // Remove common formatting characters
            string cleanPhone = phone.Replace("-", "").Replace(" ", "").Replace("(", "").Replace(")", "");

            // Check if all characters are digits
            return cleanPhone.All(char.IsDigit) && cleanPhone.Length >= 10 && cleanPhone.Length <= 15;
        }

        /// <summary>
        /// Get credential summary for logging (masked)
        /// </summary>
        public static string GetCredentialSummary()
        {
            var (email, password, phone, method) = GetLoginCredentials();

            return $"Login Method: {method}\n" +
                   $"Email: {MaskEmail(email)}\n" +
                   $"Phone: {MaskPhoneNumber(phone)}\n" +
                   $"Password: {MaskPassword(password)}";
        }
    }
}
