namespace Platform.Library.ClientDataStructures.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;

    public class SingletonList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private T value;
        private IEnumerator<T> enumerator;

        public SingletonList()
        {
        }

        public SingletonList(T value)
        {
            this.value = value;
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
            item.Equals(this.value);

        public void CopyTo(T[] array, int arrayIndex)
        {
            array[arrayIndex] = this.value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (this.enumerator == null)
            {
                this.enumerator = new SingletonEnumerator<T>(this.value);
            }
            else
            {
                ((SingletonEnumerator<T>) this.enumerator).value = this.value;
                this.enumerator.Reset();
            }
            return this.enumerator;
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public SingletonList<T> Init(T value)
        {
            this.value = value;
            return (SingletonList<T>) this;
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            this.GetEnumerator();

        public int Count =>
            1;

        public bool IsReadOnly =>
            true;

        public T this[int index]
        {
            get
            {
                if (index != 0)
                {
                    throw new NotImplementedException();
                }
                return this.value;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}

