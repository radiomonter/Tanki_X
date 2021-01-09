namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class ContainerContentItemGroupComponent : GroupComponent
    {
        public ContainerContentItemGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public ContainerContentItemGroupComponent(long key) : base(key)
        {
        }
    }
}

