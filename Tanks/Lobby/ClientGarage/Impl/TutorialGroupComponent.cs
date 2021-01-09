namespace Tanks.Lobby.ClientGarage.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [SerialVersionUID(0x15e2d9fb379L)]
    public class TutorialGroupComponent : GroupComponent
    {
        public TutorialGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public TutorialGroupComponent(long key) : base(key)
        {
        }
    }
}

