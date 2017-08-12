using System;
using System.Collections.Generic;

namespace Alkami.Framework.Settings
{
    /// <summary>
    /// /// The InMemorySettingProvider is an ISettingProvider use for managing settings in a test environment.
    /// </summary>
    public class InMemorySettingProvider : ISettingProvider
    {
        private Dictionary<string, Dictionary<string, string>> savedSettings = new Dictionary<string, Dictionary<string, string>>();
        
        /// <summary>
        /// This method gets the settings associated with a particular namespaceIdentifier.
        /// </summary>
        /// <param name="namespaceIdentifier">The identifier of the namespace that determine which settings to retrieve.</param>
        /// <returns>Returns an instance of <T> for the particular namespaceIdentifier</returns>
        /// <exception cref="ArgumentNullException">Thrown when the namespaceIdentifier parameter is null</exception>
        public T GetSettings<T>(string namespaceIdentifier) where T : BaseSettings, new()
        {
            if (namespaceIdentifier == null)
                throw new ArgumentNullException(nameof(namespaceIdentifier));

            var settings = new T();

            if (this.savedSettings.ContainsKey(namespaceIdentifier))
            {
                var savedSettings = this.savedSettings[namespaceIdentifier];
                if (savedSettings != null)
                {
                    settings.SetProperties(savedSettings);
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
            if (namespaceIdentifier == null)
                throw new ArgumentNullException(nameof(namespaceIdentifier));
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));
                
            var changedSettings = settings.GetChangedProperties();
            this.savedSettings[namespaceIdentifier] = changedSettings;
        }
    }
}