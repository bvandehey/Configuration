using System;
using System.IO;

namespace Alkami.Utilities.Configuration.Settings.Providers.SettingStore
{
    /// <summary>
    /// The FilebasedSettingStoreProvider class is an abstract base class for file-based implementations of <see cref="ISettingStoreProvider"/>.
    /// </summary>
    /// <seealso cref="ISettingStoreProvider" />
    /// <seealso cref="JsonSettingStoreProvider" />
    /// <seealso cref="PropertySettingStoreProvider" />
    public abstract class FilebasedSettingStoreProvider
    {
        protected readonly string settingStorePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertySettingStoreProvider"/> class.
        /// </summary>
        /// <param name="settingStorePath">The path to the location of the setting store.</param>
        public FilebasedSettingStoreProvider(string settingStorePath)
        {
            this.settingStorePath = settingStorePath;
        }

        /// <summary>
        /// Gets the namespace filename given the namespace identifier.
        /// </summary>
        /// <param name="namespaceIdentifier">The namespace identifier.</param>
        /// <returns>Returns a string that is a full path to the filename for the given namespace.</returns>
        protected string GetNamespaceFilename(string namespaceIdentifier)
        {
            return Path.Combine(settingStorePath, namespaceIdentifier) + ".settings";
        }

        /// <summary>
        /// A protected method that ensures the directories in the path of the filename exists.
        /// </summary>
        /// <param name="settingFilename">The setting filename.</param>
        protected void EnsurePathExists(string settingFilename)
        {
            var directory = Path.GetDirectoryName(settingFilename);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }
    }
}