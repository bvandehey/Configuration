using System;
using System.Collections.Generic;
using System.IO;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;

namespace Alkami.Framework.Settings
{
    /// <summary>
    /// /// The GitSettingProvider is an ISettingProvider use for managing settings using Git
    /// as the backing store.
    /// </summary>
    public class GitSettingProvider : ISettingProvider
    {
        private const char SettingDelimiter = '=';

        private readonly string sourceRepoPath = "/Users/bvandehey/Projects/VSCode/SettingsRepo";
        private readonly string targetRepoPath = "/Users/bvandehey/Projects/VSCode/Settings/repo";
        private readonly Repository repository;
        private readonly ILogger logger = ApplicationLogging.CreateLogger<GitSettingProvider>();

        private Dictionary<string, Dictionary<string, string>> savedSettings = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// Create an instance of the GitSettingProvider initializing with just
        /// a target path.
        /// </summary>
        /// <param name="targetRepoPath">The path of the Git repo. If the repo 
        /// doesn't exist, it will be created</param>
        public GitSettingProvider(string targetRepoPath)
            : this(null, targetRepoPath)
        {
        }

        /// <summary>
        /// Create an instance of the GitSettingProvider initializing with a source
        /// repo to clone and track into the target path.
        /// </summary>
        /// <param name="sourceRepoPath">The path of the parent Git repo to clone
        /// or track to</param>
        /// <param name="targetRepoPath">The path of the Git repo. If the repo 
        /// doesn't exist, it will be created</param>
        public GitSettingProvider(string sourceRepoPath, string targetRepoPath)
        {
            if (targetRepoPath == null)
                throw new ArgumentNullException(nameof(targetRepoPath));

            this.sourceRepoPath = sourceRepoPath;
            this.targetRepoPath = targetRepoPath;

            if (!Directory.Exists(targetRepoPath))
            {
                if (this.sourceRepoPath != null)
                {
                    logger.LogInformation($"Cloning repository {sourceRepoPath} to {targetRepoPath}...");
                    CloneOptions options = new CloneOptions();
                    options.Checkout = true;
                    var repopath = Repository.Clone(sourceRepoPath, targetRepoPath, options);
                    logger.LogInformation($"Cloning repository {sourceRepoPath} to {targetRepoPath}...done");
                    logger.LogInformation($"Opening repository {targetRepoPath} ...");
                    repository = new Repository(repopath);
                    logger.LogInformation($"Opening repository {targetRepoPath} ...done");
                }
                else
                {
                    logger.LogInformation($"Initializing repository {targetRepoPath}...");
                    var repopath = Repository.Init(targetRepoPath);
                    repository = new Repository(repopath);
                    logger.LogInformation($"Initializing repository {targetRepoPath} ...done");
                }
            }

        }

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

            var savedSettings = new Dictionary<string, string>();
            var settingFilename = GetNamespaceFilename(namespaceIdentifier);
            if (File.Exists(settingFilename))
            {
                logger.LogDebug($"Reading Settings File: {settingFilename}");
                using (var sr = new StreamReader(File.OpenRead(settingFilename)))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            var posDelimiter = line.IndexOf(SettingDelimiter);
                            if (posDelimiter >= 0)
                            {
                                var key = line.Substring(0, posDelimiter);
                                var value = line.Substring(posDelimiter + 1);
                                logger.LogTrace($"Setting Read: Key:{key}, Value:{value}");
                                savedSettings.Add(key, value);
                            }
                        }
                    }
                }

                settings.SetProperties(savedSettings);
            }
            else
            {
                logger.LogTrace($"No Settings File Found: {settingFilename}. Using default settings.");
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
            var settingFilename = GetNamespaceFilename(namespaceIdentifier);
            logger.LogDebug($"Creating Settings File: {settingFilename}");
            // Only create the settings file if there are settings
            // If the file already exists, don't delete it - just clear all the settings from it
            if (changedSettings.Count > 0 || File.Exists(settingFilename))
            {
                EnsurePathExists(settingFilename);
                using (var sw = File.CreateText(settingFilename))
                {
                    foreach (var setting in changedSettings)
                    {
                        logger.LogTrace($"Writing Setting: {setting.Key}={setting.Value}");
                        sw.WriteLine($"{setting.Key}{SettingDelimiter}{setting.Value}");
                    }
                }
            }
        }

        protected void EnsurePathExists(string settingFilename)
        {
            var directory = Path.GetDirectoryName(settingFilename);
            logger.LogTrace($"Ensuring directory exists: {directory}");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        protected string GetNamespaceFilename(string namespaceIdentifier)
        {
            return Path.Combine(targetRepoPath, namespaceIdentifier) + ".settings";
        }
    }
}