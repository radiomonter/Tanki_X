namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class MuzzlePointSystem : ECSSystem
    {
        public const string MUZZLE_POINT_NAME = "muzzle_point";

        [OnEventFire]
        public void CreateMuzzlePoint(NodeAddedEvent e, SingleNode<WeaponVisualRootComponent> weaponVisualNode)
        {
            List<Transform> list = new List<Transform>();
            Transform item = weaponVisualNode.component.transform;
            if (item.name == "muzzle_point")
            {
                list.Add(item);
            }
            IEnumerator enumerator = item.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    Transform current = (Transform) enumerator.Current;
                    if (current.name == "muzzle_point")
                    {
                        list.Add(current);
                    }
                }
            }
            finally
            {
                IDisposable disposable = enumerator as IDisposable;
                if (disposable != null)
                {
                    disposable.Dispose();
                }
            }
            MuzzlePointComponent component = new MuzzlePointComponent {
                Points = list.ToArray()
            };
            weaponVisualNode.Entity.AddComponent(component);
            weaponVisualNode.Entity.AddComponent<MuzzlePointInitializedComponent>();
        }
    }
}

