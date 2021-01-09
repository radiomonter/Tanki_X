namespace Platform.Library.ClientDataStructures.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public class EmptyList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
    {
        public static IList<T> Instance;

        static EmptyList()
        {
            EmptyList<T>.Instance = new EmptyList<T>();
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
        }

        public bool Contains(T item) => 
            false;

        public void CopyTo(T[] array, int arrayIndex)
        {
        }

        public IEnumerator<T> GetEnumerator() => 
            EmptyEnumerator<T>.Instance;

        public override int GetHashCode() => 
            0;

        public int IndexOf(T item) => 
            -1;

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item) => 
            false;

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            this.GetEnumerator();

        public int Count =>
            0;

        public bool IsReadOnly =>
            true;

        public T this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

