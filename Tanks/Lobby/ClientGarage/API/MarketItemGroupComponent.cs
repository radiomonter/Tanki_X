namespace Tanks.Lobby.ClientGarage.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0xe0daa42f809233L)]
    public class MarketItemGroupComponent : GroupComponent
    {
        public MarketItemGroupComponent(Entity keyEntity) : base(keyEntity)
        {
        }

        public MarketItemGroupComponent(long key) : base(key)
        {
        }
    }
}

