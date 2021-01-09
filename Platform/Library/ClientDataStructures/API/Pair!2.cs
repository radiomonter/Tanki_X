namespace Platform.Library.ClientDataStructures.API
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class Pair<K, V>
    {
        public Pair(K k, V v)
        {
            this.Key = k;
            this.Value = v;
        }

        protected bool Equals(Pair<K, V> other) => 
            EqualityComparer<V>.Default.Equals(this.Value, other.Value) && EqualityComparer<K>.Default.Equals(this.Key, other.Key);

        public override bool Equals(object obj) => 
            !ReferenceEquals(null, obj) ? (!ReferenceEquals(this, obj) ? (ReferenceEquals(obj.GetType(), base.GetType()) ? this.Equals((Pair<K, V>) obj) : false) : true) : false;

        public override int GetHashCode() => 
            (EqualityComparer<V>.Default.GetHashCode(this.Value) * 0x18d) ^ EqualityComparer<K>.Default.GetHashCode(this.Key);

        public V Value { get; set; }

        public K Key { get; set; }
    }
}

