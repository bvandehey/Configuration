using Xunit;
using Alkami.Bank;

namespace Alkami.Framework.Settings.Tests
{
    public class SettingProviderTest
    {
        [Fact]
        public void Test_Setting_InMemorySettingProvider()
        {
            var provider = new InMemorySettingProvider();
            Test_Setting_Provider_Test_Data(provider);
            Test_Setting_Provider_No_Data(provider);
        }

        [Fact]
        public void Test_Setting_GitSettingProvider()
        {
            const string sourceRepoPath = "/Users/bvandehey/Projects/VSCode/SettingsRepo";
            const string targetRepoPath = "/Users/bvandehey/Projects/VSCode/Settings/repo";

            var provider = new GitSettingProvider(sourceRepoPath, targetRepoPath);
            Test_Setting_Provider_Test_Data(provider);
            Test_Setting_Provider_No_Data(provider);
        }

        private void Test_Setting_Provider_Test_Data(ISettingProvider provider)
        {
            var origSettings = new BankSettings();
            origSettings.DeviceNumber = "My Device Number";
            origSettings.DeviceType = "My Device Type";
            origSettings.MultiplexerConfigName = "My MultiplexerConfigName";
            origSettings.IntPropertyWithNoMetadata = 5678;
            origSettings.FIType = FIType.Bank;
            //origSettings.VCardPrefix = "My VCard Prefix";
            provider.SaveSettings("BankSettings/Device/1", origSettings);
            var newSettings = provider.GetSettings<BankSettings>("BankSettings/Device/1");

            // Validate the original settings are the same as the restored settings
            Assert.Equal(origSettings.DeviceNumber, newSettings.DeviceNumber);
            Assert.Equal(origSettings.DeviceType, newSettings.DeviceType);
            Assert.Equal(origSettings.MultiplexerConfigName, newSettings.MultiplexerConfigName);
            Assert.Equal(origSettings.VCardPrefix, newSettings.VCardPrefix);
            Assert.Equal(origSettings.IntPropertyWithNoMetadata, newSettings.IntPropertyWithNoMetadata);
            Assert.Equal(origSettings.FIType, newSettings.FIType);
        }

        private void Test_Setting_Provider_No_Data(ISettingProvider provider)
        {
            var origSettings = new BankSettings();
            provider.SaveSettings("myfolder1/myfolder2/2", origSettings);
            var newSettings = provider.GetSettings<BankSettings>("myfolder1/myfolder2/2");

            // Validate the original settings are the same as the restored settings
            Assert.Equal(origSettings.DeviceNumber, newSettings.DeviceNumber);
            Assert.Equal(origSettings.DeviceType, newSettings.DeviceType);
            Assert.Equal(origSettings.MultiplexerConfigName, newSettings.MultiplexerConfigName);
            Assert.Equal(origSettings.VCardPrefix, newSettings.VCardPrefix);
            Assert.Equal(origSettings.IntPropertyWithNoMetadata, newSettings.IntPropertyWithNoMetadata);
            Assert.Equal(origSettings.FIType, newSettings.FIType);
        }
    }
}