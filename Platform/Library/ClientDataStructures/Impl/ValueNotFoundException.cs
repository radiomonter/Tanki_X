namespace Platform.Library.ClientDataStructures.Impl
{
    using System;
    using System.Collections.Generic;

    public class ValueNotFoundException : KeyNotFoundException
    {
        public ValueNotFoundException(object value) : base($"Value {value} not found in map.")
        {
        }
    }
}

