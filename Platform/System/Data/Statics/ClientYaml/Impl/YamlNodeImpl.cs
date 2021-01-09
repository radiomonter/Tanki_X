namespace Platform.System.Data.Statics.ClientYaml.Impl
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class YamlNodeImpl : YamlNode
    {
        public Dictionary<object, object> innerDictionary;
        public Dictionary<Type, object> typeToPrototypeCache = new Dictionary<Type, object>(new Comparer());
        public Dictionary<string, YamlNode> keyToChildNode = new Dictionary<string, YamlNode>();
        private bool noClone;
        [CompilerGenerated]
        private static Func<Dictionary<object, object>, YamlNodeImpl> <>f__am$cache0;
        [CompilerGenerated]
        private static Func<object, string> <>f__am$cache1;

        public YamlNodeImpl(Dictionary<object, object> innerDictionary)
        {
            this.innerDictionary = innerDictionary;
        }

        [CompilerGenerated]
        private static bool <GetListContentsOf`1>m__1<T>(object x) => 
            x is T;

        public T CastValueTo<T>(string key)
        {
            object obj2;
            if (!this.innerDictionary.TryGetValue(key, out obj2))
            {
                throw new UnknownYamlKeyException(key);
            }
            this.CheckType(key, typeof(T), obj2);
            return (T) obj2;
        }

        private void CheckType(string key, Type type, object value)
        {
            if ((type != null) && !type.IsInstanceOfType(value))
            {
                throw new WrongYamlStructureException(key, type, value.GetType());
            }
        }

        public T ConvertTo<T>() => 
            (T) this.ConvertTo(typeof(T));

        public object ConvertTo(Type t)
        {
            object objectPrototypeForType = this.GetObjectPrototypeForType(t);
            return (!this.noClone ? CloneObjectUtil.CloneObject(objectPrototypeForType) : objectPrototypeForType);
        }

        public List<YamlNode> GetChildListNodes(string key)
        {
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = x => new YamlNodeImpl(x);
            }
            return this.GetListContentsOf<Dictionary<object, object>>(key).Select<Dictionary<object, object>, YamlNodeImpl>(<>f__am$cache0).Cast<YamlNode>().ToList<YamlNode>();
        }

        public List<string> GetChildListValues(string key) => 
            this.GetListContentsOf<string>(key).ToList<string>();

        public YamlNode GetChildNode(string key)
        {
            YamlNode node;
            if (!this.keyToChildNode.TryGetValue(key, out node))
            {
                node = new YamlNodeImpl(this.CastValueTo<Dictionary<object, object>>(key));
                this.keyToChildNode.Add(key, node);
            }
            return node;
        }

        public ICollection<string> GetKeys()
        {
            if (<>f__am$cache1 == null)
            {
                <>f__am$cache1 = k => k.ToString();
            }
            return new List<string>(this.innerDictionary.Keys.Select<object, string>(<>f__am$cache1));
        }

        public List<T> GetList<T>(string key) => 
            this.GetListContentsOf<T>(key).ToList<T>();

        private IEnumerable<T> GetListContentsOf<T>(string key)
        {
            List<object> source = this.CastValueTo<List<object>>(key);
            if (!source.All<object>(new Func<object, bool>(YamlNodeImpl.<GetListContentsOf`1>m__1<T>)))
            {
                throw new WrongYamlStructureException("element of " + key, typeof(T), typeof(object));
            }
            return source.Cast<T>();
        }

        private object GetObjectPrototypeForType(Type t)
        {
            object obj2;
            this.typeToPrototypeCache.TryGetValue(t, out obj2);
            if (obj2 == null)
            {
                obj2 = YamlService.Load(this, t);
                this.typeToPrototypeCache.Add(t, obj2);
            }
            return obj2;
        }

        public string GetStringValue(string key) => 
            this.CastValueTo<string>(key);

        public object GetValue(string key) => 
            this.innerDictionary[key];

        public object GetValueOrNull(string key)
        {
            object obj2;
            return (!this.innerDictionary.TryGetValue(key, out obj2) ? null : obj2);
        }

        public bool HasValue(string key) => 
            this.innerDictionary.ContainsKey(key);

        public void Merge(YamlNodeImpl yamlNode)
        {
            this.MergeDictionary(this.innerDictionary, yamlNode.innerDictionary);
        }

        public void MergeDictionary(IDictionary destination, IDictionary source)
        {
            IDictionaryEnumerator enumerator = source.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    DictionaryEntry current = (DictionaryEntry) enumerator.Current;
                    destination[current.Key] = this.MergeValue(current.Key, destination[current.Key], current.Value);
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

        private void MergeList(IList destination, IList source)
        {
            IEnumerator enumerator = source.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    object current = enumerator.Current;
                    destination.Add(current);
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

        private object MergeValue(object key, object destValue, object sourceValue)
        {
            if (destValue == null)
            {
                return sourceValue;
            }
            if (!ReferenceEquals(destValue.GetType(), sourceValue.GetType()))
            {
                throw new MergingYamlMismatchException((string) key, destValue.GetType(), sourceValue.GetType());
            }
            if (destValue is IDictionary)
            {
                this.MergeDictionary((IDictionary) destValue, (IDictionary) sourceValue);
            }
            else
            {
                if (!(destValue is IList))
                {
                    return sourceValue;
                }
                this.MergeList((IList) destValue, (IList) sourceValue);
            }
            return destValue;
        }

        public void PreloadObject(Type type, bool noCloneOnConvert)
        {
            this.GetObjectPrototypeForType(type);
            this.noClone = noCloneOnConvert;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<object, object> pair in this.innerDictionary)
            {
                builder.AppendFormat("{0}: {1}, ", pair.Key, pair.Value);
            }
            return builder.ToString().TrimEnd(",".ToCharArray());
        }

        [Inject]
        public static Platform.System.Data.Statics.ClientYaml.API.YamlService YamlService { get; set; }

        public class Comparer : IEqualityComparer<Type>
        {
            public bool Equals(Type x, Type y) => 
                x.FullName.Equals(y.FullName);

            public int GetHashCode(Type obj) => 
                obj.GetHashCode();
        }
    }
}

