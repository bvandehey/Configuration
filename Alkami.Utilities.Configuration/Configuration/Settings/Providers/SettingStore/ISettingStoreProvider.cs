// // --------------------------------------------------------------------------------------------------------------------
// // <copyright file="ISettingStoreProvider.cs" company="Alkami Technology, Inc.">
// //   Copyright 2013 Alkami Technology, Inc.  All rights reserved.
// // </copyright>
// // <summary>
// //
// // </summary>
// // --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace Alkami.Utilities.Configuration.Settings.Providers.SettingStore
{
    /// <summary>
    /// The ISettingStoreProvider interface is used to implement a settings store for persisting settings.
    /// This can be used by <see cref="ISettingProvider"/> to support different formats for storing the settings.
    /// </summary>
    /// <seealso cref="GitSettingProvider"/>
    public interface ISettingStoreProvider
    {
        /// <summary>
        /// Saves the settings to the Setting Store using the namespace identifier as the key for retrieving the settings.
        /// </summary>
        /// <param name="namespaceIdentifier">The namespace identifier to associated with the settings.</param>
        /// <param name="settings">A name/value dictionary that contains the settings to store.</param>
        /// <param name="settingsType">Class Type of the settings that are being stored.</param>
        /// <exception cref="ArgumentNullException">Thrown if either the namespaceIdentifier or the settingsType are null.</exception>
        void SaveSettings(string namespaceIdentifier, Dictionary<string, string> settings, Type settingsType);

        /// <summary>
        /// Gets the settings from the Setting Store using the namespace identifier as the key for retrieving the settings.
        /// </summary>
        /// <param name="namespaceIdentifier">The namespace identifier associated with the settings to retrieve.</param>
        /// <returns>Returns a name/value dictionary containing the settings or null if nothing exists.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the namespaceIdentifier is null.</exception>
        Dictionary<string, string> GetSettings(string namespaceIdentifier);
    }
}