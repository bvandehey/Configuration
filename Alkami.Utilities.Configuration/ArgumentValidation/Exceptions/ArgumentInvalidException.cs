using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Alkami.Utilities.ArgumentValidation.Exceptions
{
    [Serializable]
    public class ArgumentInvalidException : ArgumentException
    {
        public ArgumentInvalidException()
        {
        }

        public ArgumentInvalidException(string message, string paramName)
            : base(GetMessage(message, paramName), paramName)
        {
        }

        protected ArgumentInvalidException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
        }

        private static string GetAutoMessage(string paramName) => $"{paramName} is invalid";

        private static string GetMessage(string message, string paramName) => string.IsNullOrEmpty(message)
            ? GetAutoMessage(paramName)
            : message;

        protected static string Niceify(Expression body)
        {
            var regex = new Regex("value\\(.+?\\)\\.");
            return regex.Replace(body.ToString(), "");
        }
    }
}