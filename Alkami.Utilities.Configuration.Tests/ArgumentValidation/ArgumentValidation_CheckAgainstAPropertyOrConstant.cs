using Alkami.Utilities.ArgumentValidation;
using Alkami.Utilities.ArgumentValidation.Exceptions;
using NUnit.Framework;

namespace Alkami.Utilities.Tests.ArgumentValidation
{
    [TestFixture]
    public class ArgumentValidation_CheckAgainstAPropertyOrConstant
    {
        public class CheckAgainstAProperty
        {
            [Test]
            public void CheckAgainstSetPropertyWorks()
            {
                var person = new Person
                {
                    Name = "Chancellor Palpatine"
                };

                Assert.DoesNotThrow(() => Validate.That(() => person.Name).IsNotNullOrEmpty());
            }

            [Test]
            public void CheckingAgainstUnsetPropertyWorks()
            {
                var person = new Person();
                Assert.Throws<ArgumentNullOrEmptyException>(() => Validate.That(() => person.Name).IsNotNullOrEmpty());
            }

            private class Person
            {
                public string Name { get; set; }
            }
        }

        public class CheckAgainstConstant
        {
            [Test]
            public void CheckAgainstConstantWorks()
            {
                const string name = "Lando";
                Assert.Throws<ArgumentShouldBeEqualException<string>>(() => Validate.That(() => name).IsEqualTo("Ackbar"));
            }

            [Test]
            public void CheckAgainstConstantHasNiceMessage()
            {
                const string name = "Biggs";
                Assert.AreEqual(
                    Assert.Throws<ArgumentShouldBeEqualException<string>>(() => Validate.That(() => name).IsEqualTo("Dooku")).Message,
                    "\"Biggs\" should be equal to \"Dooku\"\r\nParameter name: Constant \"Biggs\"");
            }
        }
    }
}
