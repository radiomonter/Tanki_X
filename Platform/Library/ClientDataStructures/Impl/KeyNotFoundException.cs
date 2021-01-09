namespace Platform.Library.ClientDataStructures.Impl
{
    using System;
    using System.Collections.Generic;

    public class KeyNotFoundException : KeyNotFoundException
    {
        public KeyNotFoundException(object key) : base($"Key {key} not found in map.")
        {
        }
    }
}

