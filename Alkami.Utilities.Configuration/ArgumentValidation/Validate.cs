using System;
using System.Linq.Expressions;

namespace Alkami.Utilities.ArgumentValidation
{
    public static class Validate
    {
        public static GenericArgumentExpression<T> That<T>(Expression<Func<T>> target)
        {
            return new GenericArgumentExpression<T>(target);
        }

        /// <summary>
        /// Create an invariant check instance for a string field
        /// </summary>
        /// <param name="target">An expression whose body is a string field</param>
        /// <returns>An invariant check instance for a string</returns>
        public static StringArgumentExpression That(Expression<Func<string>> target)
        {
            return new StringArgumentExpression(target);
        }

        /// <summary>
        /// Create an invariant check instance for a Type:
        ///     Check.That(() => typeof(Foo))...
        /// </summary>
        /// <param name="target">An expression that returns a Type</param>
        /// <returns>A check invariant instance for a Type</returns>
        public static TypeArgumentExpression That(Expression<Func<Type>> target)
        {
            return new TypeArgumentExpression(target);
        }

        /// <summary>
        /// Create an invariant check instance for a boolean expression which executes
        /// immediately and checks for boolean true
        /// </summary>
        /// <param name="target">An expression that results in a boolean</param>
        /// <returns>A check invariant instance for a boolean</returns>
        public static BooleanArgumentExpression That(Expression<Func<bool>> target)
        {
            return new BooleanArgumentExpression(target);
        }

        /// <summary>
        /// Create an invariant check instance for a Guid field.
        /// </summary>
        /// <param name="target">An expression whose body is a Guid field</param>
        /// <returns>An invariant check instance for a Guid</returns>
        public static GuidArgumentExpression That(
            Expression<Func<Guid>> target)
        {
            return new GuidArgumentExpression(target);
        }

    }
}
