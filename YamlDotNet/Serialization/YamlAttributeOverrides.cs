namespace YamlDotNet.Serialization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using YamlDotNet;

    public sealed class YamlAttributeOverrides
    {
        private readonly Dictionary<Type, Dictionary<string, List<Attribute>>> overrides = new Dictionary<Type, Dictionary<string, List<Attribute>>>();

        public void Add(Type type, string member, Attribute attribute)
        {
            Dictionary<string, List<Attribute>> dictionary;
            List<Attribute> list;
            <Add>c__AnonStorey0 storey = new <Add>c__AnonStorey0 {
                attribute = attribute
            };
            if (!this.overrides.TryGetValue(type, out dictionary))
            {
                dictionary = new Dictionary<string, List<Attribute>>();
                this.overrides.Add(type, dictionary);
            }
            if (!dictionary.TryGetValue(member, out list))
            {
                list = new List<Attribute>();
                dictionary.Add(member, list);
            }
            if (!list.Any<Attribute>(new Func<Attribute, bool>(storey.<>m__0)))
            {
                list.Add(storey.attribute);
            }
            else
            {
                object[] args = new object[] { type.FullName, member, storey.attribute };
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Attribute ({3}) already set for Type {0}, Member {1}", args));
            }
        }

        public T GetAttribute<T>(Type type, string member) where T: Attribute
        {
            ICollection<Attribute> source = this[type, member];
            return ((source != null) ? source.OfType<T>().FirstOrDefault<T>() : null);
        }

        public ICollection<Attribute> this[Type type, string member]
        {
            get
            {
                Dictionary<string, List<Attribute>> dictionary;
                List<Attribute> list;
                return (this.overrides.TryGetValue(type, out dictionary) ? (dictionary.TryGetValue(member, out list) ? list : null) : null);
            }
        }

        [CompilerGenerated]
        private sealed class <Add>c__AnonStorey0
        {
            internal Attribute attribute;

            internal bool <>m__0(Attribute attr) => 
                attr.GetType().IsInstanceOf(this.attribute);
        }
    }
}

