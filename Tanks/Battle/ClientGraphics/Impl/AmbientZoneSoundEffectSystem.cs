namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class AmbientZoneSoundEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void DisableZoneTransition(NodeRemoveEvent evt, SingleNode<MapInstanceComponent> map, [JoinAll] AmbientLevelSoundListenerNode listener)
        {
            listener.ambientZoneSoundEffect.AmbientZoneSoundEffect.DisableZoneTransition();
        }

        [OnEventFire]
        public void FinalizeAmbientLevelSoundEffect(LobbyAmbientSoundPlayEvent evt, AmbientLevelSoundListenerNode listener)
        {
            listener.ambientZoneSoundEffect.AmbientZoneSoundEffect.Stop();
            listener.Entity.RemoveComponent<AmbientZoneSoundEffectComponent>();
        }

        [OnEventFire]
        public void InitAmbientLevelSoundEffect(MapAmbientSoundPlayEvent evt, NonAmbientLevelSoundListenerNode listener, [JoinAll] SingleNode<AmbientZoneSoundEffectMarkerComponent> mapEffect)
        {
            AmbientZoneSoundEffect ambientZoneSoundEffect = Object.Instantiate<AmbientZoneSoundEffect>(mapEffect.component.AmbientZoneSoundEffect);
            Transform transform = ambientZoneSoundEffect.transform;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            ambientZoneSoundEffect.Play(listener.soundListener.transform);
            listener.Entity.AddComponent(new AmbientZoneSoundEffectComponent(ambientZoneSoundEffect));
        }

        public class AmbientLevelSoundListenerNode : AmbientZoneSoundEffectSystem.SoundListenerNode
        {
            public AmbientZoneSoundEffectComponent ambientZoneSoundEffect;
        }

        [Not(typeof(AmbientZoneSoundEffectComponent))]
        public class NonAmbientLevelSoundListenerNode : AmbientZoneSoundEffectSystem.SoundListenerNode
        {
        }

        public class SoundListenerNode : Node
        {
            public SoundListenerComponent soundListener;
        }
    }
}

