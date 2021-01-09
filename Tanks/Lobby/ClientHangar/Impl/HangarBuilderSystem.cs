namespace Tanks.Lobby.ClientHangar.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Lobby.ClientEntrance.API;
    using Tanks.Lobby.ClientGarage.API;
    using Tanks.Lobby.ClientHangar.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class HangarBuilderSystem : ECSSystem
    {
        [OnEventFire]
        public void CleanHangarOnBattle(NodeAddedEvent e, ReadyToBattleUser battleUser, [JoinAll] SingleNode<HangarAssetComponent> hangar)
        {
            base.ScheduleEvent<CleanHangarEvent>(hangar);
        }

        [OnEventFire]
        public void CleanUpHangarWhenUnloading(CleanHangarEvent evt, SingleNode<AssetRequestComponent> hangarScene)
        {
            hangarScene.Entity.RemoveComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void CleanUpHangarWhenUnloading(CleanHangarEvent evt, SingleNode<ResourceDataComponent> hangarScene)
        {
            hangarScene.Entity.RemoveComponent<ResourceDataComponent>();
        }

        [OnEventFire]
        public void CleanUpHangarWhenUnloading(CleanHangarEvent evt, SingleNode<HangarInstanceComponent> hangarScene)
        {
            hangarScene.Entity.RemoveComponent<HangarInstanceComponent>();
        }

        [OnEventFire]
        public void CleanUpHangarWhenUnloading(CleanHangarEvent evt, SingleNode<HangarLocationsComponent> hangarScene)
        {
            hangarScene.Entity.RemoveComponent<HangarLocationsComponent>();
        }

        [OnEventFire]
        public void HideScreenForeground(NodeRemoveEvent e, InstantiatedHangarNode node, [JoinAll] SingleNode<ScreenForegroundComponent> screenForeground)
        {
            base.ScheduleEvent<ForceHideScreenForegroundEvent>(screenForeground);
        }

        [OnEventComplete]
        public void InitHangarScene(NodeAddedEvent e, SingleNode<HangarSceneLoadedMarkerComponent> hangarSceneLoadedMarker, [JoinAll, Mandatory] HangarResourceNode hangar)
        {
            GameObject gameObject = hangarSceneLoadedMarker.component.transform.parent.gameObject;
            gameObject.GetComponent<EntityBehaviour>().BuildEntity(hangar.Entity);
            HangarLocationsComponent component = new HangarLocationsComponent {
                Locations = new Dictionary<HangarLocation, Transform>()
            };
            foreach (HangarLocationBehaviour behaviour2 in gameObject.GetComponentsInChildren<HangarLocationBehaviour>(true))
            {
                component.Locations.Add(behaviour2.HangarLocation, behaviour2.transform);
            }
            hangar.Entity.AddComponent(component);
            hangar.Entity.AddComponent(new HangarInstanceComponent(gameObject));
            Object.Destroy(hangarSceneLoadedMarker.component.gameObject);
        }

        [OnEventComplete]
        public void LoadHangarResourcesOnBattleExit(NodeRemoveEvent e, SingleNode<SelfBattleUserComponent> selfBattleUser, [JoinAll] SingleNode<HangarAssetComponent> hangar, [JoinAll] ICollection<SingleNode<MapComponent>> maps)
        {
            maps.ForEach<SingleNode<MapComponent>>(m => base.ScheduleEvent<CleanMapEvent>(m.Entity));
            if (!hangar.Entity.HasComponent<AssetRequestComponent>())
            {
                hangar.Entity.AddComponent(new AssetRequestComponent(-100));
            }
        }

        [OnEventFire]
        public void LoadHangarResourcesOnBattleExit(LoadHangarEvent e, Node any, [JoinAll] SingleNode<HangarAssetComponent> hangar, [JoinAll] ICollection<SingleNode<MapComponent>> maps)
        {
            maps.ForEach<SingleNode<MapComponent>>(m => base.ScheduleEvent<CleanMapEvent>(m.Entity));
            if (!hangar.Entity.HasComponent<AssetRequestComponent>())
            {
                hangar.Entity.AddComponent(new AssetRequestComponent(-100));
            }
        }

        [OnEventFire]
        public void LoadHangarScene(NodeAddedEvent e, HangarResourceNode hangar, SingleNode<SoundListenerResourcesComponent> readySoundListener)
        {
            this.MarkAllGameObjectsAsUnloadedExceptMap();
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(hangar.resourceData.Name);
            base.ScheduleEvent(new LoadSceneEvent(fileNameWithoutExtension, hangar.resourceData.Data), hangar);
        }

        private void MarkAllGameObjectsAsUnloadedExceptMap()
        {
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                Scene sceneAt = SceneManager.GetSceneAt(i);
                if (sceneAt.isLoaded)
                {
                    foreach (GameObject obj2 in sceneAt.GetRootGameObjects())
                    {
                        if (!ImportantGameObjectsNames.MAP_ROOT.Equals(obj2.name.ToLower()))
                        {
                            Object.DontDestroyOnLoad(obj2);
                        }
                    }
                }
            }
        }

        [OnEventFire]
        public void PrepareForHangarSceneUnloading(NodeRemoveEvent e, HangarSceneNode hangarScene)
        {
            base.ScheduleEvent<CleanHangarEvent>(hangarScene);
        }

        [OnEventFire]
        public void UnloadUnusedResources(NodeAddedEvent e, SingleNode<HangarSceneLoadedMarkerComponent> hangarSceneLoadedMarker, [JoinAll, Mandatory] HangarResourceNode hangar)
        {
            base.ScheduleEvent<UnloadUnusedAssetsEvent>(hangar);
        }

        public class HangarResourceNode : Node
        {
            public HangarAssetComponent hangarAsset;
            public ResourceDataComponent resourceData;
        }

        public class HangarSceneNode : Node
        {
            public HangarComponent hangar;
            public CurrentSceneComponent currentScene;
        }

        public class InstantiatedHangarNode : Node
        {
            public ResourceDataComponent resourceData;
            public HangarInstanceComponent hangarInstance;
        }

        public class ReadyToBattleUser : Node
        {
            public SelfBattleUserComponent selfBattleUser;
            public UserReadyToBattleComponent userReadyToBattle;
        }

        public class ReadyToLobbyUser : Node
        {
            public SelfUserComponent selfUser;
            public UserReadyForLobbyComponent userReadyForLobby;
        }
    }
}

