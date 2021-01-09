namespace Platform.Library.ClientDataStructures.Impl
{
    using System;

    public class TypeIsNotComparableException : InvalidOperationException
    {
        public TypeIsNotComparableException(Type type) : base($"Type {type} is not derived from IComparable.")
        {
        }
    }
}

