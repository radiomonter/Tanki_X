namespace YamlDotNet.Serialization.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using YamlDotNet.Core;

    internal sealed class ObjectAnchorCollection
    {
        private readonly IDictionary<string, object> objectsByAnchor = new Dictionary<string, object>();
        private readonly IDictionary<object, string> anchorsByObject = new Dictionary<object, string>();

        public void Add(string anchor, object @object)
        {
            this.objectsByAnchor.Add(anchor, @object);
            if (@object != null)
            {
                this.anchorsByObject.Add(@object, anchor);
            }
        }

        public bool TryGetAnchor(object @object, out string anchor) => 
            this.anchorsByObject.TryGetValue(@object, out anchor);

        public object this[string anchor]
        {
            get
            {
                object obj2;
                if (this.objectsByAnchor.TryGetValue(anchor, out obj2))
                {
                    return obj2;
                }
                object[] args = new object[] { anchor };
                throw new AnchorNotFoundException(string.Format(CultureInfo.InvariantCulture, "The anchor '{0}' does not exists", args));
            }
        }
    }
}

