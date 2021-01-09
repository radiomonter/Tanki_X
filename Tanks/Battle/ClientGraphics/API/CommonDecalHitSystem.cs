namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class CommonDecalHitSystem : AbstractDecalHitSystem
    {
        [OnEventFire]
        public void DrawBulletHitDecal(BulletHitEvent evt, Node node, [JoinByTank] SingleNode<DynamicDecalProjectorComponent> decalHitNode, [JoinByTank] SingleNode<MuzzlePointComponent> muzzlePointNode, [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode)
        {
            if (decalManagerNode.component.EnableDecals && (evt is BulletStaticHitEvent))
            {
                DynamicDecalProjectorComponent decalProjector = decalHitNode.component;
                Vector3 barrelOriginWorld = new MuzzleVisualAccessor(muzzlePointNode.component).GetBarrelOriginWorld();
                base.DrawHitDecal(decalManagerNode.component, decalProjector, barrelOriginWorld, (evt.Position - barrelOriginWorld).normalized);
            }
        }

        [OnEventFire]
        public void DrawHitDecal(HitEvent evt, SingleNode<DynamicDecalProjectorComponent> decalProjectorNode, [JoinByTank] SingleNode<MuzzlePointComponent> muzzlePointNode, [JoinAll] SingleNode<DecalManagerComponent> decalManagerNode)
        {
            if (decalManagerNode.component.EnableDecals)
            {
                base.DrawHitDecal(evt, decalManagerNode.component, decalProjectorNode.component, muzzlePointNode.component);
            }
        }

        [OnEventFire]
        public void Init(NodeAddedEvent evt, SingleNode<MapInstanceComponent> mapInstance, SingleNode<DecalSettingsComponent> settingsNode)
        {
            DecalSettingsComponent component = settingsNode.component;
            DecalManagerComponent component2 = new DecalManagerComponent {
                DecalMeshBuilder = new DecalMeshBuilder()
            };
            component2.DecalMeshBuilder.WarmupMeshCaches(mapInstance.component.SceneRoot);
            component2.BulletHoleDecalManager = new BulletHoleDecalManager(mapInstance.component.SceneRoot, component.MaxCount, component.LifeTimeMultipler, component2.DecalsQueue);
            component2.EnableDecals = component.EnableDecals;
            mapInstance.Entity.AddComponent(component2);
        }

        [OnEventFire]
        public void Release(NodeRemoveEvent evt, SingleNode<MapInstanceComponent> mapInstance)
        {
            mapInstance.Entity.RemoveComponent<DecalManagerComponent>();
        }
    }
}

