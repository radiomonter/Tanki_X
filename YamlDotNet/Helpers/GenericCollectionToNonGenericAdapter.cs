namespace YamlDotNet.Helpers
{
    using System;
    using System.Collections;
    using System.Reflection;
    using YamlDotNet;

    internal sealed class GenericCollectionToNonGenericAdapter : IList, ICollection, IEnumerable
    {
        private readonly object genericCollection;
        private readonly MethodInfo addMethod;
        private readonly MethodInfo indexerSetter;
        private readonly MethodInfo countGetter;

        public GenericCollectionToNonGenericAdapter(object genericCollection, Type genericCollectionType, Type genericListType)
        {
            this.genericCollection = genericCollection;
            this.addMethod = genericCollectionType.GetPublicInstanceMethod("Add");
            this.countGetter = genericCollectionType.GetPublicProperty("Count").GetGetMethod();
            if (genericListType != null)
            {
                this.indexerSetter = genericListType.GetPublicProperty("Item").GetSetMethod();
            }
        }

        public int Add(object value)
        {
            int num = (int) this.countGetter.Invoke(this.genericCollection, null);
            object[] parameters = new object[] { value };
            this.addMethod.Invoke(this.genericCollection, parameters);
            return num;
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator() => 
            ((IEnumerable) this.genericCollection).GetEnumerator();

        public int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        public void Remove(object value)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

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

        public object this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                object[] parameters = new object[] { index, value };
                this.indexerSetter.Invoke(this.genericCollection, parameters);
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
    }
}

