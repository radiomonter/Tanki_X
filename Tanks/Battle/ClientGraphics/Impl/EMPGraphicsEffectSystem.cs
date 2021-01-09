namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientCore.Impl;
    using UnityEngine;

    public class EMPGraphicsEffectSystem : ECSSystem
    {
        private const float REMOVE_TIME_OFFSET_SEC = 0.5f;
        private const float LIGHTING_TIME_OFFSET_RELATIVE_TO_WAVE = 0.8f;
        private const float INITIAL_NORMALIZED_VALUE = 0.152f;
        private const float INITIAL_CURVE_SCALAR = 50f;
        private const float EMP_HIT_REMOVE_TIMEOUT = 2f;

        [OnEventFire]
        public void PlayEMPEffect(EMPEffectReadyEvent evt, [Combine] TankNode tank)
        {
            base.NewEvent<PlayEMPHitTargetTankEvent>().Attach(tank).ScheduleDelayed(tank.empWaveGraphicsEffect.EMPWaveAsset.WaveParticleSystem.startLifetime * 0.8f);
        }

        [OnEventFire]
        public void PlayEMPEffect(PlayEMPHitTargetTankEvent e, TankNode tank)
        {
            ParticleSystem system = Object.Instantiate<ParticleSystem>(tank.empHitVisualEffect.EmpHitPrefab, tank.tankSoundRoot.SoundRootTransform.position, tank.tankSoundRoot.SoundRootTransform.rotation, tank.tankSoundRoot.SoundRootTransform.transform);
            system.shape.mesh = tank.baseRenderer.Mesh;
            Object.Destroy(system.gameObject, 2f);
        }

        [OnEventFire]
        public void PlayEMPEffect(EMPEffectReadyEvent e, EMPEffectNode effect, [JoinByTank] TankNode tank, [JoinAll] SingleNode<MapInstanceComponent> map)
        {
            EMPWaveGraphicsBehaviour behaviour = Object.Instantiate<EMPWaveGraphicsBehaviour>(tank.empWaveGraphicsEffect.EMPWaveAsset, tank.tankVisualRoot.transform.position, tank.tankVisualRoot.transform.rotation, map.component.SceneRoot.transform);
            float num2 = effect.empEffect.Radius * 2f;
            ParticleSystem waveParticleSystem = behaviour.WaveParticleSystem;
            ParticleSystem.SizeOverLifetimeModule sizeOverLifetime = waveParticleSystem.sizeOverLifetime;
            ParticleSystem.MinMaxCurve size = sizeOverLifetime.size;
            AnimationCurve curve = size.curve;
            Keyframe key = curve.keys[0];
            key.value = (num2 * 0.152f) / 50f;
            curve.MoveKey(0, key);
            size.curveMultiplier = num2;
            sizeOverLifetime.size = size;
            AudioSource waveSound = behaviour.WaveSound;
            waveSound.transform.parent = null;
            waveSound.Play();
            waveParticleSystem.Play();
            Object.Destroy(behaviour.gameObject, waveParticleSystem.startLifetime + 0.5f);
            Object.Destroy(waveSound.gameObject, waveSound.clip.length + 0.5f);
        }

        public class EMPEffectNode : Node
        {
            public EMPEffectComponent empEffect;
            public TankGroupComponent tankGroup;
        }

        public class TankNode : Node
        {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public EMPWaveGraphicsEffectComponent empWaveGraphicsEffect;
            public EMPHitVisualEffectComponent empHitVisualEffect;
            public TankVisualRootComponent tankVisualRoot;
            public BaseRendererComponent baseRenderer;
            public TankSoundRootComponent tankSoundRoot;
            public TankGroupComponent tankGroup;
        }
    }
}

