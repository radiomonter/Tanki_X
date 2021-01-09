namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class MapNativeSoundsSystem : ECSSystem
    {
        [OnEventFire]
        public void FinalizeAmbientMapSoundEffect(LobbyAmbientSoundPlayEvent evt, MapNativeSoundsListenerNode listener)
        {
            listener.mapNativeSounds.MapNativeSounds.Stop();
            listener.Entity.RemoveComponent<MapNativeSoundsComponent>();
        }

        [OnEventFire]
        public void InitAmbientMapSoundEffect(MapAmbientSoundPlayEvent evt, NonMapNativeSoundsListenerNode listener, [JoinAll] MapNode map, [JoinByMap] MapEffectNode mapEffect)
        {
            Transform transform = map.mapInstance.SceneRoot.transform;
            MapNativeSoundsBehaviour mapNativeSounds = Object.Instantiate<MapNativeSoundsBehaviour>(mapEffect.mapNativeSoundsAsset.Asset);
            Object.DontDestroyOnLoad(mapNativeSounds.gameObject);
            Transform transform2 = mapNativeSounds.transform;
            transform2.position = transform.position;
            transform2.rotation = transform.rotation;
            transform2.localScale = Vector3.one;
            listener.Entity.AddComponent(new MapNativeSoundsComponent(mapNativeSounds));
            mapNativeSounds.Play();
        }

        public class MapEffectNode : Node
        {
            public MapNativeSoundsAssetComponent mapNativeSoundsAsset;
            public MapGroupComponent mapGroup;
        }

        public class MapNativeSoundsListenerNode : MapNativeSoundsSystem.SoundListenerNode
        {
            public MapNativeSoundsComponent mapNativeSounds;
        }

        public class MapNode : Node
        {
            public MapInstanceComponent mapInstance;
            public MapGroupComponent mapGroup;
        }

        [Not(typeof(MapNativeSoundsComponent))]
        public class NonMapNativeSoundsListenerNode : MapNativeSoundsSystem.SoundListenerNode
        {
        }

        public class SoundListenerNode : Node
        {
            public SoundListenerComponent soundListener;
        }
    }
}

