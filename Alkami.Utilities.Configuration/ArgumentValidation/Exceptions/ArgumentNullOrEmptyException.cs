using System;
using System.Runtime.Serialization;

namespace Alkami.Utilities.ArgumentValidation.Exceptions
{
    [Serializable]
    public class ArgumentNullOrEmptyException : ArgumentInvalidException
    {
        public ArgumentNullOrEmptyException()
        {
        }

        public ArgumentNullOrEmptyException(string message, string paramName)
            : base(GetMessage(message, paramName), paramName)
        {
        }

        protected ArgumentNullOrEmptyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        private static string GetAutoMessage(string paramName) => $"{paramName} should not be null or empty";

        private static string GetMessage(string message, string paramName) => string.IsNullOrEmpty(message)
            ? GetAutoMessage(paramName)
            : message;
    }
}