namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using Tanks.Lobby.ClientGarage.API;

    public class HangarTankBaseSystem : ECSSystem
    {
        public class HangarCameraNode : Node
        {
            public CameraComponent camera;
            public CameraRootTransformComponent cameraRootTransform;
            public HangarComponent hangar;
        }

        public class HangarNode : Node
        {
            public HangarTankPositionComponent hangarTankPosition;
            public HangarContainerPositionComponent hangarContainerPosition;
        }

        public class HangarPreviewItemNode : Node
        {
            public HangarItemPreviewComponent hangarItemPreview;
        }

        public class HullSkinItemPreviewLoadedNode : HangarTankBaseSystem.SkinItemPreviewLoadedNode
        {
            public HullSkinItemComponent hullSkinItem;
        }

        public class SkinItemPreviewLoadedNode : HangarTankBaseSystem.HangarPreviewItemNode
        {
            public ResourceDataComponent resourceData;
            public SkinItemComponent skinItem;
        }

        public class TankItemPreviewNode : HangarTankBaseSystem.HangarPreviewItemNode
        {
            public TankItemComponent tankItem;
            public ParentGroupComponent parentGroup;
        }

        public class WeaponItemPreviewNode : HangarTankBaseSystem.HangarPreviewItemNode
        {
            public WeaponItemComponent weaponItem;
            public ParentGroupComponent parentGroup;
        }

        public class WeaponSkinItemPreviewLoadedNode : HangarTankBaseSystem.SkinItemPreviewLoadedNode
        {
            public WeaponSkinItemComponent weaponSkinItem;
        }
    }
}

