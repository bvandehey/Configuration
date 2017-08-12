using Xunit;

namespace Alkami.Framework.Settings.Tests
{
    public class SettingUtilityTest
    {
        [Theory]
        [InlineData("Name", "Name")]
        [InlineData("DisplayName", "Display Name")]
        [InlineData("lowercase", "Lowercase")]
        [InlineData("MyName", "My Name")]
        [InlineData("HTML", "HTML")]
        [InlineData("PDFFilename", "PDF Filename")]
        [InlineData("AString", "A String")]
        [InlineData("SimpleXMLParser", "Simple XML Parser")]
        [InlineData("GL1Version", "GL 1 Version")]
        [InlineData("GL12Version", "GL 12 Version")]
        [InlineData("GL123Version", "GL 123 Version")]
        [InlineData("99Bottles", "99 Bottles")]
        [InlineData("May5", "May 5")]
        [InlineData("BFG9000", "BFG 9000")]
        public void Test_Convert_Name_To_DisplayName(string inputName, string expectedResult)
        {
            Assert.Equal(expectedResult, inputName.ToDisplayName());
        }
    }
}