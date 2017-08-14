using System;
using System.Collections.Generic;
using Alkami.Utilities.ArgumentValidation;

namespace Alkami.Utilities.Configuration.Settings.Providers
{
    /// <summary>
    /// /// The InMemorySettingProvider is an ISettingProvider use for managing settings in a test environment.
    /// </summary>
    public class InMemorySettingProvider : ISettingProvider
    {
        private readonly Dictionary<string, Dictionary<string, string>> savedSettings = new Dictionary<string, Dictionary<string, string>>();
        
        /// <summary>
        /// This method gets the settings associated with a particular namespaceIdentifier.
        /// </summary>
        /// <param name="namespaceIdentifier">The identifier of the namespace that determine which settings to retrieve.</param>
        /// <returns>Returns an instance of <T> for the particular namespaceIdentifier</returns>
        /// <exception cref="ArgumentNullException">Thrown when the namespaceIdentifier parameter is null</exception>
        public T GetSettings<T>(string namespaceIdentifier) where T : BaseSettings, new()
        {
            Validate.That(() => namespaceIdentifier).IsNotNull();

            var settings = new T();

            if (this.savedSettings.ContainsKey(namespaceIdentifier))
            {
                var foundSettings = this.savedSettings[namespaceIdentifier];
                if (foundSettings != null)
                {
                    settings.SetProperties(foundSettings);
                }
            }
            return settings;
        }

        /// <summary>
        /// This method saves the settings associated with a particular namespaceIdentifier.
        /// </summary>
        /// <param name="namespaceIdentifier">The identifier of the namespace to determine where to save the settings.</param>
        /// <param name="settings"></param>
        /// <exception cref="ArgumentNullException">Thrown when either the namespaceIdentifier or settings parameter is null</exception>
        public void SaveSettings<T>(string namespaceIdentifier, T settings) where T : BaseSettings
        {
            Validate.That(() => namespaceIdentifier).IsNotNull();
            Validate.That(() => settings).IsNotNull();

            var changedSettings = settings.GetChangedProperties();
            this.savedSettings[namespaceIdentifier] = changedSettings;
        }
    }
}