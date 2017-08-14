using System.Collections.Generic;
using Alkami.Utilities.Configuration.Settings;
using NUnit.Framework;

namespace Alkami.Utilities.Tests.Configuration
{
    public class SettingDescriptorTest
    {
        [Test]
        public void SettingDescriptor_Collection_Matches_Expected_Values() 
        {
            var settings = new TestSettings();
            var descriptors = settings.SettingDescriptors;

            /*
                    [SettingDescriptor(Name="DEVICENUMBER", DisplayName="Device Number", Description="Symconnect device number", IsRequired=true, Order=1)]
                    public string DeviceNumber { get; set; } = "0";

                    [SettingDescriptor(DisplayName="My Device Type", Description="Symconnect device type", IsRequired=true, Order=2)]
                    public string DeviceType { get; set; } = "ALK";

                    [SettingDescriptor(Description="Symconnect VCard prefix", IsRequired=true, Order=3)]
                    public string VCardPrefix { get; set; } = "5050";

                    [SettingDescriptor(Description="Symconnect Multiplexer Config Name", IsRequired=false, Order=4)]
                    public string MultiplexerConfigName { get; set; } = "BAXTER";

                    [SettingDescriptor(Order=5)]
                    public int IntPropertyWithNoMetadata { get; set; } = 1234;
            */
            var expectedDescriptors = new List<SettingDescriptor>(6)
            {
                new SettingDescriptor { Name = "FIType", DisplayName = "FI Type", Description = "", IsRequired = false, Order = 0, DefaultValue = settings.FIType },
                new SettingDescriptor { Name = "DEVICENUMBER", DisplayName = "Device Number", Description = "Symconnect device number", IsRequired = true, Order = 1, DefaultValue = settings.DeviceNumber },
                new SettingDescriptor { Name = "DeviceType", DisplayName = "Device Type", Description = "Symconnect device type", IsRequired = true, Order = 2, DefaultValue = settings.DeviceType },
                new SettingDescriptor { Name = "VCardPrefix", DisplayName = "VCard Prefix", Description = "Symconnect VCard prefix", IsRequired = true, Order = 3, DefaultValue = settings.VCardPrefix },
                new SettingDescriptor { Name = "MultiplexerConfigName", DisplayName = "Multiplexer Config Name", Description = "Symconnect Multiplexer Config Name", IsRequired = false, Order = 4, DefaultValue = settings.MultiplexerConfigName },
                new SettingDescriptor { Name = "IntPropertyWithNoMetadata", DisplayName = "Int Property With No Metadata", Description = "", IsRequired = false, Order = 5, DefaultValue = settings.IntPropertyWithNoMetadata },
            };
            //Assert.That(expectedDescriptors, Is.EquivalentTo(descriptors));
            /*
                    Assert.Collection<Alkami.Framework.Settings.SettingDescriptor>(descriptors, 
                        sd => { 
                            Assert.Equal("FIType", sd.Name);
                            Assert.Equal("FI Type", sd.DisplayName);
                            Assert.Equal("", sd.Description);
                            Assert.Equal(false, sd.IsRequired);
                            Assert.Equal(0, sd.Order);
                            Assert.Equal(settings.FIType, sd.DefaultValue);
                        }, 
                        sd => { 
                            Assert.Equal("DEVICENUMBER", sd.Name);
                            Assert.Equal("Device Number", sd.DisplayName);
                            Assert.Equal("Symconnect device number", sd.Description);
                            Assert.Equal(true, sd.IsRequired);
                            Assert.Equal(1, sd.Order);
                            Assert.Equal(settings.DeviceNumber, sd.DefaultValue);
                        }, 
                        sd => { 
                            Assert.Equal("DeviceType", sd.Name);
                            Assert.Equal("Device Type", sd.DisplayName);
                            Assert.Equal("Symconnect device type", sd.Description);
                            Assert.Equal(true, sd.IsRequired);
                            Assert.Equal(2, sd.Order);
                            Assert.Equal(settings.DeviceType, sd.DefaultValue);
                        }, 
                        sd => { 
                            Assert.Equal("VCardPrefix", sd.Name);
                            Assert.Equal("VCard Prefix", sd.DisplayName);
                            Assert.Equal("Symconnect VCard prefix", sd.Description);
                            Assert.Equal(true, sd.IsRequired);
                            Assert.Equal(3, sd.Order);
                            Assert.Equal(settings.VCardPrefix, sd.DefaultValue);
                        }, 
                        sd => { 
                            Assert.Equal("MultiplexerConfigName", sd.Name);
                            Assert.Equal("Multiplexer Config Name", sd.DisplayName);
                            Assert.Equal("Symconnect Multiplexer Config Name", sd.Description);
                            Assert.Equal(false, sd.IsRequired);
                            Assert.Equal(4, sd.Order);
                            Assert.Equal(settings.MultiplexerConfigName, sd.DefaultValue);
                        },
                        sd => { 
                            Assert.Equal("IntPropertyWithNoMetadata", sd.Name);
                            Assert.Equal("Int Property With No Metadata", sd.DisplayName);
                            Assert.Equal("", sd.Description);
                            Assert.Equal(false, sd.IsRequired);
                            Assert.Equal(5, sd.Order);
                            Assert.Equal(settings.IntPropertyWithNoMetadata, sd.DefaultValue);
                        }
                    );
                    */
        }
    }
}
