using Alkami.Utilities.Configuration.Settings;
using Alkami.Utilities.Configuration.Settings.Providers;
using Common.Logging.Configuration;
using NUnit.Framework;

namespace Alkami.Utilities.Tests.Configuration
{
    [TestFixture]
    public class SettingProviderTest
    {
        [SetUp]
        public void Test_Setup()
        {
            // Setup logging
            NameValueCollection properties = new NameValueCollection
            {
                ["showDateTime"] = "false"
            };
            Common.Logging.LogManager.Adapter = new Common.Logging.Simple.ConsoleOutLoggerFactoryAdapter(properties);
        }

        [Test]
        public void Test_Setting_InMemorySettingProvider()
        {
            var provider = new InMemorySettingProvider();
            //Test_Setting_Provider_Test_Data(provider);
            //Test_Setting_Provider_No_Data(provider);
        }

        [Test]
        public void Test_Setting_GitSettingProvider()
        {
            const string sourceRepoPath = null; //"/Users/bvandehey/Projects/VSCode/SettingsRepo";
            const string targetRepoPath = @"C:\Temp\Settings\repo";

            var provider = new GitSettingProvider(sourceRepoPath, targetRepoPath);
            Test_Setting_Provider_Test_Data(provider);
            //Test_Setting_Provider_No_Data(provider);
        }

        private void Test_Setting_Provider_Test_Data(ISettingProvider provider)
        {
            var origSettings = new TestSettings();
            origSettings.DeviceNumber = "My Device Number";
            origSettings.DeviceType = "My Device Type";
            origSettings.MultiplexerConfigName = "My MultiplexerConfigName";
            origSettings.IntPropertyWithNoMetadata = 5678;
            origSettings.FIType = FIType.Bank;
            origSettings.DummyProperty = "My Dummy String with a newline \n and a quote \" in the middle";
            origSettings.VCardPrefix = "My VCard Prefix";
            origSettings.EmbeddedClass.EmbeddedProperty1 = "A changed property 1";
            origSettings.EmbeddedClass.EmbeddedProperty2 = "Another changed property 2";
            origSettings.EmbeddedClass.AnotherEmbeddedClass.AnotherEmbeddedProperty1 = "Still yet another embedded property 1";
            origSettings.HtmlProperty = @"<html>
    <head>
        <title>
            This is the title\test/another
        </title>
    </head>
    <body>
        <h1>Some header</h1>
    </body>
</html>
";
            provider.SaveSettings("TestSettings/Device/1", origSettings);
            var newSettings = provider.GetSettings<TestSettings>("TestSettings/Device/1");

            // Validate the original settings are the same as the restored settings
            Assert.AreEqual(origSettings.DeviceNumber, newSettings.DeviceNumber);
            Assert.AreEqual(origSettings.DeviceType, newSettings.DeviceType);
            Assert.AreEqual(origSettings.MultiplexerConfigName, newSettings.MultiplexerConfigName);
            Assert.AreEqual(origSettings.VCardPrefix, newSettings.VCardPrefix);
            Assert.AreEqual(origSettings.IntPropertyWithNoMetadata, newSettings.IntPropertyWithNoMetadata);
            Assert.AreEqual(origSettings.FIType, newSettings.FIType);
            Assert.AreEqual(origSettings.DummyProperty, newSettings.DummyProperty);
            Assert.AreEqual(origSettings.EmbeddedClass.EmbeddedProperty1, newSettings.EmbeddedClass.EmbeddedProperty1);
            Assert.AreEqual(origSettings.EmbeddedClass.EmbeddedProperty2, newSettings.EmbeddedClass.EmbeddedProperty2);
            Assert.AreEqual(origSettings.EmbeddedClass.AnotherEmbeddedClass.AnotherEmbeddedProperty1, newSettings.EmbeddedClass.AnotherEmbeddedClass.AnotherEmbeddedProperty1);
            Assert.AreEqual(origSettings.HtmlProperty, newSettings.HtmlProperty);
        }

        private void Test_Setting_Provider_No_Data(ISettingProvider provider)
        {
            var origSettings = new TestSettings();
            provider.SaveSettings("myfolder1/myfolder2/2", origSettings);
            var newSettings = provider.GetSettings<TestSettings>("myfolder1/myfolder2/2");

            // Validate the original settings are the same as the restored settings
            Assert.AreEqual(origSettings.DeviceNumber, newSettings.DeviceNumber);
            Assert.AreEqual(origSettings.DeviceType, newSettings.DeviceType);
            Assert.AreEqual(origSettings.MultiplexerConfigName, newSettings.MultiplexerConfigName);
            Assert.AreEqual(origSettings.VCardPrefix, newSettings.VCardPrefix);
            Assert.AreEqual(origSettings.IntPropertyWithNoMetadata, newSettings.IntPropertyWithNoMetadata);
            Assert.AreEqual(origSettings.FIType, newSettings.FIType);
            Assert.AreEqual(origSettings.DummyProperty, newSettings.DummyProperty);
        }
    }
}