namespace Platform.System.Data.Statics.ClientConfigurator.Impl
{
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using System;
    using System.IO;

    public class ConfiguratorTestUtil
    {
        public static readonly string IDE_CONFIG_PATH = "../../../../ServerGame/config".Replace('/', Path.DirectorySeparatorChar);
        public static readonly string MAVEN_CONFIG_PATH = "../../../../assembly/ServerGame/config".Replace('/', Path.DirectorySeparatorChar);

        public static void SetDefaultConfigs(ConfigurationServiceImpl configurationService)
        {
            string str;
            FileSystemConfigsImporter importer = new FileSystemConfigsImporter();
            if (Directory.Exists(MAVEN_CONFIG_PATH))
            {
                str = MAVEN_CONFIG_PATH;
            }
            else
            {
                if (!Directory.Exists(IDE_CONFIG_PATH))
                {
                    string[] textArray1 = new string[] { "Configs directory was not found. Path for maven: ", MAVEN_CONFIG_PATH, ". Path for IDE: ", IDE_CONFIG_PATH, "." };
                    throw new ConfigsNotFoundException(string.Concat(textArray1));
                }
                str = IDE_CONFIG_PATH;
            }
            ConfigTreeNodeImpl configTreeNode = importer.Import<ConfigTreeNodeImpl>(str, new ConfigurationProfileImpl(null));
            configurationService.SetRootConfigNode(configTreeNode);
        }

        public static void SetTestConfigs(ConfigurationServiceImpl configurationService, string path)
        {
            if (!Directory.Exists(path))
            {
                throw new ConfigsNotFoundException("Configs directory '" + path + "' was not found.");
            }
            ConfigTreeNodeImpl configTreeNode = new FileSystemConfigsImporter().Import<ConfigTreeNodeImpl>(path, new ConfigurationProfileImpl(null));
            configurationService.SetRootConfigNode(configTreeNode);
        }
    }
}

