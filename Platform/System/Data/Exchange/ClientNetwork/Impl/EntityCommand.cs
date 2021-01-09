namespace Platform.System.Data.Exchange.ClientNetwork.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.ECS.ClientEntitySystem.Impl;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Runtime.CompilerServices;

    public abstract class EntityCommand : AbstractCommand
    {
        public EntityCommand()
        {
        }

        public EntityCommand(Platform.Kernel.ECS.ClientEntitySystem.API.Entity entity)
        {
            this.Entity = (EntityInternal) entity;
        }

        [ProtocolParameterOrder(0)]
        public EntityInternal Entity { get; set; }
    }
}

