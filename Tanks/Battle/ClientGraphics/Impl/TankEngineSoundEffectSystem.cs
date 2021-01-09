namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using UnityEngine;

    public class TankEngineSoundEffectSystem : ECSSystem
    {
        private void InitTankEngineSoundEffect(InitialTankEngineSoundEffectNode tank, bool self)
        {
            GameObject obj3 = Object.Instantiate<GameObject>(tank.tankEngineSoundEffect.EnginePrefab);
            Transform soundRootTransform = tank.tankSoundRoot.SoundRootTransform;
            obj3.transform.parent = soundRootTransform;
            obj3.transform.localPosition = Vector3.zero;
            obj3.transform.localRotation = Quaternion.identity;
            HullSoundEngineController component = obj3.GetComponent<HullSoundEngineController>();
            component.Init(self);
            tank.tankEngineSoundEffect.SoundEngineController = component;
            tank.Entity.AddComponent<TankEngineSoundEffectReadyComponent>();
        }

        [OnEventFire]
        public void InitTankEngineSoundEffect(NodeAddedEvent evt, [Combine] RemoteTankNode tank, SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            this.InitTankEngineSoundEffect(tank, false);
        }

        [OnEventFire]
        public void InitTankEngineSoundEffect(NodeAddedEvent evt, [Combine] SelfTankNode tank, SingleNode<SoundListenerBattleStateComponent> soundListener)
        {
            this.InitTankEngineSoundEffect(tank, true);
        }

        [OnEventFire]
        public void StartEngine(NodeAddedEvent evt, TankEngineSoundEffectReadyNode tank)
        {
            tank.tankEngineSoundEffect.SoundEngineController.Play();
        }

        [OnEventFire]
        public void StopEngine(NodeRemoveEvent evt, TankEngineSoundEffectReadyNode tank)
        {
            tank.tankEngineSoundEffect.SoundEngineController.Stop();
        }

        [OnEventFire]
        public void UpdateEngine(UpdateEvent evt, TankEngineSoundEffectReadyNode tank)
        {
            tank.tankEngineSoundEffect.SoundEngineController.InputRpmFactor = tank.tankEngine.Value;
        }

        public class InitialTankEngineSoundEffectNode : Node
        {
            public TankEngineComponent tankEngine;
            public TankEngineSoundEffectComponent tankEngineSoundEffect;
            public TankSoundRootComponent tankSoundRoot;
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
        }

        public class RemoteTankNode : TankEngineSoundEffectSystem.InitialTankEngineSoundEffectNode
        {
            public RemoteTankComponent remoteTank;
        }

        public class SelfTankNode : TankEngineSoundEffectSystem.InitialTankEngineSoundEffectNode
        {
            public SelfTankComponent selfTank;
        }

        public class TankEngineSoundEffectReadyNode : Node
        {
            public TankEngineComponent tankEngine;
            public TankMovableComponent tankMovable;
            public TankEngineSoundEffectComponent tankEngineSoundEffect;
            public TankEngineSoundEffectReadyComponent tankEngineSoundEffectReady;
        }
    }
}

