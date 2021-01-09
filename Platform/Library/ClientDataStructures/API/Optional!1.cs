namespace Platform.Library.ClientDataStructures.API
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential)]
    public struct Optional<T> where T: class
    {
        public static readonly Optional<T> EMPTY;
        private readonly T value;
        public Optional(T value)
        {
            this.value = value;
        }

        public T Get() => 
            this.value;

        public bool IsPresent() => 
            this.value != null;

        public override string ToString() => 
            "Optional[" + this.value + "]";

        public static Optional<T> nullableOf(T value) => 
            (value != null) ? Optional<T>.of(value) : Optional<T>.empty();

        public static Optional<T> empty() => 
            Optional<T>.EMPTY;

        public static Optional<T> of(T value) => 
            new Optional<T>(value);

        static Optional()
        {
            Optional<T> optional = new Optional<T>();
            Optional<T>.EMPTY = optional;
        }
    }
}

