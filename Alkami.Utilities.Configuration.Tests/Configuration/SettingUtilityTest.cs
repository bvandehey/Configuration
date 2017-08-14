using NUnit.Framework;

namespace Alkami.Utilities.Tests.Configuration
{
    public class SettingUtilityTest
    {
        [TestCase("Name", "Name")]
        [TestCase("DisplayName", "Display Name")]
        [TestCase("lowercase", "Lowercase")]
        [TestCase("MyName", "My Name")]
        [TestCase("HTML", "HTML")]
        [TestCase("PDFFilename", "PDF Filename")]
        [TestCase("AString", "A String")]
        [TestCase("SimpleXMLParser", "Simple XML Parser")]
        [TestCase("GL1Version", "GL 1 Version")]
        [TestCase("GL12Version", "GL 12 Version")]
        [TestCase("GL123Version", "GL 123 Version")]
        [TestCase("99Bottles", "99 Bottles")]
        [TestCase("May5", "May 5")]
        [TestCase("BFG9000", "BFG 9000")]
        public void Test_Convert_Name_To_DisplayName(string inputName, string expectedResult)
        {
            Assert.AreEqual(expectedResult, inputName.ToDisplayName());
        }
    }
}