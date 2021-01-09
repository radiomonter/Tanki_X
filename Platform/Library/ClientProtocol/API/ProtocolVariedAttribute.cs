namespace Platform.Library.ClientProtocol.API
{
    using System;

    [AttributeUsage(AttributeTargets.Property)]
    public class ProtocolVariedAttribute : Attribute
    {
        public readonly bool value;

        public ProtocolVariedAttribute()
        {
            this.value = true;
        }

        public ProtocolVariedAttribute(bool value)
        {
            this.value = value;
        }
    }
}

