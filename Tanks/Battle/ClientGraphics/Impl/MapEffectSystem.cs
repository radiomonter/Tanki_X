namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class MapEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void InitMapEffect(NodeAddedEvent e, MapNode map)
        {
            Entity entity = base.CreateEntity("MapEffect");
            entity.AddComponent<MapEffectComponent>();
            map.mapGroup.Attach(entity);
            entity.AddComponent(new AssetReferenceComponent(map.mapInstance.SceneRoot.GetComponent<MapEffectReferenceBehaviour>().MapEffect));
            entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void PrepareMapEffect(NodeAddedEvent e, MapEffectLoadedNode mapEffect)
        {
            GameObject instance = Object.Instantiate<GameObject>((GameObject) mapEffect.resourceData.Data);
            GameObject obj4 = Object.Instantiate<GameObject>(instance.GetComponent<CommonMapEffectBehaviour>().CommonMapEffectPrefab);
            obj4.transform.SetParent(instance.transform);
            obj4.transform.localPosition = Vector3.zero;
            obj4.transform.localRotation = Quaternion.identity;
            instance.GetComponent<EntityBehaviour>().BuildEntity(mapEffect.Entity);
            mapEffect.Entity.AddComponent(new MapEffectInstanceComponent(instance));
        }

        [OnEventFire]
        public void PrepareMapEffect(NodeAddedEvent e, ReadyMapEffectNode mapEffect)
        {
            mapEffect.Entity.AddComponent<MapEffectAssembledComponent>();
        }

        [OnEventFire]
        public void RemoveMapEffect(NodeRemoveEvent e, SingleNode<MapEffectInstanceComponent> mapEffect)
        {
            Object.DestroyObject(mapEffect.component.Instance);
        }

        [OnEventFire]
        public void RemoveMapEffect(NodeRemoveEvent e, SingleNode<MapInstanceComponent> map, [JoinByMap] SingleNode<MapEffectComponent> mapEffect)
        {
            base.DeleteEntity(mapEffect.Entity);
        }

        public class MapEffectLoadedNode : Node
        {
            public MapEffectComponent mapEffect;
            public ResourceDataComponent resourceData;
            public MapGroupComponent mapGroup;
        }

        public class MapNode : Node
        {
            public MapInstanceComponent mapInstance;
            public MapGroupComponent mapGroup;
        }

        public class ReadyMapEffectNode : MapEffectSystem.MapEffectLoadedNode
        {
            public PreloadingModuleEffectsComponent preloadingModuleEffects;
            public PreloadedModuleEffectsComponent preloadedModuleEffects;
            public MapEffectInstanceComponent mapEffectInstance;
        }
    }
}

