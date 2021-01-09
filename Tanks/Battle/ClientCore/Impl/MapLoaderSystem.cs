namespace Tanks.Battle.ClientCore.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientDataStructures.API;
    using Platform.Library.ClientResources.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientLoading.API;
    using Tanks.Lobby.ClientNavigation.API;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class MapLoaderSystem : ECSSystem
    {
        [OnEventFire]
        public void CleanMap(CleanMapEvent e, SingleNode<AssetRequestComponent> map)
        {
            map.Entity.RemoveComponent<AssetRequestComponent>();
        }

        [OnEventFire]
        public void CleanMap(CleanMapEvent e, SingleNode<ResourceDataComponent> map)
        {
            map.Entity.RemoveComponent<ResourceDataComponent>();
        }

        [OnEventFire]
        public void CleanMap(CleanMapEvent e, SingleNode<MapInstanceComponent> map)
        {
            map.Entity.RemoveComponent<MapInstanceComponent>();
        }

        [OnEventFire]
        public void CleanMaps(NodeAddedEvent e, SingleNode<RoundRestartingStateComponent> round, [JoinAll] ICollection<SingleNode<MapComponent>> maps)
        {
            maps.ForEach<SingleNode<MapComponent>>(new Action<SingleNode<MapComponent>>(this.ScheduleEvent<CleanMapEvent>));
        }

        [OnEventComplete]
        public void InitMap(NodeAddedEvent e, SingleNode<MapSceneLoadedMarkerComponent> mapSceneLoadedMarker)
        {
            base.ScheduleEvent<UnloadUnusedAssetsEvent>(mapSceneLoadedMarker);
        }

        [OnEventFire]
        public void InitMap(NodeAddedEvent e, SingleNode<MapSceneLoadedMarkerComponent> mapSceneLoadedMarker, [JoinAll, Context] LoadedMapNode map)
        {
            GameObject gameObject = mapSceneLoadedMarker.component.transform.parent.gameObject;
            if (!gameObject)
            {
                throw new CannotFindMapRootException(map.resourceData.Name);
            }
            EntityBehaviour behaviour = gameObject.AddComponent<EntityBehaviour>();
            behaviour.handleAutomaticaly = false;
            behaviour.BuildEntity(map.Entity);
            map.Entity.AddComponent(new MapInstanceComponent(gameObject));
        }

        [OnEventComplete]
        public void LoadMapResources(NodeAddedEvent e, BattleUserNode user, [JoinByBattle, Context] BattleNode battle, [JoinByMap, Context] MapNode map, [JoinAll] ICollection<SingleNode<MapComponent>> maps)
        {
            <LoadMapResources>c__AnonStorey0 storey = new <LoadMapResources>c__AnonStorey0 {
                map = map,
                $this = this
            };
            maps.ForEach<SingleNode<MapComponent>>(new Action<SingleNode<MapComponent>>(storey.<>m__0));
            storey.map.Entity.AddComponent(new AssetRequestComponent(-100));
        }

        [OnEventFire]
        public void LoadMapScene(NodeAddedEvent e, LoadedMapNode map, [Context] BattleLoadScreenNode screen)
        {
            this.MarkAllObjectsAsUnloadedExceptHangar();
            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(map.resourceData.Name);
            base.ScheduleEvent(new LoadSceneEvent(fileNameWithoutExtension, map.resourceData.Data), map);
        }

        private void MarkAllObjectsAsUnloadedExceptHangar()
        {
            int sceneCount = SceneManager.sceneCount;
            for (int i = 0; i < sceneCount; i++)
            {
                Scene sceneAt = SceneManager.GetSceneAt(i);
                if (sceneAt.isLoaded && !SceneNames.HANGAR.Equals(sceneAt.name))
                {
                    foreach (GameObject obj2 in sceneAt.GetRootGameObjects())
                    {
                        Object.DontDestroyOnLoad(obj2);
                    }
                }
            }
        }

        [OnEventFire, Mandatory]
        public void PrepareToMapSceneUnloading(NodeRemoveEvent e, MapSceneNode map)
        {
            base.ScheduleEvent<CleanMapEvent>(map);
        }

        [CompilerGenerated]
        private sealed class <LoadMapResources>c__AnonStorey0
        {
            internal MapLoaderSystem.MapNode map;
            internal MapLoaderSystem $this;

            internal void <>m__0(SingleNode<MapComponent> m)
            {
                if (!m.Entity.Equals(this.map.Entity))
                {
                    this.$this.ScheduleEvent<CleanMapEvent>(m);
                }
            }
        }

        public class BattleLoadScreenNode : Node
        {
            public BattleLoadScreenComponent battleLoadScreen;
            public BattleLoadScreenReadyToHideComponent battleLoadScreenReadyToHide;
        }

        public class BattleNode : Node
        {
            public MapGroupComponent mapGroup;
            public BattleComponent battle;
            public BattleGroupComponent battleGroup;
        }

        public class BattleUserNode : Node
        {
            public BattleGroupComponent battleGroup;
            public SelfBattleUserComponent selfBattleUser;
        }

        public class LoadedMapNode : Node
        {
            public MapComponent map;
            public ResourceDataComponent resourceData;
        }

        public class MapNode : Node
        {
            public MapComponent map;
            public AssetReferenceComponent assetReference;
            public MapGroupComponent mapGroup;
        }

        public class MapNodeWithRequest : Node
        {
            public MapComponent map;
            public AssetRequestComponent assetRequest;
        }

        public class MapSceneNode : Node
        {
            public MapComponent map;
            public CurrentSceneComponent currentScene;
        }
    }
}

