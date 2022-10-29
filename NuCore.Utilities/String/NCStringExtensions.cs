using System;
using System.IO;
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

        public static byte[] ToByteArrayWithLength(this string input)
        {
            if (input.Length > 255) _ = new NCException("String must be max 255 Chars for NCStringExtensions::ToByteArrayWithLength", 184,
                "input parameter to NCStringExtensions::ToByteArrayWithLength had a Length parameter larger than 255", NCExceptionSeverity.FatalError);
            byte[] finalArray = new byte[input.Length + 1];
            finalArray[0] = Convert.ToByte(input.Length);
            Buffer.BlockCopy(input.ToArray(), 1, finalArray, 1, input.Length);
            return finalArray;
        }
    }
}
