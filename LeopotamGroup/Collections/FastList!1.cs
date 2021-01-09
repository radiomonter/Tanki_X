namespace LeopotamGroup.Collections
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [Serializable]
    public class FastList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
    {
        private const int InitCapacity = 8;
        private readonly bool _isNullable;
        private T[] _items;
        private int _count;
        private int _capacity;
        private readonly EqualityComparer<T> _comparer;
        private bool _useObjectCastComparer;

        public FastList() : this(null)
        {
        }

        public FastList(EqualityComparer<T> comparer) : this(8, comparer)
        {
        }

        public FastList(int capacity, EqualityComparer<T> comparer = null)
        {
            Type nullableType = typeof(T);
            this._isNullable = !nullableType.IsValueType || !ReferenceEquals(Nullable.GetUnderlyingType(nullableType), null);
            this._capacity = (capacity <= 8) ? 8 : capacity;
            this._count = 0;
            this._comparer = comparer;
            this._items = new T[this._capacity];
        }

        public void Add(T item)
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

        public void AddRange(IEnumerable<T> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            ICollection<T> is2 = data as ICollection<T>;
            if (is2 != null)
            {
                int count = is2.Count;
                if (count > 0)
                {
                    this.Reserve(count, false, false);
                    is2.CopyTo(this._items, this._count);
                    this._count += count;
                }
            }
            else
            {
                using (IEnumerator<T> enumerator = data.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        this.Add(enumerator.Current);
                    }
                }
            }
        }

        public void AssignData(T[] data, int count)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            this._items = data;
            this._count = (count < 0) ? 0 : count;
            this._capacity = this._items.Length;
        }

        public void Clear()
        {
            this.Clear(false);
        }

        public void Clear(bool forceSetDefaultValues)
        {
            if (this._isNullable || forceSetDefaultValues)
            {
                for (int i = this._count - 1; i >= 0; i--)
                {
                    this._items[i] = default(T);
                }
            }
            this._count = 0;
        }

        public bool Contains(T item) => 
            this.IndexOf(item) != -1;

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(this._items, 0, array, arrayIndex, this._count);
        }

        public void FillWithEmpty(int amount, bool clearCollection = false, bool forceSetDefaultValues = true)
        {
            if (amount > 0)
            {
                if (clearCollection)
                {
                    this._count = 0;
                }
                this.Reserve(amount, clearCollection, forceSetDefaultValues);
                this._count += amount;
            }
        }

        public T[] GetData() => 
            this._items;

        public T[] GetData(out int count)
        {
            count = this._count;
            return this._items;
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotSupportedException();
        }

        public int IndexOf(T item)
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
            return num;
        }

        public void Insert(int index, T item)
        {
            if ((index < 0) || (index > this._count))
            {
                throw new ArgumentOutOfRangeException();
            }
            this.Reserve(1, false, false);
            Array.Copy(this._items, index, this._items, index + 1, this._count - index);
            this._items[index] = item;
            this._count++;
        }

        public bool Remove(T item)
        {
            int index = this.IndexOf(item);
            if (index == -1)
            {
                return false;
            }
            this.RemoveAt(index);
            return true;
        }

        public void RemoveAt(int id)
        {
            if ((id >= 0) && (id < this._count))
            {
                this._count--;
                Array.Copy(this._items, id + 1, this._items, id, this._count - id);
            }
        }

        public bool RemoveLast(bool forceSetDefaultValues = true)
        {
            if (this._count <= 0)
            {
                return false;
            }
            this._count--;
            if (forceSetDefaultValues)
            {
                this._items[this._count] = default(T);
            }
            return true;
        }

        public void Reserve(int amount, bool totalAmount = false, bool forceSetDefaultValues = true)
        {
            if (amount > 0)
            {
                int num2 = (!totalAmount ? this._count : 0) + amount;
                if (num2 > this._capacity)
                {
                    if (this._capacity <= 0)
                    {
                        this._capacity = 8;
                    }
                    while (true)
                    {
                        if (this._capacity >= num2)
                        {
                            T[] destinationArray = new T[this._capacity];
                            Array.Copy(this._items, destinationArray, this._count);
                            this._items = destinationArray;
                            break;
                        }
                        this._capacity = this._capacity << 1;
                    }
                }
                if (forceSetDefaultValues)
                {
                    for (int i = this._count; i < num2; i++)
                    {
                        this._items[i] = default(T);
                    }
                }
            }
        }

        public void Reverse()
        {
            if (this._count > 0)
            {
                int index = 0;
                int num2 = this._count >> 1;
                while (index < num2)
                {
                    T local = this._items[index];
                    this._items[index] = this._items[(this._count - index) - 1];
                    this._items[(this._count - index) - 1] = local;
                    index++;
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotSupportedException();
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

        public void UseCastToObjectComparer(bool state)
        {
            this._useObjectCastComparer = state;
        }

        public int Count =>
            this._count;

        public int Capacity =>
            this._capacity;

        public T this[int index]
        {
            get
            {
                if (index >= this._count)
                {
                    throw new ArgumentOutOfRangeException();
                }
                return this._items[index];
            }
            set
            {
                if (index >= this._count)
                {
                    throw new ArgumentOutOfRangeException();
                }
                this._items[index] = value;
            }
        }

        public bool IsReadOnly =>
            false;
    }
}

