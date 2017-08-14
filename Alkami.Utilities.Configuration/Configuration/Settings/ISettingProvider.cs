namespace Alkami.Utilities.Configuration.Settings
{
    /// <summary>
    /// The ISettingProvider interface is use to define a provider for managing settings.
    /// </summary>
    public interface ISettingProvider
    {
        /// <summary>
        /// This method gets the settings associated with a particular instance.
        /// </summary>
        /// <param name="instanceId">The ID that identifies which settings to retrieve.</param>
        /// <returns>Returns an instance of <T> for the particular instanceId</returns>
        T GetSettings<T>(string instanceId) where T : BaseSettings, new();
        
        /// <summary>
        /// This method saves the settings associated with a particular instance.
        /// </summary>
        /// <param name="instanceId"></param>
        /// <param name="settings"></param>
        void SaveSettings<T>(string instanceId, T settings) where T : BaseSettings;
    }
}