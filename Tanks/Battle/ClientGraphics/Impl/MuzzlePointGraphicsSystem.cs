namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class MuzzlePointGraphicsSystem : ECSSystem
    {
        [OnEventFire]
        public void AttachMuzzlePointToVisualRootForRemoteTank(NodeAddedEvent e, MuzzlePointInitNode weaponNode)
        {
            Transform transform = weaponNode.weaponVisualRoot.transform;
            foreach (Transform transform2 in weaponNode.muzzlePoint.Points)
            {
                transform2.parent = transform;
            }
        }

        public class MuzzlePointInitNode : Node
        {
            public MuzzlePointComponent muzzlePoint;
            public MuzzlePointInitializedComponent muzzlePointInitialized;
            public WeaponVisualRootComponent weaponVisualRoot;
        }
    }
}

