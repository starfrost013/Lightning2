using System.Linq;
using System.Text.RegularExpressions;

namespace NuCore.Utilities
{
    /// <summary>
    /// StringExtensions
    /// 
    /// August 3, 2022
    /// 
    /// String extension methods
    /// </summary>
    public static class NCStringExtensions
    {
        /// <summary>
        /// Exact replace method; only matches the exact string.
        /// </summary>
        /// <param name="input">The input string to replace with.</param>
        /// <param name="find">The pattern to find.</param>
        /// <param name="replace">The pattern to replace <paramref name="find"/> with.</param>
        /// <param name="ignoreCase">Determines if the compare will be case-insensitive or not.</param>
        /// <returns></returns>
        public static string ReplaceExact(this string input, string find, string replace, bool ignoreCase = false)
        {
            if (ignoreCase) input = input.ToLower();
            string textToFind = string.Format(@"\b{0}\b", find);
            return Regex.Replace(input, textToFind, replace, ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);
        }
    }
}
