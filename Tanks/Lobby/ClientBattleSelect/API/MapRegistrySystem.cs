namespace Tanks.Lobby.ClientBattleSelect.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientResources.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Lobby.ClientLoading.Impl;

    public class MapRegistrySystem : ECSSystem, MapRegistry
    {
        private Dictionary<Entity, Map> maps = new Dictionary<Entity, Map>();

        public Map GetMap(Entity mapEntity)
        {
            if (!this.maps.ContainsKey(mapEntity))
            {
                this.maps.Add(mapEntity, new Map(mapEntity));
            }
            return this.maps[mapEntity];
        }

        [OnEventFire]
        public void RequestMapLoadPreview(NodeAddedEvent e, MapNode mapNode)
        {
            if (this.GetMap(mapNode.Entity).LoadPreview == null)
            {
                AssetRequestEvent eventInstance = new AssetRequestEvent();
                eventInstance.Init<MapLoadPreviewDataComponent>(mapNode.mapLoadPreview.AssetGuid);
                base.ScheduleEvent(eventInstance, mapNode);
            }
        }

        [Not(typeof(MapLoadPreviewDataComponent))]
        public class MapNode : Node
        {
            public MapComponent map;
            public MapLoadPreviewComponent mapLoadPreview;
        }
    }
}

