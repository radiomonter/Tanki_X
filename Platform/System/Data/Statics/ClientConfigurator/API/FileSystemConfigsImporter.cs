namespace Platform.System.Data.Statics.ClientConfigurator.API
{
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.System.Data.Statics.ClientConfigurator.Impl;
    using Platform.System.Data.Statics.ClientYaml.API;
    using Platform.System.Data.Statics.ClientYaml.Impl;
    using System;
    using System.IO;
    using System.Runtime.CompilerServices;

    public class FileSystemConfigsImporter
    {
        private ConfigTreeNode root;
        private string rootPath;
        private ConfigurationProfile configurationProfile;
        private ConfigsMerger configsMerger = new ConfigsMerger();

        private void CreateConfigTree(DirectoryInfo directoryInfo, ConfigsMerger merger)
        {
            foreach (DirectoryInfo info in directoryInfo.GetDirectories())
            {
                string path = this.GetPath(info.FullName);
                this.root.FindOrCreateNode(path);
                this.CreateConfigTree(info, merger);
            }
            foreach (FileInfo info2 in directoryInfo.GetFiles())
            {
                if (this.configurationProfile.Match(info2.Name))
                {
                    string path = this.GetPath(directoryInfo.FullName);
                    ConfigTreeNode configTreeNode = this.root.FindOrCreateNode(path);
                    try
                    {
                        YamlNodeImpl yamlNode = YamlService.Load(info2);
                        merger.Put(configTreeNode, info2.Name, yamlNode);
                    }
                    catch (Exception exception1)
                    {
                        throw new Exception(path, exception1);
                    }
                }
            }
        }

        private string GetPath(string fullConfigPath)
        {
            char[] trimChars = new char[] { Path.DirectorySeparatorChar };
            return fullConfigPath.Substring(this.rootPath.Length).Trim(trimChars).Replace(Path.DirectorySeparatorChar, '/');
        }

        public T Import<T>(string path, ConfigurationProfile configurationProfile) where T: ConfigTreeNode, new()
        {
            this.configurationProfile = configurationProfile;
            this.root = Activator.CreateInstance<T>();
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            this.rootPath = directoryInfo.FullName;
            this.CreateConfigTree(directoryInfo, this.configsMerger);
            this.configsMerger.Merge();
            return (T) this.root;
        }

        [Inject]
        public static Platform.System.Data.Statics.ClientYaml.API.YamlService YamlService { get; set; }
    }
}

