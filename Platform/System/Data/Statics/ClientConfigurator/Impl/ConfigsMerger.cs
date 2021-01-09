namespace Platform.System.Data.Statics.ClientConfigurator.Impl
{
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using Platform.System.Data.Statics.ClientYaml.Impl;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    public class ConfigsMerger
    {
        private static readonly ConfigDataComparer configDataComparer = new ConfigDataComparer();
        private Dictionary<ConfigTreeNode, List<ConfigData>> configNodeToConfigDataList = new Dictionary<ConfigTreeNode, List<ConfigData>>();

        public void Merge()
        {
            foreach (ConfigTreeNode node in this.configNodeToConfigDataList.Keys)
            {
                YamlNodeImpl yamlNode;
                List<ConfigData> list = this.configNodeToConfigDataList[node];
                if (list.Count <= 1)
                {
                    yamlNode = list[0].yamlNode;
                }
                else
                {
                    list.Sort(configDataComparer);
                    yamlNode = list[0].yamlNode;
                    for (int i = 1; i < list.Count; i++)
                    {
                        ConfigData data2 = list[i];
                        yamlNode.Merge(data2.yamlNode);
                    }
                }
                node.SetYaml(yamlNode);
            }
            this.configNodeToConfigDataList.Clear();
        }

        public void Put(ConfigTreeNode configTreeNode, string configName, YamlNodeImpl yamlNode)
        {
            List<ConfigData> list;
            ConfigData item = new ConfigData(configName, yamlNode);
            if (this.configNodeToConfigDataList.ContainsKey(configTreeNode))
            {
                list = this.configNodeToConfigDataList[configTreeNode];
            }
            else
            {
                list = new List<ConfigData>();
                this.configNodeToConfigDataList[configTreeNode] = list;
            }
            list.Add(item);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct ConfigData
        {
            public readonly YamlNodeImpl yamlNode;
            public readonly int profileElements;
            public ConfigData(string configName, YamlNodeImpl yamlNode)
            {
                this.yamlNode = yamlNode;
                char[] separator = new char[] { '_' };
                this.profileElements = configName.Split(separator).Length - 1;
            }
        }

        private class ConfigDataComparer : IComparer<ConfigsMerger.ConfigData>
        {
            public int Compare(ConfigsMerger.ConfigData x, ConfigsMerger.ConfigData y) => 
                x.profileElements.CompareTo(y.profileElements);
        }
    }
}

