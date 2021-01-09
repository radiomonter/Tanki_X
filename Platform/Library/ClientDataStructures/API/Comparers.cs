namespace Platform.Library.ClientDataStructures.API
{
    using Platform.Library.ClientDataStructures.Impl;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Comparers
    {
        public static IComparer<T> GetComparer<T>()
        {
            if (!typeof(IComparable<T>).IsAssignableFrom(typeof(T)) && !typeof(IComparable).IsAssignableFrom(typeof(T)))
            {
                throw new TypeIsNotComparableException(typeof(T));
            }
            return Comparer<T>.Default;
        }

        public static IComparer<T> GetComparer<T>(Comparison<T> comparison) => 
            new ComparisonComparer<T>(comparison);

        public static IComparer GetComparer(Type type)
        {
            if (!typeof(IComparable).IsAssignableFrom(type))
            {
                throw new TypeIsNotComparableException(type);
            }
            return Comparer.Default;
        }

        private class ComparisonComparer<T> : IComparer<T>
        {
            private Comparison<T> comparison;

            public ComparisonComparer(Comparison<T> comparison)
            {
                this.comparison = comparison;
            }

            public int Compare(T x, T y) => 
                this.comparison(x, y);
        }
    }
}

