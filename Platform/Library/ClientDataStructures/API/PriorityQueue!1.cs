namespace Platform.Library.ClientDataStructures.API
{
    using Platform.Library.ClientDataStructures.Impl;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class PriorityQueue<T> : IPriorityQueue<T>, ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private IComparer<T> comparer;
        private List<T> items;
        private int version;

        public PriorityQueue() : this(Comparers.GetComparer<T>())
        {
        }

        public PriorityQueue(IComparer<T> comparer)
        {
            this.comparer = comparer;
            this.Init();
        }

        public PriorityQueue(Comparison<T> comparison) : this(Comparers.GetComparer<T>(comparison))
        {
        }

        public void Add(T item)
        {
            this.Enqueue(item);
        }

        private void BubbleDown(int i)
        {
            int num = i * 2;
            int num2 = (i * 2) + 1;
            int num3 = i;
            if ((num < this.items.Count) && (this.comparer.Compare(this.items[num], this.items[i]) < 0))
            {
                num3 = num;
            }
            if ((num2 < this.items.Count) && (this.comparer.Compare(this.items[num2], this.items[num3]) < 0))
            {
                num3 = num2;
            }
            if (num3 != i)
            {
                T local = this.items[num3];
                this.items[num3] = this.items[i];
                this.items[i] = local;
                this.BubbleDown(num3);
            }
        }

        private void BubbleUp(int i)
        {
            int num = i;
            for (int j = i / 2; (j != 0) && (this.comparer.Compare(this.items[j], this.items[num]) > 0); j = num / 2)
            {
                this.items[j] = this.items[num];
                this.items[num] = this.items[j];
                num = j;
            }
        }

        public void Clear()
        {
            this.Init();
            this.version++;
        }

        public bool Contains(T item) => 
            this.items.Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            for (int i = 1; i < this.items.Count; i++)
            {
                array[(arrayIndex + i) - 1] = this.items[i];
            }
        }

        public T Dequeue()
        {
            if (this.Count == 0)
            {
                throw new QueueIsEmptyException();
            }
            T local = this.items[1];
            this.items[1] = this.items[this.items.Count - 1];
            this.items.RemoveAt(this.items.Count - 1);
            this.BubbleDown(1);
            this.version++;
            return local;
        }

        public void Enqueue(T item)
        {
            this.items.Add(item);
            this.BubbleUp(this.items.Count - 1);
            this.version++;
        }

        public IEnumerator<T> GetEnumerator() => 
            new Enumerator<T>((PriorityQueue<T>) this);

        private void Init()
        {
            this.items = new List<T>();
            T item = default(T);
            this.items.Add(item);
        }

        public T Peek()
        {
            if (this.Count == 0)
            {
                throw new QueueIsEmptyException();
            }
            return this.items[1];
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            this.GetEnumerator();

        public int Count =>
            this.items.Count - 1;

        public bool IsReadOnly =>
            false;

        [StructLayout(LayoutKind.Sequential)]
        private struct Enumerator : IEnumerator<T>, IDisposable, IEnumerator
        {
            private PriorityQueue<T> pq;
            private int index;
            private int ver;
            private EnumeratorHelper.EnumeratorState state;
            internal Enumerator(PriorityQueue<T> pq)
            {
                this.pq = pq;
                this.ver = pq.version;
                this.index = -1;
                this.state = EnumeratorHelper.EnumeratorState.Before;
            }

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                EnumeratorHelper.CheckVersion(this.ver, this.pq.version);
                EnumeratorHelper.EnumeratorState state = this.state;
                if (state == EnumeratorHelper.EnumeratorState.Before)
                {
                    if (this.pq.Count == 0)
                    {
                        this.state = EnumeratorHelper.EnumeratorState.After;
                        return false;
                    }
                    this.state = EnumeratorHelper.EnumeratorState.Current;
                    this.index = 1;
                    return true;
                }
                if (state != EnumeratorHelper.EnumeratorState.After)
                {
                    if (++this.index <= this.pq.Count)
                    {
                        return true;
                    }
                    this.state = EnumeratorHelper.EnumeratorState.After;
                }
                return false;
            }

            public T Current
            {
                get
                {
                    EnumeratorHelper.CheckCurrentState(this.state);
                    return this.pq.items[this.index];
                }
            }
            void IEnumerator.Reset()
            {
                EnumeratorHelper.CheckVersion(this.ver, this.pq.version);
                this.state = EnumeratorHelper.EnumeratorState.Before;
            }

            object IEnumerator.Current =>
                this.Current;
        }
    }
}

