namespace Platform.System.Data.Statics.ClientConfigurator.Impl
{
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using Platform.System.Data.Statics.ClientYaml.Impl;
    using System;
    using System.Collections.Generic;

    public class ConfigurationServiceImpl : ConfigurationService
    {
        public static readonly YamlNode EMPTY_YAML_NODE = new YamlNodeImpl(new Dictionary<object, object>());
        public readonly Dictionary<string, YamlNodeImpl> cache = new Dictionary<string, YamlNodeImpl>();
        public static readonly string CONFIG_FILE = "public";
        private ConfigTreeNode root = new ConfigTreeNodeImpl(string.Empty);

        public void CacheAllPaths()
        {
            CacheAllPaths(this.root, string.Empty, this.cache);
        }

        private static void CacheAllPaths(ConfigTreeNode node, string parentPath, Dictionary<string, YamlNodeImpl> nodes)
        {
            foreach (ConfigTreeNode node2 in node.GetChildren())
            {
                string key = !string.IsNullOrEmpty(parentPath) ? (parentPath + "/" + node2.ConfigPath) : node2.ConfigPath;
                if (node2.HasYaml())
                {
                    nodes.Add(key, (YamlNodeImpl) node2.GetYaml());
                }
                CacheAllPaths(node2, key, nodes);
            }
        }

        private static void CollectAllLeafPaths(ConfigTreeNode parentNode, string parentPath, List<string> paths)
        {
            if (parentNode != null)
            {
                foreach (ConfigTreeNode node in parentNode.GetChildren())
                {
                    string str = parentPath + "/" + node.ConfigPath;
                    if (node.GetChildren().Count > 0)
                    {
                        CollectAllLeafPaths(node, str, paths);
                        continue;
                    }
                    paths.Add(str);
                }
            }
        }

        public YamlNode GetConfig(string path)
        {
            YamlNode configOrNull = this.GetConfigOrNull(path);
            if (configOrNull == null)
            {
                throw new ConfigWasNotFoundException(path);
            }
            return configOrNull;
        }

        public YamlNode GetConfigOrNull(string path)
        {
            YamlNodeImpl impl;
            if (!this.cache.TryGetValue(path, out impl))
            {
                ConfigTreeNode node = this.root.FindNode(path);
                impl = (node == null) ? null : ((YamlNodeImpl) node.GetYaml());
                this.cache[path] = impl;
            }
            return impl;
        }

        public List<string> GetPathsByWildcard(string pathWithWildcard)
        {
            List<string> paths = new List<string>();
            if (pathWithWildcard.EndsWith("*"))
            {
                string parentPath = pathWithWildcard.Substring(0, pathWithWildcard.Length - 2);
                CollectAllLeafPaths(this.root.FindNode(parentPath), parentPath, paths);
            }
            return paths;
        }

        public bool HasConfig(string path) => 
            !ReferenceEquals(this.GetConfigOrNull(path), null);

        public void SetRootConfigNode(ConfigTreeNode configTreeNode)
        {
            this.root = configTreeNode;
        }
    }
}

