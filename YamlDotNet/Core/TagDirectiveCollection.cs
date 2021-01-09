namespace YamlDotNet.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using YamlDotNet.Core.Tokens;

    public class TagDirectiveCollection : KeyedCollection<string, TagDirective>
    {
        public TagDirectiveCollection()
        {
        }

        public TagDirectiveCollection(IEnumerable<TagDirective> tagDirectives)
        {
            foreach (TagDirective directive in tagDirectives)
            {
                base.Add(directive);
            }
        }

        public bool Contains(TagDirective directive) => 
            base.Contains(this.GetKeyForItem(directive));

        protected override string GetKeyForItem(TagDirective item) => 
            item.Handle;
    }
}

