using Alkami.Utilities.Configuration.Settings;

namespace Alkami.Utilities.Tests.Configuration
{
	public class EmbeddedClass
	{
		[SettingDescriptor(Description="Embedded Property 1")]
		public string EmbeddedProperty1 { get; set; } = "Property 1";

		[SettingDescriptor(Description="Embedded Property 2")]
		public string EmbeddedProperty2 { get; set; } = "Property 2";

        public AnotherEmbeddedClass AnotherEmbeddedClass { get; set; } = new AnotherEmbeddedClass();
	}

    public class AnotherEmbeddedClass
    {
        [SettingDescriptor(Description = "Another Embedded Property 1")]
        public string AnotherEmbeddedProperty1 { get; set; } = "Another Property 1";

        [SettingDescriptor(Description = "Another Embedded Property 2")]
        public string AnotherEmbeddedProperty2 { get; set; } = "Another Property 2";
    }


    public enum FIType
	{
		Unknown,
		CreditUnion,
		Bank
	}
	public class TestSettings : BaseSettings
	{
		public TestSettings()
		{
		}
		
		[SettingDescriptor(Description="Symconnect device type", IsRequired=true, Order=2)]
		public string DeviceType { get; set; } = "ALK";

		[SettingDescriptor(DisplayName="VCard Prefix", Description="Symconnect VCard prefix", IsRequired=true, Order=3)]
		public string VCardPrefix { get; set; } = "5050";

		[SettingDescriptor(Description="Symconnect Multiplexer Config Name", IsRequired=false, Order=4)]
		public string MultiplexerConfigName { get; set; } = "BAXTER";

		[SettingDescriptor(Name="DEVICENUMBER", DisplayName="Device Number", Description="Symconnect device number", IsRequired=true, Order=1)]
		public string DeviceNumber { get; set; } = "0";

		[SettingDescriptor(Order=5)]
		public int IntPropertyWithNoMetadata { get; set; } = 1234;

		[SettingDescriptor]
		public FIType FIType { get; set; } = FIType.CreditUnion;

		[SettingDescriptor]
		public EmbeddedClass EmbeddedClass { get; set; } = new EmbeddedClass();

        public string DummyProperty {get; set; }

        [SettingDescriptor(IsHtml = true)]
        public string HtmlProperty { get; set; }
	}
}
