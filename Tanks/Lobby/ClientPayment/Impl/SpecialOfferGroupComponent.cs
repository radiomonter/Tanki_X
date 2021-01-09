namespace Tanks.Lobby.ClientPayment.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;

    [Shared, SerialVersionUID(0x8d427a10edcb38eL)]
    public class SpecialOfferGroupComponent : GroupComponent
    {
        public SpecialOfferGroupComponent(Entity keyEntity) : this(keyEntity.Id)
        {
        }

        public SpecialOfferGroupComponent(long key) : base(key)
        {
        }
    }
}

