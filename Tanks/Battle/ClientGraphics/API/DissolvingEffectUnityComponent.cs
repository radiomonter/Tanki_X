namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Collections.Generic;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class DissolvingEffectUnityComponent : BehaviourComponent
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
        private float phaseTime = 1f;
        [SerializeField]
        private float offsetPhaseTime = 0.3f;
        [SerializeField]
        private float maxDistortion = 70f;
        [SerializeField]
        private Vector2 dissolveMapScale = new Vector2(2f, 2f);
        [SerializeField]
        private List<Renderer> renderers;
        [SerializeField]
        private AnimationCurve dissolvingCurve;
        [SerializeField]
        private SoundController soundInstance;
        private int dissolveMapIntKey;
        private int dissolveCoeffIntKey;
        private int distortionCoeffIntKey;
        private List<Shader> savedShaders;
        private List<Material> materials;
        private int materialsLength;
        private Entity entity;
        private float timer;
        private bool inited;

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

        private void ConfigureMaterials(float dissolveVal, float distortionVal, Shader shader)
        {
            Texture2D textured = this.dissolveMaps[Random.Range(0, this.dissolveMaps.Length)];
            this.savedShaders = new List<Shader>(this.dissolveMaps.Length);
            for (int i = 0; i < this.materialsLength; i++)
            {
                Material material = this.materials[i];
                this.savedShaders.Add(material.shader);
                material.shader = shader;
                material.SetTexture(this.dissolveMapIntKey, textured);
                material.SetTextureScale("_DissolveMap", this.dissolveMapScale);
                material.SetFloat(this.dissolveCoeffIntKey, dissolveVal);
                material.SetFloat(this.distortionCoeffIntKey, distortionVal);
            }
        }

        private void ReturnMaterials()
        {
            int num = 0;
            foreach (Material material in this.materials)
            {
                material.shader = this.savedShaders[num++];
            }
        }

        public void Start()
        {
            this.StartEffect();
        }

        public void StartEffect()
        {
            this.timer = this.phaseTime;
            this.materials = new List<Material>();
            foreach (Renderer renderer in this.renderers)
            {
                this.materials.AddRange(renderer.materials);
            }
            this.materialsLength = this.materials.Count;
            this.dissolveCoeffIntKey = Shader.PropertyToID("_DissolveCoeff");
            this.dissolveMapIntKey = Shader.PropertyToID("_DissolveMap");
            this.distortionCoeffIntKey = Shader.PropertyToID("_DistortionCoeff");
            this.ConfigureMaterials(1.01f, this.maxDistortion, this.invisibilityEffectTransitionShader);
            base.enabled = true;
            if (this.soundInstance)
            {
                this.soundInstance.SetSoundActive();
            }
            this.inited = true;
        }

        private void Update()
        {
            if (this.inited)
            {
                float num = 0f;
                float dissolveCoeff = 0f;
                this.timer -= Time.deltaTime;
                num = this.timer / this.phaseTime;
                dissolveCoeff = num;
                this.ApplyTransition(this.dissolvingCurve, dissolveCoeff, num - this.offsetPhaseTime);
                if (dissolveCoeff <= 0f)
                {
                    this.ReturnMaterials();
                    base.enabled = false;
                }
            }
        }

        public Shader InvisibilityEffectTransitionShader =>
            this.invisibilityEffectTransitionShader;
    }
}

