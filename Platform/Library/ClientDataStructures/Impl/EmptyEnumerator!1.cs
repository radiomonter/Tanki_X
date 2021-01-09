namespace Platform.Library.ClientDataStructures.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class EmptyEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
    {
        public static IEnumerator<T> Instance;

        static EmptyEnumerator()
        {
            EmptyEnumerator<T>.Instance = new EmptyEnumerator<T>();
        }

        public void Dispose()
        {
        }

        public bool MoveNext() => 
            false;

        public void Reset()
        {
        }

        object IEnumerator.Current =>
            this.Current;

        public T Current
        {
            get
            {
                throw new NotSupportedException();
            }
        }
    }
}

