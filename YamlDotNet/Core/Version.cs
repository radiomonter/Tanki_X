namespace YamlDotNet.Core
{
    using System;
    using System.Runtime.CompilerServices;

    [Serializable]
    public class Version
    {
        public Version(int major, int minor)
        {
            this.Major = major;
            this.Minor = minor;
        }

        public override bool Equals(object obj)
        {
            Version version = obj as Version;
            return (((version != null) && (this.Major == version.Major)) && (this.Minor == version.Minor));
        }

        public override int GetHashCode() => 
            this.Major.GetHashCode() ^ this.Minor.GetHashCode();

        public int Major { get; private set; }

        public int Minor { get; private set; }
    }
}

