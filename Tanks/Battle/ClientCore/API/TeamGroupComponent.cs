namespace Tanks.Battle.ClientCore.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0x6087fe9a8bac3bcaL)]
    public class TeamGroupComponent : GroupComponent
    {
        public TeamGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public TeamGroupComponent(long key) : base(key)
        {
        }
    }
}

