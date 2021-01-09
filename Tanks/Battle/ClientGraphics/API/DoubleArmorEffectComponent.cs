namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    [RequireComponent(typeof(Animator))]
    public class DoubleArmorEffectComponent : MonoBehaviour, Component
    {
        public GameObject effectPrefab;
        public Transform effectPoints;
        public float effectTime = 2f;
        public ArmorSoundEffectComponent soundEffect;
        [SerializeField]
        public bool changeEmission;
        public Color emissionColor;
        public Renderer renderer;
        private ParticleSystem[] effectInstances;
        private SupplyAnimationPlayer animationPlayer;
        [HideInInspector]
        public Color usualEmissionColor;
        private Material mainMaterial;

        private void Awake()
        {
            this.effectInstances = SupplyEffectUtil.InstantiateEffect(this.effectPrefab, this.effectPoints);
            this.animationPlayer = new SupplyAnimationPlayer(base.GetComponent<Animator>(), AnimationParameters.ARMOR_ACTIVE);
            this.soundEffect.Init(base.transform);
            if (this.changeEmission)
            {
                this.mainMaterial = TankMaterialsUtil.GetMainMaterial(this.renderer);
                this.usualEmissionColor = this.mainMaterial.GetColor("_EmissionColor");
            }
        }

        private void OnArmorStart()
        {
            base.StartCoroutine(this.PlayTransitionCoroutine());
            this.soundEffect.BeginEffect();
            if (this.changeEmission)
            {
                this.mainMaterial.SetColor("_EmissionColor", this.emissionColor);
            }
        }

        private void OnArmorStop()
        {
            this.soundEffect.StopEffect();
            if (this.changeEmission)
            {
                this.mainMaterial.SetColor("_EmissionColor", this.usualEmissionColor);
            }
        }

        public void Play()
        {
            this.animationPlayer.StartAnimation();
        }

        [DebuggerHidden]
        private IEnumerator PlayTransitionCoroutine() => 
            new <PlayTransitionCoroutine>c__Iterator0 { $this = this };

        public void Stop()
        {
            this.animationPlayer.StopAnimation();
        }

        [CompilerGenerated]
        private sealed class <PlayTransitionCoroutine>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
        {
            internal DoubleArmorEffectComponent $this;
            internal object $current;
            internal bool $disposing;
            internal int $PC;

            [DebuggerHidden]
            public void Dispose()
            {
                this.$disposing = true;
                this.$PC = -1;
            }

            public bool MoveNext()
            {
                uint num = (uint) this.$PC;
                this.$PC = -1;
                switch (num)
                {
                    case 0:
                        for (int i = 0; i < this.$this.effectInstances.Length; i++)
                        {
                            this.$this.effectInstances[i].Play(true);
                        }
                        this.$current = new WaitForSeconds(this.$this.effectTime);
                        if (!this.$disposing)
                        {
                            this.$PC = 1;
                        }
                        return true;

                    case 1:
                    {
                        int index = 0;
                        while (true)
                        {
                            if (index >= this.$this.effectInstances.Length)
                            {
                                this.$PC = -1;
                                break;
                            }
                            this.$this.effectInstances[index].Stop(true);
                            index++;
                        }
                        break;
                    }
                    default:
                        break;
                }
                return false;
            }

            [DebuggerHidden]
            public void Reset()
            {
                throw new NotSupportedException();
            }

            object IEnumerator<object>.Current =>
                this.$current;

            object IEnumerator.Current =>
                this.$current;
        }
    }
}

