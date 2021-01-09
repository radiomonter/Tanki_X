namespace Platform.Common.ClientECSCommon.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [Shared, SerialVersionUID(0x150adc6dd5aL)]
    public class NameComponent : Component
    {
        public NameComponent()
        {
        }

        public NameComponent(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}

