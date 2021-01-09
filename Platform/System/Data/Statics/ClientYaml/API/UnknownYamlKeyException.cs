namespace Platform.System.Data.Statics.ClientYaml.API
{
    using System;

    public class UnknownYamlKeyException : Exception
    {
        public UnknownYamlKeyException(string key) : base(key)
        {
        }
    }
}

