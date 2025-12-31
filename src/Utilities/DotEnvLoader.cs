using System;
using System.IO;

namespace DarazAutomation.Utilities
{
    /// <summary>
    /// Helper class to load environment variables from .env file
    /// Follows the dotenv pattern for easy credential management
    /// </summary>
    public static class DotEnvLoader
    {
        /// <summary>
        /// Loads environment variables from .env file if it exists
        /// Call this before accessing ConfigurationManager
        /// </summary>
        public static void Load()
        {
            var envFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".env");
            
            if (!File.Exists(envFilePath))
            {
                // .env file is optional - environment variables or appsettings.json will be used
                return;
            }

            try
            {
                foreach (var line in File.ReadAllLines(envFilePath))
                {
                    var trimmedLine = line.Trim();
                    
                    // Skip empty lines and comments
                    if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith("#"))
                    {
                        continue;
                    }

                    // Parse KEY=VALUE format
                    var separatorIndex = trimmedLine.IndexOf('=');
                    if (separatorIndex > 0)
                    {
                        var key = trimmedLine.Substring(0, separatorIndex).Trim();
                        var value = trimmedLine.Substring(separatorIndex + 1).Trim();
                        
                        // Remove quotes if present
                        if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                            (value.StartsWith("'") && value.EndsWith("'")))
                        {
                            value = value.Substring(1, value.Length - 2);
                        }
                        
                        // Only set if not already set (environment variables take precedence)
                        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(key)))
                        {
                            Environment.SetEnvironmentVariable(key, value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Failed to load .env file: {ex.Message}");
                // Don't throw - .env file is optional
            }
        }
        
        /// <summary>
        /// Loads environment variables from a custom .env file path
        /// </summary>
        /// <param name="filePath">Path to the .env file</param>
        public static void Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($".env file not found at: {filePath}");
            }

            try
            {
                foreach (var line in File.ReadAllLines(filePath))
                {
                    var trimmedLine = line.Trim();
                    
                    if (string.IsNullOrWhiteSpace(trimmedLine) || trimmedLine.StartsWith("#"))
                    {
                        continue;
                    }

                    var separatorIndex = trimmedLine.IndexOf('=');
                    if (separatorIndex > 0)
                    {
                        var key = trimmedLine.Substring(0, separatorIndex).Trim();
                        var value = trimmedLine.Substring(separatorIndex + 1).Trim();
                        
                        if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
                            (value.StartsWith("'") && value.EndsWith("'")))
                        {
                            value = value.Substring(1, value.Length - 2);
                        }
                        
                        if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(key)))
                        {
                            Environment.SetEnvironmentVariable(key, value);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to load .env file from {filePath}: {ex.Message}", ex);
            }
        }
    }
}
