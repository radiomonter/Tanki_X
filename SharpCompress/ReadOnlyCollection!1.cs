namespace SharpCompress
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    internal class ReadOnlyCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private ICollection<T> collection;

        public ReadOnlyCollection(ICollection<T> collection)
        {
            this.collection = collection;
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item) => 
            this.collection.Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.collection.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator() => 
            this.collection.GetEnumerator();

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public int Count =>
            this.collection.Count;

        public bool IsReadOnly =>
            true;
    }
}

