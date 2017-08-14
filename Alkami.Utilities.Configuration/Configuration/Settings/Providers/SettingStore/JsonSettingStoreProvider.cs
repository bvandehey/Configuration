using System;
using System.Collections.Generic;
using System.IO;
using Alkami.Utilities.ArgumentValidation;
using Common.Logging;
using Newtonsoft.Json;

namespace Alkami.Utilities.Configuration.Settings.Providers.SettingStore
{
    /// <summary>
    /// The JsonSettingStoreProvider class is an implementation of <see cref="ISettingStoreProvider"/>
    /// that stores the settings in a property file in JSON format.
    /// </summary>
    /// <seealso cref="ISettingStoreProvider" />
    public class JsonSettingStoreProvider : FilebasedSettingStoreProvider, ISettingStoreProvider
    {
        private readonly ILog logger = LogManager.GetLogger<PropertySettingStoreProvider>();

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySettingStoreProvider"/> class.
        /// </summary>
        /// <param name="settingStorePath">The path to the location of the setting store.</param>
        public JsonSettingStoreProvider(string settingStorePath)
            : base(settingStorePath)
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
                using (var jsonWriter = new JsonTextWriter(sw))
                {
                    jsonWriter.Formatting = Formatting.Indented;
                    var serializer = new JsonSerializer();
                    serializer.Serialize(jsonWriter, settings);
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
                using (var jsonReader = new JsonTextReader(sr))
                {
                    var serializer = new JsonSerializer();
                    settings = serializer.Deserialize<Dictionary<string, string>>(jsonReader);
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