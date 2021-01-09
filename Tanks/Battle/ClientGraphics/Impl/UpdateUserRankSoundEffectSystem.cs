namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class UpdateUserRankSoundEffectSystem : ECSSystem
    {
        private const float DESTROY_DELAY = 0.3f;

        [OnEventFire]
        public void PlayRemoteUserRankSoundEffect(UpdateUserRankEffectEvent evt, RemoteTankNode tank, [JoinAll] SingleNode<MapInstanceComponent> map)
        {
            AudioSource source = Object.Instantiate<AudioSource>(tank.updateUserRankSoundEffectAsset.RemoteUserRankSource);
            source.transform.position = tank.tankSoundRoot.SoundRootTransform.position;
            source.transform.rotation = tank.tankSoundRoot.SoundRootTransform.rotation;
            source.transform.SetParent(map.component.SceneRoot.transform, true);
            source.Play();
            Object.DestroyObject(source.gameObject, source.clip.length + 0.3f);
        }

        [OnEventFire]
        public void PlaySelfUserRankSoundEffect(UpdateUserRankEffectEvent evt, SelfTankNode tank, [JoinAll] SingleNode<MapInstanceComponent> map)
        {
            AudioSource source = Object.Instantiate<AudioSource>(tank.updateUserRankSoundEffectAsset.SelfUserRankSource);
            Entity entity = base.CreateEntity("UpdateUserRankSoundEffect");
            entity.AddComponent(new SelfUserRankSoundEffectInstanceComponent(source));
            source.transform.SetParent(map.component.SceneRoot.transform, true);
            source.Play();
            base.NewEvent<RemoveSelfUserRankSoundEffectEvent>().Attach(entity).ScheduleDelayed(source.clip.length);
        }

        [OnEventFire]
        public void RemoveSelfUserRankSoundEffect(RemoveSelfUserRankSoundEffectEvent e, SingleNode<SelfUserRankSoundEffectInstanceComponent> effect)
        {
            if (effect.component.Source == null)
            {
                base.DeleteEntity(effect.Entity);
            }
            else
            {
                Object.DestroyObject(effect.component.Source.gameObject, 0.3f);
                base.DeleteEntity(effect.Entity);
            }
        }

        public class RemoteTankNode : UpdateUserRankSoundEffectSystem.TankNode
        {
            public RemoteTankComponent remoteTank;
        }

        public class SelfTankNode : UpdateUserRankSoundEffectSystem.TankNode
        {
            public SelfTankComponent selfTank;
        }

        public class TankNode : Node
        {
            public TankSoundRootComponent tankSoundRoot;
            public UpdateUserRankSoundEffectAssetComponent updateUserRankSoundEffectAsset;
        }
    }
}

