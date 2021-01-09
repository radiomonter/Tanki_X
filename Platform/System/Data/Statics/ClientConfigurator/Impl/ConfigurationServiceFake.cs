namespace Platform.System.Data.Statics.ClientConfigurator.Impl
{
    using Platform.Library.ClientDataStructures.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public class ConfigurationServiceFake : ConfigurationService
    {
        private IDictionary<string, YamlNode> nodes = new Dictionary<string, YamlNode>();

        public void AddConfig(string path, YamlNode yamlNode)
        {
            this.nodes[path] = yamlNode;
        }

        public YamlNode GetConfig(string path) => 
            this.nodes[path];

        public virtual IEnumerable<ConfigTreeNode> GetConfigLocations(string path) => 
            Collections.EmptyList<ConfigTreeNode>();

        public YamlNode GetConfigOrNull(string path)
        {
            YamlNode node;
            this.nodes.TryGetValue(path, out node);
            return node;
        }

        public List<string> GetPathsByWildcard(string pathWithWildcard) => 
            new List<string>();

        public bool HasConfig(string path) => 
            this.nodes.ContainsKey(path);

        public Action ErrorHandler { private get; set; }
    }
}

