namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class AmbientMapSoundEffectSystem : ECSSystem
    {
        [OnEventFire]
        public void FinalizeAmbientMapSoundEffect(LobbyAmbientSoundPlayEvent evt, AmbientMapSoundListenerNode listener)
        {
            listener.ambientMapSoundEffect.AmbientMapSound.Stop();
            listener.Entity.RemoveComponent<AmbientMapSoundEffectComponent>();
        }

        [OnEventFire]
        public void InitAmbientMapSoundEffect(MapAmbientSoundPlayEvent evt, NonAmbientMapSoundListenerNode listener, [JoinAll] SingleNode<AmbientMapSoundEffectMarkerComponent> mapEffect)
        {
            AmbientSoundFilter ambientMapSound = Object.Instantiate<AmbientSoundFilter>(mapEffect.component.AmbientSoundFilter);
            Transform transform = ambientMapSound.transform;
            transform.parent = listener.soundListener.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            ambientMapSound.Play(-1f);
            listener.Entity.AddComponent(new AmbientMapSoundEffectComponent(ambientMapSound));
        }

        public class AmbientMapSoundListenerNode : AmbientMapSoundEffectSystem.SoundListenerNode
        {
            public AmbientMapSoundEffectComponent ambientMapSoundEffect;
        }

        [Not(typeof(AmbientMapSoundEffectComponent))]
        public class NonAmbientMapSoundListenerNode : AmbientMapSoundEffectSystem.SoundListenerNode
        {
        }

        public class SoundListenerNode : Node
        {
            public SoundListenerComponent soundListener;
        }
    }
}

