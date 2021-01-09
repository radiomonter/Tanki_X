namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientGarage.Impl;
    using UnityEngine;

    public class HangarContainerBuilderSystem : HangarTankBaseSystem
    {
        private void BuildContainer(Transform containerPosition, Object containerPrefab, Transform hangarCamera)
        {
            containerPosition.DestroyChildren();
            GameObject hull = (GameObject) Object.Instantiate(containerPrefab);
            hull.transform.SetParent(containerPosition.transform);
            hull.transform.localPosition = Vector3.zero;
            hull.transform.localRotation = Quaternion.identity;
            PhysicsUtil.SetGameObjectLayer(containerPosition.gameObject, Layers.HANGAR);
            Renderer containerRenderer = TankBuilderUtil.GetContainerRenderer(hull);
            BurningTargetBloom componentInChildren = hangarCamera.GetComponentInChildren<BurningTargetBloom>();
            componentInChildren.targets.Clear();
            componentInChildren.targets.Add(containerRenderer);
        }

        [OnEventFire]
        public void BuildContainer(NodeAddedEvent e, HangarTankBaseSystem.HangarNode hangar, ContainerItemPreviewLoadedNode container, HangarTankBaseSystem.HangarCameraNode hangarCamera, SingleNode<MainScreenComponent> screen)
        {
            screen.component.HideNewItemNotification();
            Transform containerPosition = hangar.hangarContainerPosition.transform;
            ContainerComponent componentInChildren = hangar.hangarContainerPosition.GetComponentInChildren<ContainerComponent>();
            if ((componentInChildren != null) && (componentInChildren.assetGuid == container.assetReference.Reference.AssetGuid))
            {
                base.ScheduleEvent<HangarContainerBuildedEvent>(hangar);
            }
            else
            {
                this.BuildContainer(containerPosition, container.resourceData.Data, hangarCamera.cameraRootTransform.Root);
                containerPosition.GetComponentInChildren<ContainerComponent>().assetGuid = container.assetReference.Reference.AssetGuid;
                base.ScheduleEvent<HangarContainerBuildedEvent>(hangar);
            }
        }

        [OnEventFire]
        public void DestroyHangarContainer(NodeRemoveEvent e, ContainersScreenNode screen, [JoinAll] ICollection<SingleNode<ContainerMarkerComponent>> containers, [JoinAll] SingleNode<HangarContainerPositionComponent> containerPosition)
        {
            if (containers.Count == 0)
            {
                containerPosition.component.transform.DestroyChildren();
            }
        }

        public class ContainerItemPreviewLoadedNode : HangarTankBaseSystem.HangarPreviewItemNode
        {
            public ContainerMarkerComponent containerMarker;
            public AssetReferenceComponent assetReference;
            public ResourceDataComponent resourceData;
        }

        public class ContainersScreenNode : Node
        {
            public ContainersScreenComponent containersScreen;
            public ActiveScreenComponent activeScreen;
        }

        [Not(typeof(ResourceDataComponent)), Not(typeof(AssetRequestComponent))]
        public class NotLoadedContainerItemNode : Node
        {
            public ContainerMarkerComponent containerMarker;
            public AssetReferenceComponent assetReference;
        }
    }
}

