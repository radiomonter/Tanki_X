namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0x159f3b543ddL)]
    public class ModuleGroupComponent : GroupComponent
    {
        public ModuleGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public ModuleGroupComponent(long key) : base(key)
        {
        }
    }
}

