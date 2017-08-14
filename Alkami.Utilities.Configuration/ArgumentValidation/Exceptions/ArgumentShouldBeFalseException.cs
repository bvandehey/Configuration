using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Alkami.Utilities.ArgumentValidation.Exceptions
{
    [Serializable]
    public class ArgumentShouldBeFalseException : ArgumentInvalidException
    {
        public ArgumentShouldBeFalseException()
        {
        }

        public ArgumentShouldBeFalseException(Expression<Func<bool>> target, string message)
            : base(GetMessage(message, target), null)
        {
            this.Target = target;
        }

        protected ArgumentShouldBeFalseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public Expression<Func<bool>> Target { get; protected set; }

        private static string GetAutoMessage(Expression<Func<bool>> target) => $"{Niceify(target.Body)} should be false";

        private static string GetMessage(string message, Expression<Func<bool>> target) => string.IsNullOrEmpty(message)
            ? GetAutoMessage(target)
            : message;
    }
}
