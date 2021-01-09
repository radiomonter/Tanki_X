namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Kernel.OSGi.ClientCore.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using Platform.System.Data.Statics.ClientConfigurator.API;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientLoading.API;

    public class AssetsFirstLoadingSystem : ECSSystem
    {
        private const string CONFIG_PATH = "clientlocal/clientresources/mandatoryassets";

        [OnEventFire]
        public void CheckAssetRequestIsValid(NodeAddedEvent e, [Combine] AssetRequestNode node, NotCompletedMandatoryAssetsLoadingNode mandatoryLoading)
        {
            if (!mandatoryLoading.mandatoryAssetsFirstLoading.IsAssetRequestMandatory(node.assetRequest))
            {
                base.Log.InfoFormat("MandatoryAssetsLoading: Unexpected asset {0} will be loaded as mandatory!", node.assetReference.Reference.AssetGuid);
            }
        }

        private static List<string> GetMandatoryAssetGUIDsFromConfig(long userId)
        {
            ConfigPathCollectionComponent component = ConfigurationService.GetConfig("clientlocal/clientresources/mandatoryassets").GetChildNode("configPathCollection").ConvertTo<ConfigPathCollectionComponent>();
            long num = userId % 3L;
            num = 1L;
            if ((num >= 1L) && (num <= 4L))
            {
                switch (((int) (num - 1L)))
                {
                    case 0:
                        return component.Collection1;

                    case 1:
                        return component.Collection2;

                    case 2:
                        return component.Collection3;

                    case 3:
                        return component.Collection4;

                    default:
                        break;
                }
            }
            return component.Collection;
        }

        [OnEventFire]
        public void Init(NodeAddedEvent e, SelfUserNode selfUser)
        {
            base.CreateEntity("MandatoryAssetsFirstLoading").AddComponent<MandatoryAssetsFirstLoadingComponent>();
            foreach (string str in GetMandatoryAssetGUIDsFromConfig(selfUser.Entity.Id))
            {
                AssetReferenceComponent component = new AssetReferenceComponent(new AssetReference(str));
                Entity entity2 = base.CreateEntity("Loader " + str);
                entity2.AddComponent(component);
                entity2.AddComponent<PreloadComponent>();
                entity2.AddComponent(new AssetRequestComponent(100));
            }
        }

        [OnEventFire]
        public void OnComplete(ClientEnterLobbyEvent e, Node node, [JoinAll] NotCompletedMandatoryAssetsLoadingNode mandatoryLoading)
        {
            mandatoryLoading.Entity.AddComponent<MandatoryAssetsFirstLoadingCompletedComponent>();
        }

        [OnEventFire]
        public void OnPreloadingComplete(NodeRemoveEvent e, SingleNode<PreloadAllResourcesComponent> lobbyLoadScreen, [JoinAll] SingleNode<SelfUserComponent> user)
        {
            base.ScheduleEvent<ClientEnterLobbyEvent>(user);
        }

        private void RequestAsset(Entity entity, MandatoryAssetsFirstLoadingComponent.MandatoryRequestsState requestsState, NotCompletedMandatoryAssetsLoadingNode mandatoryLoading)
        {
            AssetRequestComponent assetRequest = new AssetRequestComponent(100);
            mandatoryLoading.mandatoryAssetsFirstLoading.MarkAsRequested(assetRequest, requestsState);
            entity.AddComponent(assetRequest);
            this.ShowLoadingScreenIfAllAssetsAreRequired(mandatoryLoading);
        }

        [OnEventFire]
        public void RequestColoringAssets(NodeAddedEvent e, MountedTankPaintNode coloring, NotCompletedMandatoryAssetsLoadingNode mandatoryLoading)
        {
            this.RequestAsset(coloring.Entity, MandatoryAssetsFirstLoadingComponent.MandatoryRequestsState.TANK_COLORING, mandatoryLoading);
        }

        [OnEventFire]
        public void RequestColoringAssets(NodeAddedEvent e, MountedWeaponPaintNode coloring, NotCompletedMandatoryAssetsLoadingNode mandatoryLoading)
        {
            this.RequestAsset(coloring.Entity, MandatoryAssetsFirstLoadingComponent.MandatoryRequestsState.WEAPON_COLORING, mandatoryLoading);
        }

        [OnEventFire]
        public void RequestContainerAsset(NodeAddedEvent e, [Combine] ContainerNode container, NotCompletedMandatoryAssetsLoadingNode mandatoryLoading)
        {
            if (!mandatoryLoading.mandatoryAssetsFirstLoading.IsContainerRequested())
            {
                this.RequestAsset(container.Entity, MandatoryAssetsFirstLoadingComponent.MandatoryRequestsState.CONTAINER, mandatoryLoading);
            }
        }

        [OnEventFire]
        public void RequestHangarAssets(NodeAddedEvent e, SingleNode<SelfUserComponent> user, SingleNode<HangarAssetComponent> hangar, NotCompletedMandatoryAssetsLoadingNode mandatoryLoading)
        {
            hangar.Entity.AddComponent(new AssetReferenceComponent(new AssetReference(hangar.component.AssetGuid)));
            this.RequestAsset(hangar.Entity, MandatoryAssetsFirstLoadingComponent.MandatoryRequestsState.HANGAR, mandatoryLoading);
        }

        [OnEventFire]
        public void RequestHullSkinAssets(NodeAddedEvent e, MountedHullSkinNode hullSkin, NotCompletedMandatoryAssetsLoadingNode mandatoryLoading)
        {
            this.RequestAsset(hullSkin.Entity, MandatoryAssetsFirstLoadingComponent.MandatoryRequestsState.HULL_SKIN, mandatoryLoading);
        }

        [OnEventFire]
        public void RequestWeaponSkinAssets(NodeAddedEvent e, MountedWeaponSkinNode weaponSkin, NotCompletedMandatoryAssetsLoadingNode mandatoryLoading)
        {
            this.RequestAsset(weaponSkin.Entity, MandatoryAssetsFirstLoadingComponent.MandatoryRequestsState.WEAPON_SKIN, mandatoryLoading);
        }

        private void ShowLoadingScreenIfAllAssetsAreRequired(NotCompletedMandatoryAssetsLoadingNode mandatoryLoading)
        {
            if (mandatoryLoading.mandatoryAssetsFirstLoading.AllMandatoryAssetsAreRequested())
            {
                mandatoryLoading.mandatoryAssetsFirstLoading.LoadingScreenShown = true;
                base.ScheduleEvent<ShowScreenNoAnimationEvent<LobbyLoadScreenComponent>>(mandatoryLoading);
            }
        }

        [OnEventFire]
        public void ShowPreloadAllResourcesLoadScreen(NodeAddedEvent e, SingleNode<PreloadAllResourcesComponent> node)
        {
            base.NewEvent<ShowPreloadScreenDelayedEvent>().Attach(node).ScheduleDelayed(0.5f);
        }

        [OnEventFire]
        public void ShowPreloadAllResourcesLoadScreen(ShowPreloadScreenDelayedEvent e, SingleNode<PreloadAllResourcesComponent> node, [JoinAll] ICollection<SingleNode<PreloadComponent>> preloads)
        {
            if (preloads.Count > 0)
            {
                base.ScheduleEvent<ShowScreenNoAnimationEvent<PreloadAllResourcesScreenComponent>>(node);
            }
        }

        [OnEventFire]
        public void StartPreload(NodeAddedEvent e, [Combine] SingleNode<LoadProgressTaskCompleteComponent> loadTaskNode, SingleNode<UserReadyForLobbyComponent> user, [JoinAll] ICollection<SingleNode<LobbyLoadScreenComponent>> screen)
        {
            if ((screen.Count > 0) && !user.Entity.HasComponent<PreloadAllResourcesComponent>())
            {
                user.Entity.AddComponent<PreloadAllResourcesComponent>();
            }
        }

        [Inject]
        public static Platform.System.Data.Statics.ClientConfigurator.API.ConfigurationService ConfigurationService { get; set; }

        public class AssetRequestNode : Node
        {
            public AssetReferenceComponent assetReference;
            public AssetRequestComponent assetRequest;
        }

        public class ContainerNode : Node
        {
            public ContainerMarkerComponent containerMarker;
            public AssetReferenceComponent assetReference;
            public MarketItemComponent marketItem;
        }

        public class LobbyLoadCompletedNode : Node
        {
            public LoadProgressTaskCompleteComponent loadProgressTaskComplete;
            public LobbyLoadScreenComponent lobbyLoadScreen;
        }

        public class MountedHullSkinNode : Node
        {
            public UserItemComponent userItem;
            public MountedItemComponent mountedItem;
            public AssetReferenceComponent assetReference;
            public HullSkinItemComponent hullSkinItem;
        }

        public class MountedTankPaintNode : Node
        {
            public UserItemComponent userItem;
            public MountedItemComponent mountedItem;
            public AssetReferenceComponent assetReference;
            public TankPaintItemComponent tankPaintItem;
        }

        public class MountedWeaponPaintNode : Node
        {
            public UserItemComponent userItem;
            public MountedItemComponent mountedItem;
            public AssetReferenceComponent assetReference;
            public WeaponPaintItemComponent weaponPaintItem;
        }

        public class MountedWeaponSkinNode : Node
        {
            public UserItemComponent userItem;
            public MountedItemComponent mountedItem;
            public AssetReferenceComponent assetReference;
            public WeaponSkinItemComponent weaponSkinItem;
        }

        [Not(typeof(MandatoryAssetsFirstLoadingCompletedComponent))]
        public class NotCompletedMandatoryAssetsLoadingNode : Node
        {
            public MandatoryAssetsFirstLoadingComponent mandatoryAssetsFirstLoading;
        }

        public class SelfUserNode : Node
        {
            public SelfUserComponent selfUser;
        }

        public class ShowPreloadScreenDelayedEvent : Event
        {
        }
    }
}

