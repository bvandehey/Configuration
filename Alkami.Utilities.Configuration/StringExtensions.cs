using System;
using System.Text;

namespace Alkami.Utilities
{
    /// <summary>
    /// The StringExtensions class contains the ToDisplayName string extension which
    /// converts a string value to a more display friendly name.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts the string value to a more display friendly name
        /// </summary>
        /// <param name="value">The value to convert into a display format.</param>
        /// <returns>Returns a string that is the friendly version of the value.</returns>
        /// <exception cref="ArgumentNullException">Thrown when either the value parameter is null</exception>
        public static string ToDisplayName(this string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var sb = new StringBuilder(value.Length + 10);
            var first = true;
            var lastChar = ' ';
            for(int i = 0; i < value.Length; i++)
            {
                var last = (i == value.Length - 1);
                var currentChar = value[i];
                var nextChar = ' ';
                nextChar = !last ? value[i + 1] : ' ';
                if(char.IsLower(currentChar))
                {
                    if (first)
                        currentChar = char.ToUpper(currentChar);
                }

                if (!first)
                {
                    if(char.IsUpper(currentChar) && char.IsLower(lastChar))
                        sb.Append(' ');
                    else if(char.IsUpper(lastChar) && char.IsUpper(currentChar) && char.IsLower(nextChar))
                        sb.Append(' ');
                    else if(!char.IsDigit(lastChar) && char.IsDigit(currentChar))
                        sb.Append(' ');
                }

                sb.Append(currentChar);

                if(char.IsDigit(currentChar) && !char.IsDigit(nextChar) && !last)
                    sb.Append(' ');

                first = false;
                lastChar = currentChar;
            }
            return sb.ToString();
        }
    }
}