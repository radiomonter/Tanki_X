namespace MIConvexHull
{
    using System;
    using System.Reflection;

    internal class SimpleList<T>
    {
        private T[] items;
        private int capacity;
        public int Count;

        public void Add(T item)
        {
            int num;
            if ((this.Count + 1) > this.capacity)
            {
                this.EnsureCapacity();
            }
            this.Count = (num = this.Count) + 1;
            this.items[num] = item;
        }

        public void Clear()
        {
            this.Count = 0;
        }

        private void EnsureCapacity()
        {
            if (this.capacity == 0)
            {
                this.capacity = 0x20;
                this.items = new T[0x20];
            }
            else
            {
                T[] destinationArray = new T[this.capacity * 2];
                Array.Copy(this.items, destinationArray, this.capacity);
                this.capacity = 2 * this.capacity;
                this.items = destinationArray;
            }
        }

        public T Pop()
        {
            int num;
            this.Count = num = this.Count - 1;
            return this.items[num];
        }

        public void Push(T item)
        {
            int num;
            if ((this.Count + 1) > this.capacity)
            {
                this.EnsureCapacity();
            }
            this.Count = (num = this.Count) + 1;
            this.items[num] = item;
        }

        public T this[int i]
        {
            get => 
                this.items[i];
            set => 
                this.items[i] = value;
        }
    }
}

