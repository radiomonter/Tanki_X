namespace Platform.System.Data.Statics.ClientConfigurator.Impl
{
    using Platform.Library.ClientDataStructures.Impl;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    public class ConfigTreeNodeImpl : ConfigTreeNode
    {
        private Dictionary<string, ConfigTreeNodeImpl> children;
        private YamlNode yamlNode;
        private static readonly PathHelper pathHelper = new PathHelper();
        [CompilerGenerated]
        private static Func<ConfigTreeNodeImpl, ConfigTreeNode> <>f__am$cache0;

        public ConfigTreeNodeImpl()
        {
            this.children = new Dictionary<string, ConfigTreeNodeImpl>();
            this.ConfigPath = string.Empty;
        }

        public ConfigTreeNodeImpl(string configPath)
        {
            this.children = new Dictionary<string, ConfigTreeNodeImpl>();
            this.ConfigPath = configPath;
        }

        public void Add(ConfigTreeNodeImpl configTreeNode)
        {
            if (this.ConfigPath != configTreeNode.ConfigPath)
            {
                this.TryAddAsChild(configTreeNode.ConfigPath, configTreeNode);
            }
            else
            {
                foreach (KeyValuePair<string, ConfigTreeNodeImpl> pair in configTreeNode.children)
                {
                    this.TryAddAsChild(pair.Key, pair.Value);
                }
            }
        }

        public ConfigTreeNode FindNode(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            char[] trimChars = new char[] { '/' };
            path = path.Trim(trimChars);
            pathHelper.Init(path);
            ConfigTreeNodeImpl impl = this;
            while (pathHelper.HasNextPathPart())
            {
                ConfigTreeNodeImpl impl2;
                if (!impl.children.TryGetValue(pathHelper.GetNextPathPart(), out impl2))
                {
                    return null;
                }
                impl = impl2;
            }
            return impl;
        }

        public ConfigTreeNode FindOrCreateNode(string configPath)
        {
            if (string.IsNullOrEmpty(configPath))
            {
                return this;
            }
            char[] trimChars = new char[] { '/' };
            configPath = configPath.Trim(trimChars);
            pathHelper.Init(configPath);
            ConfigTreeNodeImpl impl = this;
            while (pathHelper.HasNextPathPart())
            {
                ConfigTreeNodeImpl impl2;
                string nextPathPart = pathHelper.GetNextPathPart();
                if (!impl.children.TryGetValue(nextPathPart, out impl2))
                {
                    impl2 = new ConfigTreeNodeImpl(nextPathPart);
                    impl.children.Add(nextPathPart, impl2);
                }
                impl = impl2;
            }
            return impl;
        }

        public ICollection<ConfigTreeNode> GetChildren()
        {
            if (this.children.Count == 0)
            {
                return EmptyList<ConfigTreeNode>.Instance;
            }
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = c => c;
            }
            return this.children.Values.Select<ConfigTreeNodeImpl, ConfigTreeNode>(<>f__am$cache0).ToList<ConfigTreeNode>();
        }

        public YamlNode GetYaml() => 
            this.yamlNode;

        public bool HasYaml() => 
            !ReferenceEquals(this.yamlNode, null);

        public void SetYaml(YamlNode yamlNode)
        {
            this.yamlNode = yamlNode;
        }

        public override string ToString() => 
            $"[{base.GetType().Name}: ConfigPath={this.ConfigPath} HasYaml={!ReferenceEquals(this.yamlNode, null)}]";

        private void TryAddAsChild(string configName, ConfigTreeNodeImpl config)
        {
            if (this.children.ContainsKey(configName))
            {
                this.children[configName].Add(config);
            }
            else
            {
                this.children.Add(configName, config);
            }
        }

        public string ConfigPath { get; protected set; }

        private class PathHelper
        {
            private string[] pathParts;
            private int index;

            public string GetNextPathPart()
            {
                this.index++;
                return this.pathParts[this.index];
            }

            public bool HasNextPathPart() => 
                (this.index + 1) < this.pathParts.Length;

            public void Init(string path)
            {
                char[] separator = new char[] { '/' };
                this.pathParts = path.Split(separator);
                this.TrimByGoBackPath();
                this.index = -1;
            }

            private void TrimByGoBackPath()
            {
                int index = this.pathParts.Length - 1;
                while (this.pathParts[index].Equals(".."))
                {
                    index--;
                }
                int num2 = ((this.pathParts.Length - 1) - index) * 2;
                if (num2 > 0)
                {
                    int length = this.pathParts.Length - num2;
                    string[] destinationArray = new string[length];
                    Array.Copy(this.pathParts, destinationArray, length);
                    this.pathParts = destinationArray;
                }
            }
        }
    }
}

