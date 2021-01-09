namespace Platform.Library.ClientDataStructures.API
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class MultiSet<KEY> : ICollection<KEY>, IEnumerable<KEY>, IEnumerable
    {
        private readonly Dictionary<KEY, int?> _values;

        public MultiSet()
        {
            this._values = new Dictionary<KEY, int?>();
        }

        public void Add(KEY item)
        {
            this.Add(item, 1);
        }

        public void Add(KEY item, int count)
        {
            if (!this._values.ContainsKey(item))
            {
                this._values[item] = new int?(count);
            }
            else
            {
                int? nullable1;
                int? nullable = this._values[item];
                if (nullable != null)
                {
                    nullable1 = new int?(nullable.GetValueOrDefault() + count);
                }
                else
                {
                    nullable1 = null;
                }
                this._values[item] = nullable1;
            }
        }

        public void AddAll(ICollection<KEY> collection)
        {
            MultiSet<KEY> set = collection as MultiSet<KEY>;
            if (set != null)
            {
                foreach (KEY local in set)
                {
                    this.Add(local, set.Occurrence(local));
                }
            }
            else
            {
                foreach (KEY local2 in collection)
                {
                    this.Add(local2);
                }
            }
        }

        public void Clear()
        {
            this._values.Clear();
        }

        public bool Contains(KEY item) => 
            this._values.ContainsKey(item);

        public void CopyTo(KEY[] array, int arrayIndex)
        {
            foreach (KeyValuePair<KEY, int?> pair in this._values)
            {
                array[arrayIndex++] = pair.Key;
            }
        }

        public IEnumerator<KEY> GetEnumerator() => 
            this._values.Keys.GetEnumerator();

        public int Occurrence(KEY key) => 
            !this._values.ContainsKey(key) ? 0 : this._values[key].Value;

        public bool Remove(KEY item)
        {
            int? nullable1;
            if (!this._values.ContainsKey(item))
            {
                return false;
            }
            int? nullable = this._values[item];
            if ((nullable.GetValueOrDefault() == 1) && (nullable != null))
            {
                this._values.Remove(item);
                return true;
            }
            if (nullable != null)
            {
                nullable1 = new int?(nullable.GetValueOrDefault() - 1);
            }
            else
            {
                nullable1 = null;
            }
            this._values[item] = nullable1;
            return true;
        }

        public void Remove(MultiSet<KEY> multiSet)
        {
            foreach (KEY local in multiSet)
            {
                if (this._values.ContainsKey(local))
                {
                    int num = this._values[local].Value - multiSet.Occurrence(local);
                    if (num > 0)
                    {
                        this._values[local] = new int?(num);
                        continue;
                    }
                    this._values.Remove(local);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            this.GetEnumerator();

        public int Count =>
            this._values.Count;

        public bool IsReadOnly =>
            false;
    }
}

