namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Runtime.CompilerServices;

    public class ConfigComponentInfo : ComponentInfo
    {
        private string keyName;

        public ConfigComponentInfo(string keyName, bool configOptional)
        {
            this.keyName = keyName;
            this.ConfigOptional = configOptional;
        }

        public string KeyName
        {
            get => 
                this.keyName;
            set => 
                this.keyName = value;
        }

        public bool ConfigOptional { get; private set; }
    }
}

