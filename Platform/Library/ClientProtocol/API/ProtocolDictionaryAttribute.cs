namespace Platform.Library.ClientProtocol.API
{
    using System;
    using System.Runtime.CompilerServices;

    [AttributeUsage(AttributeTargets.Property)]
    public class ProtocolDictionaryAttribute : Attribute
    {
        public ProtocolDictionaryAttribute(bool optionalKey, bool variedKey, bool optionalValue, bool variedValue)
        {
            this.OptionalKey = optionalKey;
            this.VariedKey = variedKey;
            this.OptionalValue = optionalValue;
            this.VariedValue = variedValue;
        }

        public bool OptionalKey { get; private set; }

        public bool VariedKey { get; private set; }

        public bool OptionalValue { get; private set; }

        public bool VariedValue { get; private set; }
    }
}

