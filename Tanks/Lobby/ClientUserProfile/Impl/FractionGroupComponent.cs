namespace Tanks.Lobby.ClientUserProfile.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0x1679c04879bL)]
    public class FractionGroupComponent : GroupComponent
    {
        public FractionGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public FractionGroupComponent(long key) : base(key)
        {
        }
    }
}

