using System;
using NUnit.Framework;
using Alkami.Utilities.ArgumentValidation;
using Alkami.Utilities.ArgumentValidation.Exceptions;

namespace Alkami.Orb.Versions.Library.Tests
{
    [TestFixture]
    public class ArgumentValidationTestFixture
    {
        [TestCase(null)]
        [TestCase("")]
        public void Test_NullOrEmptyArgument_Throws_Exception(string testValue)
        {
            Assert.Throws<ArgumentNullOrEmptyException>(() => Validate.That(() => testValue).IsNotNullOrEmpty());
        }

        [TestCase(null)]
        public void Test_NullArgument_Throws_Exception(string testValue)
        {
            Assert.Throws<ArgumentNullException>(() => Validate.That(() => testValue).IsNotNull());
        }

        [TestCase("")]
        [TestCase("Some value")]
        public void Test_EmptyArgument_Does_Not_Throw_Exception(string testValue)
        {
            Validate.That(() => testValue).IsNotNull();
        }
    }
}

