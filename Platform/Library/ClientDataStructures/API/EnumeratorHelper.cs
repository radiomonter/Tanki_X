namespace Platform.Library.ClientDataStructures.API
{
    using System;

    public static class EnumeratorHelper
    {
        public static void CheckCurrentState(EnumeratorState state)
        {
            if (state == EnumeratorState.Before)
            {
                throw new EnumeratorInvalidStateException("Enumerator in initial satte. Use MoveNext() for moving to 1st element.");
            }
            if (state == EnumeratorState.After)
            {
                throw new EnumeratorInvalidStateException("End of enumerator. Use Reset().");
            }
        }

        public static void CheckVersion(int version, int actualCollectionVersion)
        {
            if (version != actualCollectionVersion)
            {
                throw new CollectionChangedException();
            }
        }

        public class CollectionChangedException : InvalidOperationException
        {
            public CollectionChangedException() : base("Collections changed while enumeration.")
            {
            }
        }

        public class EnumeratorInvalidStateException : InvalidOperationException
        {
            public EnumeratorInvalidStateException(string message) : base(message)
            {
            }
        }

        public enum EnumeratorState
        {
            Before,
            Current,
            After
        }
    }
}

