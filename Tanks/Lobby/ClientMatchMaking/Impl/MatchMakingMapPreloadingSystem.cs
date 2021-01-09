namespace Tanks.Lobby.ClientMatchMaking.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientBattleSelect.API;

    public class MatchMakingMapPreloadingSystem : ECSSystem
    {
        [OnEventFire]
        public void CancelMapPreloadOnLobbyDroped(NodeRemoveEvent e, MatchMakingLobbyNodeWithMapPreload lobby)
        {
            if (lobby.mapPreloadOnLobby.LoaderEntity.Alive)
            {
                base.DeleteEntity(lobby.mapPreloadOnLobby.LoaderEntity);
            }
        }

        [OnEventFire]
        public void MapPreloadOnLobbyCreated(NodeAddedEvent e, MatchMakingLobbyNode lobby, [JoinByMap] MapNode map)
        {
            lobby.Entity.RemoveComponentIfPresent<MapPreloadOnLobbyComponent>();
            AssetRequestEvent eventInstance = new AssetRequestEvent {
                AssetGuid = map.assetReference.Reference.AssetGuid
            };
            base.ScheduleEvent(eventInstance, map);
            MapPreloadOnLobbyComponent component = new MapPreloadOnLobbyComponent {
                LoaderEntity = eventInstance.LoaderEntity
            };
            lobby.Entity.AddComponent(component);
        }

        public class MapNode : Node
        {
            public MapComponent map;
            public MapGroupComponent mapGroup;
            public AssetReferenceComponent assetReference;
        }

        public class MapPreloadOnLobbyComponent : Component
        {
            public Entity LoaderEntity;
        }

        public class MatchMakingLobbyNode : Node
        {
            public BattleLobbyComponent battleLobby;
            public MapGroupComponent mapGroup;
        }

        public class MatchMakingLobbyNodeWithMapPreload : Node
        {
            public MatchMakingMapPreloadingSystem.MapPreloadOnLobbyComponent mapPreloadOnLobby;
        }
    }
}

