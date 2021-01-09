namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0x159f3033f1cL)]
    public class UserItemGroupComponent : GroupComponent
    {
        public UserItemGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public UserItemGroupComponent(long key) : base(key)
        {
        }
    }
}

