namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class HangarGraffitiBuilderSystem : ECSSystem
    {
        [OnEventFire]
        public void OnExitGraffitiScreen(NodeRemoveEvent e, ActiveGraffitiScreenNode activeGraffitiScreen, [JoinAll] HangarCameraNode hangarCameraNode)
        {
            base.ScheduleEvent<HangarGraffitiBuildedEvent>(hangarCameraNode);
        }

        [OnEventComplete]
        public void OnGraffitiEquip(NodeAddedEvent e, GraffitiItemPreviewNode graffiti, ActiveGraffitiScreenNode activeGraffitiScreen, [JoinAll] HangarCameraNode hangarCameraNode)
        {
            DynamicDecalProjectorComponent component = (graffiti.resourceData.Data as GameObject).GetComponent<DynamicDecalProjectorComponent>();
            activeGraffitiScreen.graffitiPreview.SetPreview(component.Material.mainTexture);
            base.ScheduleEvent<HangarGraffitiBuildedEvent>(hangarCameraNode);
        }

        public class ActiveGraffitiScreenNode : Node
        {
            public ActiveScreenComponent activeScreen;
            public GraffitiPreviewComponent graffitiPreview;
        }

        public class GraffitiItemPreviewNode : Node
        {
            public HangarItemPreviewComponent hangarItemPreview;
            public GraffitiItemComponent graffitiItem;
            public ResourceDataComponent resourceData;
        }

        public class HangarCameraNode : Node
        {
            public HangarCameraComponent hangarCamera;
            public CameraComponent camera;
        }
    }
}

