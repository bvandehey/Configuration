using System;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Alkami.Utilities.ArgumentValidation.Exceptions
{
    [Serializable]
    public class ArgumentNeedsPublicConstructorWithParametersException : ArgumentInvalidException
    {
        public ArgumentNeedsPublicConstructorWithParametersException()
        {
        }

        public ArgumentNeedsPublicConstructorWithParametersException(Expression<Func<Type>> target, Type[] types, string message)
            : base(GetMessage(message, target, types), null)
        {
            this.Target = target;
            this.Types = types;
        }

        protected ArgumentNeedsPublicConstructorWithParametersException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public Expression<Func<Type>> Target { get; }

        public Type[] Types { get; }

        private static string GetAutoMessage(Expression<Func<Type>> target, Type[] types) => $"{target.Body} should have a public constructor accepting ({string.Join(", ", types.Select(x => x.Name))})";

        private static string GetMessage(string message, Expression<Func<Type>> target, Type[] types) => string.IsNullOrEmpty(message)
            ? GetAutoMessage(target, types)
            : message;
    }
}
