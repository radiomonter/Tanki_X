namespace YamlDotNet.Core
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public class FakeList<T>
    {
        private readonly IEnumerator<T> collection;
        private int currentIndex;

        public FakeList(IEnumerable<T> collection) : this(collection.GetEnumerator())
        {
        }

        public FakeList(IEnumerator<T> collection)
        {
            this.currentIndex = -1;
            this.collection = collection;
        }

        public T this[int index]
        {
            get
            {
                if (index < this.currentIndex)
                {
                    this.collection.Reset();
                    this.currentIndex = -1;
                }
                while (this.currentIndex < index)
                {
                    if (!this.collection.MoveNext())
                    {
                        throw new ArgumentOutOfRangeException("index");
                    }
                    this.currentIndex++;
                }
                return this.collection.Current;
            }
        }
    }
}

