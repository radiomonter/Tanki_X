namespace Platform.Library.ClientDataStructures.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal class SingletonEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
    {
        internal T value;
        private bool first;

        public SingletonEnumerator(T value)
        {
            this.value = value;
            this.first = true;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (!this.first)
            {
                return false;
            }
            this.first = false;
            return true;
        }

        public void Reset()
        {
            this.first = true;
        }

        object IEnumerator.Current =>
            this.Current;

        public T Current =>
            this.value;
    }
}

