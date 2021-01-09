namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class TankInvisibilityEffectUnityComponent : BehaviourComponent
    {
        private const float MAX_DISSOLVE_VALUE = 1.01f;
        private const string DISSOLVE_MAP_STRING_KEY = "_DissolveMap";
        private const string DISSOLVE_COEFF_STRING_KEY = "_DissolveCoeff";
        private const string DISTORTION_COEFF_STRING_KEY = "_DistortionCoeff";
        [SerializeField]
        private Texture2D[] dissolveMaps;
        [SerializeField]
        private Shader invisibilityEffectTransitionShader;
        [SerializeField]
        private Shader invisibilityEffectShader;
        [SerializeField]
        private float phaseTime = 1f;
        [SerializeField]
        private float offsetPhaseTime = 0.3f;
        [SerializeField]
        private float maxDistortion = 70f;
        [SerializeField]
        private Vector2 dissolveMapScale = new Vector2(2f, 2f);
        [SerializeField]
        private AnimationCurve dissolvingFrontCurve;
        [SerializeField]
        private AnimationCurve dissolvingBackCurve;
        [SerializeField]
        private SoundController activationSoundInstance;
        [SerializeField]
        private SoundController deactivationSoundInstance;
        private int dissolveMapIntKey;
        private int dissolveCoeffIntKey;
        private int distortionCoeffIntKey;
        private List<Material> materials;
        private int materialsLength;
        private Entity entity;
        private float timer;
        private EffectStates effectState;

        public void ActivateEffect()
        {
            this.EffectState = EffectStates.ACTIVATION;
        }

        private void ApplyTransition(AnimationCurve dissolveCurve, float dissolveCoeff, float cloackCoeff)
        {
            float num = Mathf.Lerp(0f, 1.01f, dissolveCurve.Evaluate(dissolveCoeff));
            float num2 = Mathf.Lerp(0f, this.maxDistortion, cloackCoeff);
            for (int i = 0; i < this.materialsLength; i++)
            {
                Material material = this.materials[i];
                material.SetFloat(this.dissolveCoeffIntKey, num);
                material.SetFloat(this.distortionCoeffIntKey, num2);
            }
        }

        public void ConfigureEffect(Entity entity, bool fullInvisibly, params Renderer[] renderers)
        {
            this.entity = entity;
            this.timer = 0f;
            this.materials = new List<Material>();
            foreach (Renderer renderer in renderers)
            {
                this.materials.AddRange(renderer.materials);
            }
            this.materialsLength = this.materials.Count;
            this.effectState = EffectStates.IDLE;
            this.dissolveCoeffIntKey = Shader.PropertyToID("_DissolveCoeff");
            this.dissolveMapIntKey = Shader.PropertyToID("_DissolveMap");
            this.distortionCoeffIntKey = Shader.PropertyToID("_DistortionCoeff");
            if (fullInvisibly)
            {
                this.maxDistortion = 0f;
            }
        }

        public void DeactivateEffect()
        {
            this.EffectState = EffectStates.DEACTIVATION;
        }

        public void ResetEffect()
        {
            this.EffectState = EffectStates.IDLE;
        }

        private void SetRandomDissolveTextures(float dissolveVal, float distortionVal, Shader shader)
        {
            Texture2D textured = this.dissolveMaps[Random.Range(0, this.dissolveMaps.Length)];
            for (int i = 0; i < this.materialsLength; i++)
            {
                Material material = this.materials[i];
                material.shader = shader;
                material.SetTexture(this.dissolveMapIntKey, textured);
                material.SetTextureScale("_DissolveMap", this.dissolveMapScale);
                material.SetFloat(this.dissolveCoeffIntKey, dissolveVal);
                material.SetFloat(this.distortionCoeffIntKey, distortionVal);
            }
        }

        private void SwitchEntityState<T>() where T: Node
        {
            ECSBehaviour.EngineService.Engine.ScheduleEvent(new TankInvisibilityEffectSwitchStateEvent<T>(), this.entity);
        }

        private void Update()
        {
            float num = 0f;
            float cloackCoeff = 0f;
            float dissolveCoeff = 0f;
            EffectStates effectState = this.effectState;
            if (effectState == EffectStates.ACTIVATION)
            {
                this.timer += Time.deltaTime;
                num = this.timer / this.phaseTime;
                cloackCoeff = num - this.offsetPhaseTime;
                dissolveCoeff = num;
                this.ApplyTransition(this.dissolvingFrontCurve, dissolveCoeff, cloackCoeff);
                if (cloackCoeff >= 1f)
                {
                    this.EffectState = EffectStates.WORKING;
                }
            }
            else if (effectState == EffectStates.DEACTIVATION)
            {
                this.timer -= Time.deltaTime;
                num = this.timer / this.phaseTime;
                dissolveCoeff = num;
                this.ApplyTransition(this.dissolvingBackCurve, dissolveCoeff, num - this.offsetPhaseTime);
                if (dissolveCoeff <= 0f)
                {
                    this.EffectState = EffectStates.IDLE;
                }
            }
        }

        private EffectStates EffectState
        {
            get => 
                this.effectState;
            set
            {
                EffectStates effectState = this.effectState;
                this.effectState = value;
                switch (this.effectState)
                {
                    case EffectStates.IDLE:
                        this.SwitchEntityState<TankInvisibilityEffectStates.TankInvisibilityEffectIdleState>();
                        this.timer = 0f;
                        base.enabled = false;
                        this.activationSoundInstance.FadeOut();
                        break;

                    case EffectStates.WORKING:
                        this.SwitchEntityState<TankInvisibilityEffectStates.TankInvisibilityEffectWorkingState>();
                        this.timer = this.phaseTime;
                        this.SetRandomDissolveTextures(1.01f, this.maxDistortion, this.invisibilityEffectShader);
                        base.enabled = false;
                        this.deactivationSoundInstance.FadeOut();
                        break;

                    case EffectStates.ACTIVATION:
                        this.SwitchEntityState<TankInvisibilityEffectStates.TankInvisibilityEffectActivationState>();
                        if (effectState == EffectStates.IDLE)
                        {
                            this.SetRandomDissolveTextures(0f, 0f, this.invisibilityEffectTransitionShader);
                        }
                        base.enabled = true;
                        this.activationSoundInstance.SetSoundActive();
                        this.deactivationSoundInstance.FadeOut();
                        break;

                    case EffectStates.DEACTIVATION:
                        this.SwitchEntityState<TankInvisibilityEffectStates.TankInvisibilityEffectDeactivationState>();
                        if (effectState == EffectStates.WORKING)
                        {
                            this.SetRandomDissolveTextures(1.01f, this.maxDistortion, this.invisibilityEffectTransitionShader);
                        }
                        base.enabled = true;
                        this.activationSoundInstance.FadeOut();
                        this.deactivationSoundInstance.SetSoundActive();
                        break;

                    default:
                        break;
                }
            }
        }

        public Shader InvisibilityEffectTransitionShader =>
            this.invisibilityEffectTransitionShader;

        public Shader InvisibilityEffectShader =>
            this.invisibilityEffectShader;

        private enum EffectStates
        {
            IDLE,
            WORKING,
            ACTIVATION,
            DEACTIVATION
        }
    }
}

