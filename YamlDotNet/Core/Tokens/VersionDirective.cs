namespace YamlDotNet.Core.Tokens
{
    using System;
    using YamlDotNet.Core;

    [Serializable]
    public class VersionDirective : Token
    {
        private readonly YamlDotNet.Core.Version version;

        public VersionDirective(YamlDotNet.Core.Version version) : this(version, Mark.Empty, Mark.Empty)
        {
        }

        public VersionDirective(YamlDotNet.Core.Version version, Mark start, Mark end) : base(start, end)
        {
            this.version = version;
        }

        public override bool Equals(object obj)
        {
            VersionDirective directive = obj as VersionDirective;
            return ((directive != null) && this.version.Equals(directive.version));
        }

        public override int GetHashCode() => 
            this.version.GetHashCode();

        public YamlDotNet.Core.Version Version =>
            this.version;
    }
}

