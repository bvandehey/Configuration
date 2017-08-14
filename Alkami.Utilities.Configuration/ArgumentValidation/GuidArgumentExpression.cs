using System;
using System.Linq.Expressions;
using Alkami.Utilities.ArgumentValidation.Exceptions;

namespace Alkami.Utilities.ArgumentValidation
{
    public class GuidArgumentExpression : GenericArgumentExpression<Guid>
    {
        public GuidArgumentExpression(Expression<Func<Guid>> target) 
            : base(target)
        {
        }

        /// <summary>
        /// Check that the target value is not null or equal to Guid.Empty.
        /// Throws an InvariantShouldNotBeNullOrEmptyException if the target is null or empty.
        /// </summary>
        /// <param name="message">An optional message that overrides the automatically generated one if the check fails</param>
        public void IsNotNullOrEmpty(string message = null)
        {
            if (TargetValue == null || TargetValue == Guid.Empty)
            {
                throw new ArgumentNullOrEmptyException(message, FieldName);
            }
        }
    }
}