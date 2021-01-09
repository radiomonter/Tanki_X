namespace Platform.System.Data.Statics.ClientConfigurator.API
{
    using Platform.System.Data.Statics.ClientYaml.API;
    using System;
    using System.Collections.Generic;

    public interface ConfigurationService
    {
        YamlNode GetConfig(string path);
        YamlNode GetConfigOrNull(string path);
        List<string> GetPathsByWildcard(string pathWithWildcard);
        bool HasConfig(string path);
    }
}

