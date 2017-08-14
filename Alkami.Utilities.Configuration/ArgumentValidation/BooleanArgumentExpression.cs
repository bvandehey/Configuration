using System;
using System.Linq.Expressions;
using Alkami.Utilities.ArgumentValidation.Exceptions;

namespace Alkami.Utilities.ArgumentValidation
{
    public class BooleanArgumentExpression : GenericArgumentExpression<bool>
    {
        public BooleanArgumentExpression(Expression<Func<bool>> target)
            : base(target)
        {
        }

        /// <summary>
        /// Check that the target value is true.
        /// Throws an ArgumentShouldBeTrueException if the target is false.
        /// </summary>
        /// <param name="message">An optional message that overrides the automatically generated one if the check fails</param>
        public void IsTrue(string message = null)
        {
            if (!TargetValue)
            {
                throw new ArgumentShouldBeTrueException(Target, message);
            }
        }

        /// <summary>
        /// Check that the target value is false.
        /// Throws an ArgumentShouldBeFalseException if the target is true.
        /// </summary>
        /// <param name="message">An optional message that overrides the automatically generated one if the check fails</param>
        public void IsFalse(string message = null)
        {
            if (TargetValue)
            {
                throw new ArgumentShouldBeFalseException(Target, message);
            }
        }
    }
}