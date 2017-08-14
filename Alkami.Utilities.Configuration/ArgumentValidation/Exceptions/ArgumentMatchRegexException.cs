using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace Alkami.Utilities.ArgumentValidation.Exceptions
{
    [Serializable]
    public class ArgumentMatchRegexException : ArgumentInvalidException
    {
        public ArgumentMatchRegexException()
        {
        }

        public ArgumentMatchRegexException(string message, string paramName, string received, Regex regex)
            : base(GetMessage(message, received, regex), paramName)
        {
            this.Regex = regex;
            this.Received = received;
        }

        protected ArgumentMatchRegexException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public Regex Regex { get; protected set; }

        public string Received { get; private set; }

        private static string GetAutoMessage(string received, Regex regex) => $"\"{received}\" should match regular expression \"{regex}\"";

        private static string GetMessage(string message, string received, Regex regex) => string.IsNullOrEmpty(message)
            ? GetAutoMessage(received, regex)
            : message;
    }
}