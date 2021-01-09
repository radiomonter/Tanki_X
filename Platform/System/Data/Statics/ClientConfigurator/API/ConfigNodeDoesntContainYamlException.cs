namespace Platform.System.Data.Statics.ClientConfigurator.API
{
    using System;

    public class ConfigNodeDoesntContainYamlException : Exception
    {
        public ConfigNodeDoesntContainYamlException(ConfigTreeNode treeNode) : base("node: " + treeNode)
        {
        }
    }
}

