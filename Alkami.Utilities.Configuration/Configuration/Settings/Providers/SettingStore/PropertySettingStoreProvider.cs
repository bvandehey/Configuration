using System;
using System.Collections.Generic;
using System.IO;
using Alkami.Utilities.ArgumentValidation;
using Common.Logging;

namespace Alkami.Utilities.Configuration.Settings.Providers.SettingStore
{
    /// <summary>
    /// The PropertySettingStoreProvider class is an implementation of <see cref="ISettingStoreProvider"/>
    /// that stores the settings in a property file in the format of name=value.
    /// </summary>
    /// <seealso cref="ISettingStoreProvider" />
    public class PropertySettingStoreProvider : FilebasedSettingStoreProvider, ISettingStoreProvider
    {
        private const char SettingDelimiter = '=';
        private const string CommentDelimiter = "#";

        private readonly ILog logger = LogManager.GetLogger<PropertySettingStoreProvider>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySettingStoreProvider"/> class.
        /// </summary>
        /// <param name="settingStorePath">The path to the location of the setting store.</param>
        public PropertySettingStoreProvider(string settingStorePath)
            :base(settingStorePath)
        {
        }

        /// <summary>
        /// Saves the settings to the Setting Store using the namespace identifier as the key for retrieving the settings.
        /// </summary>
        /// <param name="namespaceIdentifier">The namespace identifier to associated with the settings.</param>
        /// <param name="settings">A name/value dictionary that contains the settings to store.</param>
        /// <param name="settingsType">Class Type of the settings that are being stored.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the namespaceIdentifier or settingsType parameter is null.</exception>
        public void SaveSettings(string namespaceIdentifier, Dictionary<string, string> settings, Type settingsType)
        {
            Validate.That(() => namespaceIdentifier).IsNotNull();
            Validate.That(() => settingsType).IsNotNull();

            var settingFilename = GetNamespaceFilename(namespaceIdentifier);
            logger.Debug($"Creating Settings File: {settingFilename}");
            // Only create the settings file if there are settings
            // If the file already exists, don't delete it - just clear all the settings from it
            if (settings != null && (settings.Count > 0 || File.Exists(settingFilename)))
            {
                EnsurePathExists(settingFilename);
                using (var sw = File.CreateText(settingFilename))
                {
                    sw.WriteLine($"{CommentDelimiter} {settingsType.AssemblyQualifiedName}");
                    foreach (var setting in settings)
                    {
                        logger.Trace($"Writing Setting: {setting.Key}={setting.Value}");
                        sw.WriteLine($"{setting.Key}{SettingDelimiter}{setting.Value.Replace(@"\" ,@"\\").Replace("\n", @"\n").Replace("\r", @"\r")}");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the settings from the Setting Store using the namespace identifier as the key for retrieving the settings.
        /// </summary>
        /// <param name="namespaceIdentifier">The namespace identifier associated with the settings to retrieve.</param>
        /// <returns>Returns a name/value dictionary containing the settings or null if nothing exists.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the namespaceIdentifier parameter is null.</exception>
        public Dictionary<string, string> GetSettings(string namespaceIdentifier)
        {
            Validate.That(() => namespaceIdentifier).IsNotNull();

            var settings = new Dictionary<string, string>();
            var settingFilename = GetNamespaceFilename(namespaceIdentifier);
            if (File.Exists(settingFilename))
            {
                logger.Debug($"Reading Settings File: {settingFilename}");
                using (var sr = new StreamReader(File.OpenRead(settingFilename)))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith(CommentDelimiter))
                        {
                            var posDelimiter = line.IndexOf(SettingDelimiter);
                            if (posDelimiter >= 0)
                            {
                                var key = line.Substring(0, posDelimiter);
                                var value = line.Substring(posDelimiter + 1);
                                logger.Trace($"Setting Read: Key:{key}, Value:{value}");
                                settings.Add(key, value.Replace(@"\\", "~~~~").Replace(@"\n", "\n").Replace(@"\r", "\r").Replace("~~~~", @"\"));
                            }
                        }
                    }
                }
            }
            else
            {
                logger.Trace($"No Settings File Found: {settingFilename}. Using default settings.");
            }

            return settings;
        }
    }
}