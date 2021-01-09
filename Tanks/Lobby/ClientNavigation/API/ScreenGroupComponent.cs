namespace Tanks.Lobby.ClientNavigation.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;

    public class ScreenGroupComponent : GroupComponent
    {
        public ScreenGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public ScreenGroupComponent(long key) : base(key)
        {
        }
    }
}

