using System;
using System.Runtime.Serialization;

namespace Alkami.Utilities.ArgumentValidation.Exceptions
{
    [Serializable]
    public class ArgumentShouldBeEqualException<T> : ArgumentInvalidException
    {
        public ArgumentShouldBeEqualException()
        {
        }

        public ArgumentShouldBeEqualException(string message, string paramName, T receivedValue, T expectedValue)
            : base(GetMessage(message, paramName, receivedValue, expectedValue), paramName)
        {
            this.ReceivedValue = receivedValue;
            this.ExpectedValue = expectedValue;
        }

        protected ArgumentShouldBeEqualException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public T ReceivedValue { get; private set; }

        public T ExpectedValue { get; private set; }

        private static string GetAutoMessage(T receivedValue, T expectedValue) 
            => $"\"{expectedValue}\" should be equal to \"{receivedValue}\"";

        private static string GetMessage(string message, string paramName, T receivedValue, T expectedValue) 
            => string.IsNullOrEmpty(message)
            ? GetAutoMessage(receivedValue, expectedValue)
            : message;
    }
}