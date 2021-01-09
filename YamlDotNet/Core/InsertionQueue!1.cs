namespace YamlDotNet.Core
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class InsertionQueue<T>
    {
        private readonly IList<T> items;

        public InsertionQueue()
        {
            this.items = new List<T>();
        }

        public T Dequeue()
        {
            if (this.Count == 0)
            {
                throw new InvalidOperationException("The queue is empty");
            }
            T local = this.items[0];
            this.items.RemoveAt(0);
            return local;
        }

        public void Enqueue(T item)
        {
            this.items.Add(item);
        }

        public void Insert(int index, T item)
        {
            this.items.Insert(index, item);
        }

        public int Count =>
            this.items.Count;
    }
}

