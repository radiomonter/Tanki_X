namespace YamlDotNet.Core
{
    using System;
    using YamlDotNet.Core.Tokens;

    internal static class Constants
    {
        public static readonly TagDirective[] DefaultTagDirectives = new TagDirective[] { new TagDirective("!", "!"), new TagDirective("!!", "tag:yaml.org,2002:") };
        public const int MajorVersion = 1;
        public const int MinorVersion = 1;
        public const char HandleCharacter = '!';
        public const string DefaultHandle = "!";
    }
}

