namespace Platform.System.Data.Statics.ClientConfigurator.API
{
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Collections.Generic;

    public interface ConfigTreeNode
    {
        ConfigTreeNode FindNode(string path);
        ConfigTreeNode FindOrCreateNode(string configPath);
        ICollection<ConfigTreeNode> GetChildren();
        YamlNode GetYaml();
        bool HasYaml();
        void SetYaml(YamlNode yamlNode);

        string ConfigPath { get; }
    }
}

