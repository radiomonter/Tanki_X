namespace Platform.Library.ClientDataStructures.API
{
    using System;

    public class TypeAlreadyRegisteredException : Exception
    {
        public TypeAlreadyRegisteredException(Type type) : base("Class=" + type)
        {
        }
    }
}

