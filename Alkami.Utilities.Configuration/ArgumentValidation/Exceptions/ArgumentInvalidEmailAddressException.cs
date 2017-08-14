using System;
using System.Runtime.Serialization;

namespace Alkami.Utilities.ArgumentValidation.Exceptions
{
    [Serializable]
    public class ArgumentInvalidEmailAddressException : ArgumentInvalidException
    {
        public ArgumentInvalidEmailAddressException()
        {
        }

        public ArgumentInvalidEmailAddressException(string message, string paramName, string targetValue)
            : base(GetMessage(message, targetValue), paramName)
        {
            this.TargetValue = targetValue;
        }

        protected ArgumentInvalidEmailAddressException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        public string TargetValue { get; private set; }

        private static string GetAutoMessage(string targetValue) => $"\"{targetValue}\" should be a valid email address";

        private static string GetMessage(string message, string targetValue) => string.IsNullOrEmpty(message)
            ? GetAutoMessage(targetValue)
            : message;
    }
}