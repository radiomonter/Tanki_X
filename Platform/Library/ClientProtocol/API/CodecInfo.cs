namespace Platform.Library.ClientProtocol.API
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct CodecInfo
    {
        private const string TO_STRING_FORMAT = "Type = {0}, Optional = {1}, Varied = {2}";
        public readonly Type type;
        public readonly bool optional;
        public readonly bool varied;
        public CodecInfo(Type type, bool optional, bool varied)
        {
            this.type = type;
            this.optional = optional;
            this.varied = varied;
        }

        public bool Equals(CodecInfo other) => 
            (Equals(this.type, other.type) && (this.optional == other.optional)) && (this.varied == other.varied);

        public override int GetHashCode() => 
            (((((0 * 0x18d) ^ ((this.type == null) ? 0 : this.type.GetHashCode())) * 0x18d) ^ this.optional.GetHashCode()) * 0x18d) ^ this.varied.GetHashCode();

        public override string ToString() => 
            $"Type = {this.type}, Optional = {this.optional}, Varied = {this.varied}";
    }
}

