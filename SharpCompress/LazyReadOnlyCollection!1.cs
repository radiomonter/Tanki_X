namespace SharpCompress
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    internal class LazyReadOnlyCollection<T> : ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private readonly List<T> backing;
        private readonly IEnumerator<T> source;
        private bool fullyLoaded;
        [CompilerGenerated]
        private static Action<T> <>f__am$cache0;

        public LazyReadOnlyCollection(IEnumerable<T> source)
        {
            this.backing = new List<T>();
            this.source = source.GetEnumerator();
        }

        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            this.EnsureFullyLoaded();
            return this.backing.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.EnsureFullyLoaded();
            this.backing.CopyTo(array, arrayIndex);
        }

        internal void EnsureFullyLoaded()
        {
            if (!this.fullyLoaded)
            {
                if (LazyReadOnlyCollection<T>.<>f__am$cache0 == null)
                {
                    LazyReadOnlyCollection<T>.<>f__am$cache0 = delegate (T x) {
                    };
                }
                this.ForEach<T>(LazyReadOnlyCollection<T>.<>f__am$cache0);
                this.fullyLoaded = true;
            }
        }

        public IEnumerator<T> GetEnumerator() => 
            new LazyLoader<T>((LazyReadOnlyCollection<T>) this);

        internal IEnumerable<T> GetLoaded() => 
            this.backing;

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            this.GetEnumerator();

        public int Count
        {
            get
            {
                this.EnsureFullyLoaded();
                return this.backing.Count;
            }
        }

        public bool IsReadOnly =>
            true;

        private class LazyLoader : IEnumerator<T>, IDisposable, IEnumerator
        {
            private readonly LazyReadOnlyCollection<T> lazyReadOnlyCollection;
            private bool disposed;
            private int index;

            internal LazyLoader(LazyReadOnlyCollection<T> lazyReadOnlyCollection)
            {
                this.index = -1;
                this.lazyReadOnlyCollection = lazyReadOnlyCollection;
            }

            public void Dispose()
            {
                this.disposed ??= true;
            }

            public bool MoveNext()
            {
                if ((this.index + 1) < this.lazyReadOnlyCollection.backing.Count)
                {
                    this.index++;
                    return true;
                }
                if (this.lazyReadOnlyCollection.fullyLoaded || !this.lazyReadOnlyCollection.source.MoveNext())
                {
                    this.lazyReadOnlyCollection.fullyLoaded = true;
                    return false;
                }
                this.lazyReadOnlyCollection.backing.Add(this.lazyReadOnlyCollection.source.Current);
                this.index++;
                return true;
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }

            object IEnumerator.Current =>
                this.Current;

            public T Current =>
                this.lazyReadOnlyCollection.backing[this.index];
        }
    }
}

