namespace Platform.Library.ClientDataStructures.Impl
{
    using System;

    public class KeyAlreadyExistsException : ArgumentException
    {
        public KeyAlreadyExistsException(object key) : base($"Key {key} already exists in map.")
        {
        }
    }
}

