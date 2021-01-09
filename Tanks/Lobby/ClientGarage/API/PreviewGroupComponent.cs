namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class PreviewGroupComponent : GroupComponent
    {
        public PreviewGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public PreviewGroupComponent(long key) : base(key)
        {
        }
    }
}

