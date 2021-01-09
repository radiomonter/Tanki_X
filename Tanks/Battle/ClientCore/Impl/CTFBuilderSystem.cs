namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;

    public class CTFBuilderSystem : ECSSystem
    {
        [OnEventFire]
        public void LoadResources(NodeAddedEvent e, CTFNode ctf)
        {
            ctf.Entity.AddComponent<AssetReferenceComponent>();
            ctf.Entity.AddComponent<AssetRequestComponent>();
        }

        public class CTFNode : Node
        {
            public CTFComponent ctf;
        }
    }
}

