namespace log4net.Util
{
    using System;
    using System.Collections;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Security.Permissions;
    using System.Xml;

    [Serializable]
    public class ReadOnlyPropertiesDictionary : ISerializable, IDictionary, ICollection, IEnumerable
    {
        private Hashtable m_hashtable;

        public ReadOnlyPropertiesDictionary()
        {
            this.m_hashtable = new Hashtable();
        }

        public ReadOnlyPropertiesDictionary(ReadOnlyPropertiesDictionary propertiesDictionary)
        {
            this.m_hashtable = new Hashtable();
            IEnumerator enumerator = ((IEnumerable) propertiesDictionary).GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    this.InnerHashtable.Add(current.Key, current.Value);
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        protected ReadOnlyPropertiesDictionary(SerializationInfo info, StreamingContext context)
        {
            this.m_hashtable = new Hashtable();
            SerializationInfoEnumerator enumerator = info.GetEnumerator();
            while (enumerator.MoveNext())
            {
                SerializationEntry current = enumerator.Current;
                this.InnerHashtable[XmlConvert.DecodeName(current.Name)] = current.Value;
            }
        }

        public virtual void Clear()
        {
            throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
        }

        public bool Contains(string key) => 
            this.InnerHashtable.Contains(key);

        public string[] GetKeys()
        {
            string[] array = new string[this.InnerHashtable.Count];
            this.InnerHashtable.Keys.CopyTo(array, 0);
            return array;
        }

        [SecurityPermission(SecurityAction.Demand, SerializationFormatter=true)]
        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            IDictionaryEnumerator enumerator = this.InnerHashtable.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    string key = current.Key as string;
                    object obj2 = current.Value;
                    if ((key != null) && ((obj2 != null) && obj2.GetType().IsSerializable))
                    {
                        info.AddValue(XmlConvert.EncodeLocalName(key), obj2);
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this.InnerHashtable.CopyTo(array, index);
        }

        void IDictionary.Add(object key, object value)
        {
            throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
        }

        bool IDictionary.Contains(object key) => 
            this.InnerHashtable.Contains(key);

        IDictionaryEnumerator IDictionary.GetEnumerator() => 
            this.InnerHashtable.GetEnumerator();

        void IDictionary.Remove(object key)
        {
            throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
        }

        IEnumerator IEnumerable.GetEnumerator() => 
            ((IEnumerable) this.InnerHashtable).GetEnumerator();

        bool IDictionary.IsReadOnly =>
            true;

        object IDictionary.this[object key]
        {
            get
            {
                if (!(key is string))
                {
                    throw new ArgumentException("key must be a string");
                }
                return this.InnerHashtable[key];
            }
            set
            {
                throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
            }
        }

        ICollection IDictionary.Values =>
            this.InnerHashtable.Values;

        ICollection IDictionary.Keys =>
            this.InnerHashtable.Keys;

        bool IDictionary.IsFixedSize =>
            this.InnerHashtable.IsFixedSize;

        bool ICollection.IsSynchronized =>
            this.InnerHashtable.IsSynchronized;

        object ICollection.SyncRoot =>
            this.InnerHashtable.SyncRoot;

        public virtual object this[string key]
        {
            get => 
                this.InnerHashtable[key];
            set
            {
                throw new NotSupportedException("This is a Read Only Dictionary and can not be modified");
            }
        }

        protected Hashtable InnerHashtable =>
            this.m_hashtable;

        public int Count =>
            this.InnerHashtable.Count;
    }
}

