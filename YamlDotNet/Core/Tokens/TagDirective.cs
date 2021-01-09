﻿namespace YamlDotNet.Core.Tokens
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using YamlDotNet.Core;

    [Serializable]
    public class TagDirective : Token
    {
        private readonly string handle;
        private readonly string prefix;
        private static readonly Regex tagHandleValidator = new Regex(@"^!([0-9A-Za-z_\-]*!)?$", RegexOptions.Compiled);

        public TagDirective(string handle, string prefix) : this(handle, prefix, Mark.Empty, Mark.Empty)
        {
        }

        public TagDirective(string handle, string prefix, Mark start, Mark end) : base(start, end)
        {
            if (string.IsNullOrEmpty(handle))
            {
                throw new ArgumentNullException("handle", "Tag handle must not be empty.");
            }
            if (!tagHandleValidator.IsMatch(handle))
            {
                throw new ArgumentException("Tag handle must start and end with '!' and contain alphanumerical characters only.", "handle");
            }
            this.handle = handle;
            if (string.IsNullOrEmpty(prefix))
            {
                throw new ArgumentNullException("prefix", "Tag prefix must not be empty.");
            }
            this.prefix = prefix;
        }

        public override bool Equals(object obj)
        {
            TagDirective directive = obj as TagDirective;
            return (((directive != null) && this.handle.Equals(directive.handle)) && this.prefix.Equals(directive.prefix));
        }

        public override int GetHashCode() => 
            this.handle.GetHashCode() ^ this.prefix.GetHashCode();

        public override string ToString()
        {
            object[] args = new object[] { this.handle, this.prefix };
            return string.Format(CultureInfo.InvariantCulture, "{0} => {1}", args);
        }

        public string Handle =>
            this.handle;

        public string Prefix =>
            this.prefix;
    }
}

