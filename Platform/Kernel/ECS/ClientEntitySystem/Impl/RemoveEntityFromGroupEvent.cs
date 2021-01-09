namespace Platform.Kernel.ECS.ClientEntitySystem.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    [SerialVersionUID(0x14d4c4f6501L)]
    public class RemoveEntityFromGroupEvent : Event
    {
        public RemoveEntityFromGroupEvent(Type groupComponentType)
        {
            this.GroupComponentType = groupComponentType;
        }

        public Type GroupComponentType { get; private set; }
    }
}

