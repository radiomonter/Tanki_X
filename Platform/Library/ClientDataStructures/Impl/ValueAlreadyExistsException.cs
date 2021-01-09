namespace Platform.Library.ClientDataStructures.Impl
{
    using System;

    public class ValueAlreadyExistsException : ArgumentException
    {
        public ValueAlreadyExistsException(object value) : base($"Value {value} already exists in map.")
        {
        }
    }
}

