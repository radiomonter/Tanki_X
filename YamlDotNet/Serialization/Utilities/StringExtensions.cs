namespace YamlDotNet.Serialization.Utilities
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Text.RegularExpressions;

    internal static class StringExtensions
    {
        [CompilerGenerated]
        private static MatchEvaluator <>f__am$cache0;
        [CompilerGenerated]
        private static Func<char, char> <>f__mg$cache0;
        [CompilerGenerated]
        private static Func<char, char> <>f__mg$cache1;

        public static string FromCamelCase(this string str, string separator)
        {
            <FromCamelCase>c__AnonStorey0 storey = new <FromCamelCase>c__AnonStorey0 {
                separator = separator
            };
            str = char.ToLower(str[0]) + str.Substring(1);
            str = Regex.Replace(str.ToCamelCase(), "(?<char>[A-Z])", new MatchEvaluator(storey.<>m__0));
            return str;
        }

        public static string ToCamelCase(this string str)
        {
            if (<>f__mg$cache0 == null)
            {
                <>f__mg$cache0 = new Func<char, char>(char.ToLowerInvariant);
            }
            return ToCamelOrPascalCase(str, <>f__mg$cache0);
        }

        private static string ToCamelOrPascalCase(string str, Func<char, char> firstLetterTransform)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = match => match.Groups["char"].Value.ToUpperInvariant();
            }
            string str2 = Regex.Replace(str, @"([_\-])(?<char>[a-z])", <>f__am$cache0, RegexOptions.IgnoreCase);
            return (firstLetterTransform(str2[0]) + str2.Substring(1));
        }

        public static string ToPascalCase(this string str)
        {
            if (<>f__mg$cache1 == null)
            {
                <>f__mg$cache1 = new Func<char, char>(char.ToUpperInvariant);
            }
            return ToCamelOrPascalCase(str, <>f__mg$cache1);
        }

        [CompilerGenerated]
        private sealed class <FromCamelCase>c__AnonStorey0
        {
            internal string separator;

            internal string <>m__0(Match match) => 
                this.separator + match.Groups["char"].Value.ToLowerInvariant();
        }
    }
}

