namespace Platform.Library.ClientDataStructures.API
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class EmptyCollection<T> : ICollection<T>, ICollection, IEnumerable<T>, IEnumerable
    {
        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item) => 
            false;

        public void CopyTo(Array array, int index)
        {
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
        }

        public IEnumerator<T> GetEnumerator() => 
            new EmptyEnumerator<T>();

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            new EmptyEnumerator<T>();

        public object SyncRoot =>
            this;

        public bool IsSynchronized =>
            true;

        public int Count =>
            0;

        public bool IsReadOnly =>
            true;

        [StructLayout(LayoutKind.Sequential, Size=1)]
        private struct EmptyEnumerator : IEnumerator<T>, IDisposable, IEnumerator
        {
            public void Dispose()
            {
            }

            public bool MoveNext() => 
                false;

            public void Reset()
            {
            }

            public T Current
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
            object IEnumerator.Current
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}

