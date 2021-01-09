namespace YamlDotNet.Helpers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using YamlDotNet;

    internal sealed class GenericDictionaryToNonGenericAdapter : IDictionary, ICollection, IEnumerable
    {
        private readonly object genericDictionary;
        private readonly Type genericDictionaryType;
        private readonly MethodInfo indexerSetter;

        public GenericDictionaryToNonGenericAdapter(object genericDictionary, Type genericDictionaryType)
        {
            this.genericDictionary = genericDictionary;
            this.genericDictionaryType = genericDictionaryType;
            this.indexerSetter = genericDictionaryType.GetPublicProperty("Item").GetSetMethod();
        }

        public void Add(object key, object value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IDictionaryEnumerator GetEnumerator() => 
            new DictionaryEnumerator(this.genericDictionary, this.genericDictionaryType);

        public void Remove(object key)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            ((IEnumerable) this.genericDictionary).GetEnumerator();

        public bool IsFixedSize
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICollection Keys
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public ICollection Values
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object this[object key]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                object[] parameters = new object[] { key, value };
                this.indexerSetter.Invoke(this.genericDictionary, parameters);
            }
        }

        public int Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsSynchronized
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public object SyncRoot
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private class DictionaryEnumerator : IDictionaryEnumerator, IEnumerator
        {
            private readonly IEnumerator enumerator;
            private readonly MethodInfo getKeyMethod;
            private readonly MethodInfo getValueMethod;

            public DictionaryEnumerator(object genericDictionary, Type genericDictionaryType)
            {
                Type[] genericArguments = genericDictionaryType.GetGenericArguments();
                Type type = typeof(KeyValuePair<,>).MakeGenericType(genericArguments);
                this.getKeyMethod = type.GetPublicProperty("Key").GetGetMethod();
                this.getValueMethod = type.GetPublicProperty("Value").GetGetMethod();
                this.enumerator = ((IEnumerable) genericDictionary).GetEnumerator();
            }

            public bool MoveNext() => 
                this.enumerator.MoveNext();

            public void Reset()
            {
                this.enumerator.Reset();
            }

            public DictionaryEntry Entry =>
                new DictionaryEntry(this.Key, this.Value);

            public object Key =>
                this.getKeyMethod.Invoke(this.enumerator.Current, null);

            public object Value =>
                this.getValueMethod.Invoke(this.enumerator.Current, null);

            public object Current =>
                this.Entry;
        }
    }
}

