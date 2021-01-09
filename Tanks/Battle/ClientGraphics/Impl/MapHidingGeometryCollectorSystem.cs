namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Tool.BakedTrees.API;
    using System;
    using System.Linq;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class MapHidingGeometryCollectorSystem : ECSSystem
    {
        [OnEventFire]
        public void Clean(NodeRemoveEvent evt, SingleNode<MapInstanceComponent> map, [Combine, JoinAll] SingleNode<MapHidingGeometryComponent> hider)
        {
            base.DeleteEntity(hider.Entity);
        }

        [OnEventFire]
        public void CollectHidingGeometry(NodeAddedEvent evt, SingleNode<MapInstanceComponent> map)
        {
            foreach (HidingGeomentryRootBehaviour behaviour in Object.FindObjectsOfType<HidingGeomentryRootBehaviour>())
            {
                Renderer[] hidingRenderers = (behaviour == null) ? new Renderer[0] : behaviour.gameObject.GetComponentsInChildren<Renderer>(true).Where<Renderer>(new Func<Renderer, bool>(this.IsBillboardRendererNotShadow)).ToArray<Renderer>();
                base.CreateEntity("Foliage hider").AddComponent(new MapHidingGeometryComponent(hidingRenderers));
            }
        }

        [OnEventFire]
        public void InitializeShadowsSettingsOnBillboardTrees(NodeAddedEvent e, SingleNode<MapInstanceComponent> map, SingleNode<CameraComponent> cameraNode)
        {
            foreach (BillboardTreeMarkerBehaviour behaviour in map.component.SceneRoot.GetComponentsInChildren<BillboardTreeMarkerBehaviour>(true))
            {
                behaviour.billboardRenderer.receiveShadows = GraphicsSettings.INSTANCE.CurrentTreesShadowRecieving;
                behaviour.billboardTreeShadowMarker.gameObject.SetActive(GraphicsSettings.INSTANCE.CurrentBillboardTreesShadowCasting);
            }
        }

        private bool IsBillboardRendererNotShadow(Renderer renderer) => 
            renderer.gameObject.GetComponent<BillboardTreeShadowMarkerBehaviour>() == null;

        [OnEventFire]
        public void SetFarFoliageVisible(NodeAddedEvent e, SingleNode<MapInstanceComponent> map, SingleNode<CameraComponent> cameraNode)
        {
            FarFoliageRootBehaviour behaviour = Object.FindObjectOfType<FarFoliageRootBehaviour>();
            if (behaviour != null)
            {
                behaviour.gameObject.SetActive(GraphicsSettings.INSTANCE.CurrentFarFoliageEnabled);
            }
        }
    }
}

