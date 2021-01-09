namespace Platform.System.Data.Statics.ClientYaml.API
{
    using Platform.System.Data.Statics.ClientYaml.Impl;
    using System;
    using System.IO;
    using YamlDotNet.Serialization;

    public interface YamlService
    {
        string Dump(object data);
        void Dump(object data, FileInfo file);
        T Load<T>(YamlNodeImpl node);
        YamlNodeImpl Load(FileInfo file);
        T Load<T>(FileInfo file);
        YamlNodeImpl Load(TextReader data);
        T Load<T>(TextReader reader);
        YamlNodeImpl Load(string data);
        T Load<T>(string data);
        object Load(YamlNodeImpl node, Type type);
        object Load(string data, Type type);
        void RegisterConverter(IYamlTypeConverter converter);
    }
}

