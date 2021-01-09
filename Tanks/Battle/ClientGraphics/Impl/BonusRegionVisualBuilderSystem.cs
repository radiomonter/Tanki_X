namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class BonusRegionVisualBuilderSystem : ECSSystem
    {
        private const string TERRAIN_TAG = "Terrain";

        [OnEventFire]
        public void Destroy(NodeRemoveEvent e, InstantiatedBonusRegionNode bonusRegion)
        {
            bonusRegion.bonusRegionInstance.BonusRegionInstance.RecycleObject();
        }

        [OnEventFire]
        public void PlaceRegions(NodeAddedEvent e, [Combine] BonusRegionBuildNode region, SingleNode<MapInstanceComponent> map)
        {
            if (region.resourceData.Data != null)
            {
                GetInstanceFromPoolEvent eventInstance = new GetInstanceFromPoolEvent {
                    Prefab = region.resourceData.Data as GameObject
                };
                base.ScheduleEvent(eventInstance, region);
                Transform instance = eventInstance.Instance;
                instance.position = region.spatialGeometry.Position;
                instance.rotation = Quaternion.Euler(region.spatialGeometry.Rotation);
                GameObject gameObject = instance.gameObject;
                gameObject.SetActive(true);
                Animation component = gameObject.GetComponent<Animation>();
                if (component != null)
                {
                    component[component.clip.name].normalizedTime = Random.value;
                }
                region.Entity.AddComponent(new BonusRegionInstanceComponent(gameObject));
            }
        }

        [OnEventFire]
        public void RequestRegionPrefabs(NodeAddedEvent e, SingleNode<BonusRegionAssetComponent> region)
        {
            region.Entity.AddComponent(new AssetReferenceComponent(new AssetReference(region.component.AssetGuid)));
            region.Entity.AddComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void SetAsset(NodeAddedEvent e, [Combine] SingleNode<BonusRegionComponent> region, SingleNode<BonusRegionAssetsComponent> regionAssets)
        {
            string repairKitAssetGuid;
            BonusRegionAssetsComponent component = regionAssets.component;
            switch (region.component.Type)
            {
                case BonusType.REPAIR:
                    repairKitAssetGuid = component.RepairKitAssetGuid;
                    break;

                case BonusType.ARMOR:
                    repairKitAssetGuid = component.DoubleArmorAssetGuid;
                    break;

                case BonusType.DAMAGE:
                    repairKitAssetGuid = component.DoubleDamageAssetGuid;
                    break;

                case BonusType.SPEED:
                    repairKitAssetGuid = component.SpeedBoostAssetGuid;
                    break;

                case BonusType.GOLD:
                    repairKitAssetGuid = component.GoldAssetGuid;
                    break;

                default:
                    throw new UnknownRegionTypeException(region.component.Type);
            }
            if (region.Entity.HasComponent<BonusRegionAssetComponent>())
            {
                region.Entity.GetComponent<BonusRegionAssetComponent>().AssetGuid = repairKitAssetGuid;
            }
            else
            {
                region.Entity.AddComponent(new BonusRegionAssetComponent(repairKitAssetGuid));
            }
            if (region.component.Type != BonusType.GOLD)
            {
                region.Entity.AddComponentIfAbsent<VisibleBonusRegionComponent>();
            }
        }

        public class BonusRegionBuildNode : Node
        {
            public BonusRegionComponent bonusRegion;
            public ResourceDataComponent resourceData;
            public SpatialGeometryComponent spatialGeometry;
        }

        public class InstantiatedBonusRegionNode : Node
        {
            public BonusRegionComponent bonusRegion;
            public BonusRegionInstanceComponent bonusRegionInstance;
        }
    }
}

