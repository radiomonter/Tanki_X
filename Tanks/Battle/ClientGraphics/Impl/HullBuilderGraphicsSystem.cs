namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class HullBuilderGraphicsSystem : ECSSystem
    {
        [OnEventFire]
        public void OnNodeAdded(NodeAddedEvent evt, HullGraphicsNode hullGraphics)
        {
            Entity entity = hullGraphics.Entity;
            GameObject hullInstance = hullGraphics.hullInstance.HullInstance;
            hullInstance.GetComponent<EntityBehaviour>().BuildEntity(entity);
            BaseRendererComponent component = new BaseRendererComponent {
                Renderer = TankBuilderUtil.GetHullRenderer(hullInstance)
            };
            component.Mesh = (component.Renderer as SkinnedMeshRenderer).sharedMesh;
            entity.AddComponent<StartMaterialsComponent>();
            entity.AddComponent(component);
            TrackRendererComponent component3 = new TrackRendererComponent {
                Renderer = component.Renderer
            };
            entity.AddComponent(component3);
            base.ScheduleEvent<ChassisInitEvent>(entity);
        }

        public class HullGraphicsNode : Node
        {
            public HullInstanceComponent hullInstance;
        }
    }
}

