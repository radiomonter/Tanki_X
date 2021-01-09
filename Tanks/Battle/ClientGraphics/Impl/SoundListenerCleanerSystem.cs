namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using Tanks.Battle.ClientGraphics.API;
    using Tanks.Lobby.ClientSettings.API;
    using UnityEngine;

    public class SoundListenerCleanerSystem : ECSSystem
    {
        [OnEventFire]
        public void PrepareCleaningForCTF(PrepareDestroyCTFSoundsEvent evt, SingleNode<CTFSoundsComponent> battle, [JoinAll] SoundListenerNode listener)
        {
            Object.DestroyObject(battle.component.EffectRoot, listener.soundListenerCleaner.CTFCleanTimeSec);
        }

        [OnEventFire]
        public void PrepareCleaningForEffects(PrepareDestroyModuleEffectEvent evt, SingleNode<DroneFlySoundEffectComponent> effect, [JoinAll] SoundListenerNode listener)
        {
            float mineCleanTimeSec = listener.soundListenerCleaner.MineCleanTimeSec;
            this.PrepareCleaningForTankPart(effect.component.Sound.transform, mineCleanTimeSec);
        }

        [OnEventFire]
        public void PrepareCleaningForEffects(PrepareDestroyModuleEffectEvent evt, SingleNode<EffectActivationSoundComponent> effect, [JoinAll] SoundListenerNode listener)
        {
            float mineCleanTimeSec = listener.soundListenerCleaner.MineCleanTimeSec;
            this.PrepareCleaningForTankPart(effect.component.Sound, mineCleanTimeSec);
        }

        [OnEventFire]
        public void PrepareCleaningForEffects(PrepareDestroyModuleEffectEvent evt, SingleNode<EffectRemovingSoundComponent> effect, [JoinAll] SoundListenerNode listener)
        {
            float mineCleanTimeSec = listener.soundListenerCleaner.MineCleanTimeSec;
            this.PrepareCleaningForTankPart(effect.component.Sound, mineCleanTimeSec);
        }

        [OnEventFire]
        public void PrepareCleaningForEffects(PrepareDestroyModuleEffectEvent evt, SingleNode<WeaponShootingSoundEffectComponent> effect, [JoinAll] SoundListenerNode listener)
        {
            float mineCleanTimeSec = listener.soundListenerCleaner.MineCleanTimeSec;
            this.PrepareCleaningForTankPart(effect.component.SoundController.transform, mineCleanTimeSec);
        }

        [OnEventFire]
        public void PrepareCleaningForEffects(PrepareDestroyModuleEffectEvent evt, SingleNode<WeaponStreamHitSoundsEffectComponent> effect, [JoinAll] SoundListenerNode listener)
        {
            float mineCleanTimeSec = listener.soundListenerCleaner.MineCleanTimeSec;
            this.PrepareCleaningForTankPart(effect.component.SoundController.transform, mineCleanTimeSec);
        }

        [OnEventFire]
        public void PrepareCleaningForMines(PrepareDestroyModuleEffectEvent evt, SingleNode<SpiderMineSoundsComponent> mine, [JoinAll] SoundListenerNode listener)
        {
            float mineCleanTimeSec = listener.soundListenerCleaner.MineCleanTimeSec;
            this.PrepareCleaningForTankPart(mine.component.RunSoundController.transform, mineCleanTimeSec);
        }

        [OnEventFire]
        public void PrepareCleaningForMines(PrepareDestroyModuleEffectEvent evt, SingleNode<MineSoundsComponent> mine, [JoinAll] SoundListenerNode listener)
        {
            float mineCleanTimeSec = listener.soundListenerCleaner.MineCleanTimeSec;
            this.PrepareCleaningForTankPart(mine.component.DeactivationSound, mineCleanTimeSec);
            this.PrepareCleaningForTankPart(mine.component.ExplosionSound, mineCleanTimeSec);
            this.PrepareCleaningForTankPart(mine.component.DropGroundSound, mineCleanTimeSec);
            this.PrepareCleaningForTankPart(mine.component.DropNonGroundSound, mineCleanTimeSec);
        }

        [OnEventFire]
        public void PrepareCleaningForModuleEffectsParts(RemoveEffectEvent evt, SingleNode<EffectComponent> effect, [JoinByUnit] SingleNode<WeaponSoundRootComponent> weapon, [JoinAll] SoundListenerNode listener)
        {
            Transform tankPartTransform = weapon.component.transform;
            this.PrepareCleaningForTankPart(tankPartTransform, listener.soundListenerCleaner.TankPartCleanTimeSec);
        }

        private void PrepareCleaningForTankPart(AudioSource source, float destroyDelay)
        {
            if (source != null)
            {
                this.PrepareCleaningForTankPart(source.transform, Mathf.Max(destroyDelay, source.clip.length + 0.5f));
            }
        }

        private void PrepareCleaningForTankPart(Transform tankPartTransform, float destroyDelay)
        {
            tankPartTransform.SetParent(null, true);
            Object.DestroyObject(tankPartTransform.gameObject, destroyDelay);
        }

        [OnEventFire]
        public void PrepareCleaningForTankParts(NodeRemoveEvent evt, WeaponNode weapon, [JoinByTank] TankNode tank, [JoinAll] SoundListenerNode listener)
        {
            Transform tankPartTransform = weapon.weaponSoundRoot.transform;
            Transform soundRootTransform = tank.tankSoundRoot.SoundRootTransform;
            float tankPartCleanTimeSec = listener.soundListenerCleaner.TankPartCleanTimeSec;
            this.PrepareCleaningForTankPart(soundRootTransform, tankPartCleanTimeSec);
            this.PrepareCleaningForTankPart(tankPartTransform, tankPartCleanTimeSec);
        }

        public class SoundListenerNode : Node
        {
            public SoundListenerComponent soundListener;
            public SoundListenerCleanerComponent soundListenerCleaner;
        }

        public class TankNode : Node
        {
            public TankSoundRootComponent tankSoundRoot;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankGroupComponent tankGroup;
        }

        public class WeaponNode : Node
        {
            public WeaponSoundRootComponent weaponSoundRoot;
            public TankGroupComponent tankGroup;
        }
    }
}

