namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class ContainerGroupComponent : GroupComponent
    {
        public ContainerGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public ContainerGroupComponent(long key) : base(key)
        {
        }
    }
}

