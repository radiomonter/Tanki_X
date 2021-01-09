namespace YamlDotNet.Serialization
{
    using System;
    using YamlDotNet.Core;

    public interface IYamlSerializable
    {
        void ReadYaml(IParser parser);
        void WriteYaml(IEmitter emitter);
    }
}

