namespace Tanks.Battle.ClientGraphics.Impl
{
    using LeopotamGroup.Pooling;
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class PoolContainerSystem : ECSSystem
    {
        [OnEventFire]
        public void CleanContainer(NodeRemoveEvent e, MapInstance map)
        {
            if (map.Entity.HasComponent<MainPoolContainerComponent>())
            {
                map.Entity.RemoveComponent<MainPoolContainerComponent>();
            }
        }

        [OnEventFire]
        public void GetInstanceFromContainer(GetInstanceFromPoolEvent e, Node any, [JoinAll] Optional<MapWithContainer> map)
        {
            if (!map.IsPresent())
            {
                e.Instance = Object.Instantiate<GameObject>(e.Prefab).transform;
                if (e.AutoRecycleTime > 0f)
                {
                    Object.Destroy(e.Instance.gameObject, e.AutoRecycleTime);
                }
            }
            else
            {
                PoolContainer container;
                MainPoolContainerComponent mainPoolContainer = map.Get().mainPoolContainer;
                if (!mainPoolContainer.PrefabToPoolDict.TryGetValue(e.Prefab, out container))
                {
                    Transform transform = new GameObject(e.Prefab.name + "_PoolContainer").transform;
                    transform.SetParent(mainPoolContainer.MainContainerTransform);
                    PoolContainer container2 = transform.gameObject.AddComponent<PoolContainer>();
                    container2.ItemsRoot = transform;
                    container2.CustomPrefab = e.Prefab;
                    container = container2;
                    mainPoolContainer.PrefabToPoolDict.Add(e.Prefab, container2);
                }
                Transform poolTransform = container.Get().PoolTransform;
                e.Instance = poolTransform;
                if (e.AutoRecycleTime > 0f)
                {
                    RecycleAfterTime component = poolTransform.gameObject.GetComponent<RecycleAfterTime>();
                    if (component)
                    {
                        component.Timeout = e.AutoRecycleTime;
                    }
                    else
                    {
                        poolTransform.gameObject.AddComponent<RecycleAfterTime>().Timeout = e.AutoRecycleTime;
                    }
                }
            }
        }

        [OnEventFire]
        public void InitContainer(NodeAddedEvent e, MapInstance instance)
        {
            GameObject obj2 = new GameObject("MainPoolContainer") {
                transform = { parent = instance.mapInstance.SceneRoot.transform }
            };
            MainPoolContainerComponent component = new MainPoolContainerComponent {
                MainContainerTransform = obj2.transform
            };
            instance.Entity.AddComponent(component);
        }

        public class MapInstance : Node
        {
            public MapInstanceComponent mapInstance;
        }

        public class MapWithContainer : PoolContainerSystem.MapInstance
        {
            public MainPoolContainerComponent mainPoolContainer;
        }
    }
}

