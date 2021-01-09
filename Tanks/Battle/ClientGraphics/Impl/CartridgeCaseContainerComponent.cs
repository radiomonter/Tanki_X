namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientProtocol.API;
    using System;
    using System.Collections.Generic;

    [SerialVersionUID(0x8d2e6e1268b00baL)]
    public class CartridgeCaseContainerComponent : Component
    {
        public readonly Queue<GameObject> Cartridges = new Queue<GameObject>();
    }
}

