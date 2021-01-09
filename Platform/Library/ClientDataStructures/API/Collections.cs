namespace Platform.Library.ClientDataStructures.API
{
    using Platform.Library.ClientDataStructures.Impl;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class Collections
    {
        public static readonly object[] EmptyArray = new object[0];

        public static List<T> AsList<T>(params T[] values) => 
            new List<T>(values);

        public static IList<T> EmptyList<T>() => 
            EmptyList<T>.Instance;

        public static void ForEach<T>(IEnumerable<T> coll, Action<T> action)
        {
            Enumerator<T> enumerator = GetEnumerator<T>(coll);
            while (enumerator.MoveNext())
            {
                action(enumerator.Current);
            }
        }

        public static Enumerator<T> GetEnumerator<T>(IEnumerable<T> collection) => 
            new Enumerator<T>(collection);

        public static T GetOnlyElement<T>(ICollection<T> coll)
        {
            if (coll.Count != 1)
            {
                throw new InvalidOperationException("Count: " + coll.Count);
            }
            List<T> list = coll as List<T>;
            if (list != null)
            {
                return list[0];
            }
            HashSet<T> set = coll as HashSet<T>;
            if (set != null)
            {
                HashSet<T>.Enumerator enumerator = set.GetEnumerator();
                enumerator.MoveNext();
                return enumerator.Current;
            }
            IEnumerator<T> enumerator2 = coll.GetEnumerator();
            enumerator2.MoveNext();
            return enumerator2.Current;
        }

        public static IList<T> SingletonList<T>(T value) => 
            new SingletonList<T>(value);

        [StructLayout(LayoutKind.Sequential)]
        public struct Enumerator<T>
        {
            private IEnumerable<T> collection;
            private HashSet<T>.Enumerator hashSetEnumerator;
            private List<T>.Enumerator ListEnumerator;
            private IEnumerator<T> enumerator;
            public Enumerator(IEnumerable<T> collection)
            {
                this.collection = collection;
                this.enumerator = null;
                List<T> list = collection as List<T>;
                if (list != null)
                {
                    this.ListEnumerator = list.GetEnumerator();
                    HashSet<T>.Enumerator enumerator = new HashSet<T>.Enumerator();
                    this.hashSetEnumerator = enumerator;
                }
                else
                {
                    HashSet<T> set = collection as HashSet<T>;
                    if (set != null)
                    {
                        this.hashSetEnumerator = set.GetEnumerator();
                        List<T>.Enumerator enumerator2 = new List<T>.Enumerator();
                        this.ListEnumerator = enumerator2;
                    }
                    else
                    {
                        HashSet<T>.Enumerator enumerator3 = new HashSet<T>.Enumerator();
                        this.hashSetEnumerator = enumerator3;
                        List<T>.Enumerator enumerator4 = new List<T>.Enumerator();
                        this.ListEnumerator = enumerator4;
                        this.enumerator = collection.GetEnumerator();
                    }
                }
            }

            public bool MoveNext() => 
                !(this.collection is List<T>) ? (!(this.collection is HashSet<T>) ? this.enumerator.MoveNext() : this.hashSetEnumerator.MoveNext()) : this.ListEnumerator.MoveNext();

            public T Current =>
                !(this.collection is List<T>) ? (!(this.collection is HashSet<T>) ? this.enumerator.Current : this.hashSetEnumerator.Current) : this.ListEnumerator.Current;
        }
    }
}

