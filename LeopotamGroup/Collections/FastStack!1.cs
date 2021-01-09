namespace LeopotamGroup.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class FastStack<T>
    {
        private const int InitCapacity = 8;
        private T[] _items;
        private int _capacity;
        private int _count;
        private readonly bool _isNullable;
        private readonly EqualityComparer<T> _comparer;
        private bool _useObjectCastComparer;

        public FastStack() : this(null)
        {
        }

        public FastStack(EqualityComparer<T> comparer) : this(8, comparer)
        {
        }

        public FastStack(int capacity, EqualityComparer<T> comparer = null)
        {
            Type nullableType = typeof(T);
            this._isNullable = !nullableType.IsValueType || !ReferenceEquals(Nullable.GetUnderlyingType(nullableType), null);
            this._capacity = (capacity <= 8) ? 8 : capacity;
            this._count = 0;
            this._comparer = comparer;
            this._items = new T[this._capacity];
        }

        public void Clear()
        {
            if (this._isNullable)
            {
                for (int i = this._count - 1; i >= 0; i--)
                {
                    this._items[i] = default(T);
                }
            }
            this._count = 0;
        }

        public bool Contains(T item)
        {
            int num;
            if (this._useObjectCastComparer && this._isNullable)
            {
                num = this._count - 1;
                while ((num >= 0) && (this._items[num] != item))
                {
                    num--;
                }
            }
            else if (this._comparer == null)
            {
                num = Array.IndexOf<T>(this._items, item, 0, this._count);
            }
            else
            {
                num = this._count - 1;
                while ((num >= 0) && !this._comparer.Equals(this._items[num], item))
                {
                    num--;
                }
            }
            return (num != -1);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(this._items, 0, array, arrayIndex, this._count);
        }

        public T Peek()
        {
            if (this._count == 0)
            {
                throw new IndexOutOfRangeException();
            }
            return this._items[this._count - 1];
        }

        public T Pop()
        {
            if (this._count == 0)
            {
                throw new IndexOutOfRangeException();
            }
            this._count--;
            T local = this._items[this._count];
            if (this._isNullable)
            {
                this._items[this._count] = default(T);
            }
            return local;
        }

        public void Push(T item)
        {
            if (this._count == this._capacity)
            {
                this._capacity = (this._capacity <= 0) ? 8 : (this._capacity << 1);
                T[] destinationArray = new T[this._capacity];
                Array.Copy(this._items, destinationArray, this._count);
                this._items = destinationArray;
            }
            this._items[this._count] = item;
            this._count++;
        }

        public T[] ToArray()
        {
            T[] destinationArray = new T[this._count];
            if (this._count > 0)
            {
                Array.Copy(this._items, destinationArray, this._count);
            }
            return destinationArray;
        }

        public void TrimExcess()
        {
            throw new NotSupportedException();
        }

        public void UseCastToObjectComparer(bool state)
        {
            this._useObjectCastComparer = state;
        }

        public int Count =>
            this._count;
    }
}

