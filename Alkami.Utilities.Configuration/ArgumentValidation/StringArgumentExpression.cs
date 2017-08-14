using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using Alkami.Utilities.ArgumentValidation.Exceptions;

namespace Alkami.Utilities.ArgumentValidation
{
    public class StringArgumentExpression : GenericArgumentExpression<string>
    {
        private readonly Regex emailRegex = new Regex(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");

        public StringArgumentExpression(Expression<Func<string>> target) 
            : base(target)
        {
        }

        /// <summary>
        /// Check that the target value is not null or empty.
        /// Throws an InvariantShouldNotBeNullOrEmptyException if the target is null or empty.
        /// </summary>
        /// <param name="message">An optional message that overrides the automatically generated one if the check fails</param>
        public void IsNotNullOrEmpty(string message = null)
        {
            if (string.IsNullOrEmpty(TargetValue))
            {
                throw new ArgumentNullOrEmptyException(message, FieldName);
            }
        }

        /// <summary>
        /// Check that the target value matches the regex.
        /// Throws an InvariantShouldMatchRegexException if the target does not match the regex.
        /// </summary>
        /// <param name="regex">The regex to test against the target value</param>
        /// <param name="message">An optional message that overrides the automatically generated one if the check fails</param>
        public void IsMatchForRegex(string regex, string message = null)
        {
            IsMatchForRegex(new Regex(regex), message);
        }

        /// <summary>
        /// Check that the target value matches the regex.
        /// Throws an InvariantShouldMatchRegexException if the target does not match the regex.
        /// </summary>
        /// <param name="regex">The regex to test against the target value</param>
        /// <param name="message">An optional message that overrides the automatically generated one if the check fails</param>
        public void IsMatchForRegex(Regex regex, string message = null)
        {
            if (!regex.IsMatch(TargetValue))
            {
                throw new ArgumentMatchRegexException(message, FieldName, TargetValue, regex);
            }
        }

        /// <summary>
        /// Check that the target value is a valid email address, according to the .NET regex found at http://emailregex.com/.
        /// Throws an InvariantShouldBeValidEmailAddressException if the target is not a valid email address.
        /// </summary>
        /// <param name="message"></param>
        public void IsValidEmailAddress(string message = null)
        {
            if (!emailRegex.IsMatch(TargetValue))
            {
                throw new ArgumentInvalidEmailAddressException(message, FieldName, TargetValue);
            }
        }
    }
}