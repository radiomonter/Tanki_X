namespace Platform.System.Data.Statics.ClientYaml.API
{
    using System;
    using System.Collections.Generic;

    public interface YamlNode
    {
        T ConvertTo<T>();
        object ConvertTo(Type t);
        List<YamlNode> GetChildListNodes(string key);
        List<string> GetChildListValues(string key);
        YamlNode GetChildNode(string key);
        List<T> GetList<T>(string key);
        string GetStringValue(string key);
        object GetValue(string key);
        object GetValueOrNull(string key);
        bool HasValue(string key);
    }
}

