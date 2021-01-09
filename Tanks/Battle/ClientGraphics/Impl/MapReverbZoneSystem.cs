namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class MapReverbZoneSystem : ECSSystem
    {
        [OnEventFire]
        public void FinalizeAmbientMapSoundEffect(LobbyAmbientSoundPlayEvent evt, MapReverbZoneListenerNode listener)
        {
            Object.DestroyObject(listener.mapReverbZone.ReverbZoneRoot);
            listener.Entity.RemoveComponent<MapReverbZoneComponent>();
        }

        [OnEventFire]
        public void InitReverbZones(NodeAddedEvent evt, NonMapReverbZoneListenerNode listener, MapNode map, [JoinByMap, Context] MapEffectNode mapEffect)
        {
            Transform transform = map.mapInstance.SceneRoot.transform;
            GameObject reverbZoneRoot = Object.Instantiate<GameObject>(mapEffect.mapReverbZoneAsset.MapReverbZonesRoot);
            Object.DontDestroyOnLoad(reverbZoneRoot.gameObject);
            Transform transform2 = reverbZoneRoot.transform;
            transform2.position = transform.position;
            transform2.rotation = transform.rotation;
            transform2.localScale = Vector3.one;
            listener.Entity.AddComponent(new MapReverbZoneComponent(reverbZoneRoot));
        }

        public class MapEffectNode : Node
        {
            public MapReverbZoneAssetComponent mapReverbZoneAsset;
            public MapGroupComponent mapGroup;
        }

        public class MapNode : Node
        {
            public MapInstanceComponent mapInstance;
            public MapGroupComponent mapGroup;
        }

        public class MapReverbZoneListenerNode : MapReverbZoneSystem.SoundListenerNode
        {
            public MapReverbZoneComponent mapReverbZone;
        }

        [Not(typeof(MapReverbZoneComponent))]
        public class NonMapReverbZoneListenerNode : MapReverbZoneSystem.SoundListenerNode
        {
        }

        public class SoundListenerNode : Node
        {
            public SoundListenerComponent soundListener;
        }
    }
}

