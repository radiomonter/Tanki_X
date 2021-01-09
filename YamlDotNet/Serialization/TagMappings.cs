namespace YamlDotNet.Serialization
{
    using System;
    using System.Collections.Generic;

    public sealed class TagMappings
    {
        private readonly IDictionary<string, Type> mappings;

        public TagMappings()
        {
            this.mappings = new Dictionary<string, Type>();
        }

        public TagMappings(IDictionary<string, Type> mappings)
        {
            this.mappings = new Dictionary<string, Type>(mappings);
        }

        public void Add(string tag, Type mapping)
        {
            this.mappings.Add(tag, mapping);
        }

        internal Type GetMapping(string tag)
        {
            Type type;
            return (!this.mappings.TryGetValue(tag, out type) ? null : type);
        }
    }
}

