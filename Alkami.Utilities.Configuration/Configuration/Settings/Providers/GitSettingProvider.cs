using System;
using System.IO;
using Alkami.Utilities.ArgumentValidation;
using Alkami.Utilities.Configuration.Settings.Providers.SettingStore;
using Common.Logging;
using LibGit2Sharp;

namespace Alkami.Utilities.Configuration.Settings.Providers
{
    /// <summary>
    /// /// The GitSettingProvider is an ISettingProvider use for managing settings using Git
    /// as the backing store.
    /// </summary>
    public class GitSettingProvider : ISettingProvider
    {
        private readonly string sourceRepoPath;
        private readonly string targetRepoPath;
        private readonly Repository repository;
        protected readonly ISettingStoreProvider settingStoreProvider;
        private readonly ILog logger = LogManager.GetLogger<GitSettingProvider>();

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
            Validate.That(() => targetRepoPath).IsNotNull();

            this.sourceRepoPath = sourceRepoPath;
            this.targetRepoPath = targetRepoPath;

            this.settingStoreProvider = new JsonSettingStoreProvider(this.targetRepoPath);

            if (!Directory.Exists(targetRepoPath))
            {
                if (this.sourceRepoPath != null)
                {
                    logger.Info($"Cloning repository {sourceRepoPath} to {targetRepoPath}...");
                    CloneOptions options = new CloneOptions();
                    options.Checkout = true;
                    var repopath = Repository.Clone(sourceRepoPath, targetRepoPath, options);
                    logger.Info($"Cloning repository {sourceRepoPath} to {targetRepoPath}...done");
                    logger.Info($"Opening repository {targetRepoPath} ...");
                    repository = new Repository(repopath);
                    logger.Info($"Opening repository {targetRepoPath} ...done");
                }
                else
                {
                    logger.Info($"Initializing repository {targetRepoPath}...");
                    var repopath = Repository.Init(targetRepoPath);
                    repository = new Repository(repopath);
                    logger.Info($"Initializing repository {targetRepoPath} ...done");
                }
            }

        }

        /// <summary>
        /// This method gets the settings associated with a particular namespaceIdentifier.
        /// </summary>
        /// <param name="namespaceIdentifier">The identifier of the namespace that determine which settings to retrieve.</param>
        /// <returns>Returns an instance of &lt;T&gt; for the particular namespaceIdentifier</returns>
        /// <exception cref="ArgumentNullException">Thrown when the namespaceIdentifier parameter is null</exception>
        public T GetSettings<T>(string namespaceIdentifier) where T : BaseSettings, new()
        {
            Validate.That(() => namespaceIdentifier).IsNotNull();
            var settings = new T();
            settings.SetProperties(this.settingStoreProvider.GetSettings(namespaceIdentifier));

            return settings;
        }

        /// <summary>
        /// This method saves the settings associated with a particular namespaceIdentifier.
        /// </summary>
        /// <param name="namespaceIdentifier">The identifier of the namespace to determine where to save the settings.</param>
        /// <param name="settings"></param>
        /// <exception cref="ArgumentNullException">Thrown when the namespaceIdentifier parameter is null</exception>
        public void SaveSettings<T>(string namespaceIdentifier, T settings) where T : BaseSettings
        {
            Validate.That(() => namespaceIdentifier).IsNotNull();
            this.settingStoreProvider.SaveSettings(namespaceIdentifier, settings.GetChangedProperties(), settings.GetType());
        }
    }
}