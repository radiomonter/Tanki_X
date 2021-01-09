namespace Tanks.Battle.ClientGraphics.Impl
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class EmergencyProtectionTankShaderEffectComponent : BehaviourComponent
    {
        private const string ROTATION_ANGLE_KEY = "_RepairRotationAngle";
        private const string EMERGENCY_PROTECTION_COLOR = "_EmergencyProtectionColor";
        private const string EMERGENCY_PROTECTION_FRONT_COEFF = "_EmergencyProtectionFrontCoeff";
        private const string EMERGENCY_PROTECTION_NOISE_TEX = "_EmergencyProtectionNoise";
        [SerializeField]
        private Color emergencyProtectionColor;
        [SerializeField]
        private float duration = 1f;
        [SerializeField]
        private float waveAnimationTime = 1f;
        [SerializeField]
        private AnimationCurve straightStepCurve;
        [SerializeField]
        private AnimationCurve reverseStepCurve;
        [SerializeField]
        private Vector2 noiseTextureTiling = new Vector2(5f, 5f);
        [SerializeField]
        private Texture2D noiseTexture;
        [SerializeField]
        private ParticleSystem waveEffect;
        [SerializeField]
        private bool useWaveEffect;
        [SerializeField]
        private float delayWithWaveEffect = 0.25f;
        private float phaseTimer;
        private int waveCount;
        private int waveIterator;
        private int waveTimeoutIterator;
        private float waveTimeoutLength;
        private AnimationStates state;
        private bool frontDirection;
        private HealingGraphicEffectInputs tankEffectInput;
        private WeaponHealingGraphicEffectInputs weaponEffectInputs;

        public void InitEffect(HealingGraphicEffectInputs tankEffectInput, WeaponHealingGraphicEffectInputs weaponEffectInputs)
        {
            base.enabled = false;
            this.tankEffectInput = tankEffectInput;
            this.weaponEffectInputs = weaponEffectInputs;
            this.InitTankPartInputs(tankEffectInput);
            this.InitTankPartInputs(weaponEffectInputs);
            this.phaseTimer = 0f;
        }

        private void InitTankPartInputs(HealingGraphicEffectInputs inputs)
        {
            Material[] materials = inputs.Renderer.materials;
            int length = materials.Length;
            for (int i = 0; i < length; i++)
            {
                Material material = materials[i];
                material.SetColor("_EmergencyProtectionColor", this.emergencyProtectionColor);
                material.SetFloat("_EmergencyProtectionFrontCoeff", 0f);
                material.SetTexture("_EmergencyProtectionNoise", this.noiseTexture);
                material.SetTextureScale("_EmergencyProtectionNoise", this.noiseTextureTiling);
            }
        }

        private void ResetAnimationParameters()
        {
            this.phaseTimer = 0f;
            this.state = AnimationStates.TIMEOUT;
        }

        private void ResetAnimationWaveIterators()
        {
            this.waveIterator = this.waveCount;
            this.waveTimeoutIterator = this.waveCount + 1;
        }

        public void StartEffect(Shader shader)
        {
            this.StartEffect(shader, this.tankEffectInput);
            this.StartEffect(shader, this.weaponEffectInputs);
            this.waveCount = Mathf.FloorToInt(this.duration / this.waveAnimationTime);
            this.ResetAnimationParameters();
            this.ResetAnimationWaveIterators();
            this.waveTimeoutLength = (this.duration - (this.waveAnimationTime * this.waveCount)) / ((float) this.waveTimeoutIterator);
            this.frontDirection = true;
            base.enabled = true;
        }

        private void StartEffect(Shader shader, HealingGraphicEffectInputs inputs)
        {
            Material[] materials = inputs.Renderer.materials;
            int length = materials.Length;
            for (int i = 0; i < length; i++)
            {
                Material material = materials[i];
                material.shader = shader;
                material.SetFloat("_EmergencyProtectionFrontCoeff", 0f);
            }
        }

        public void StopEffect()
        {
            Entity[] entities = new Entity[] { this.tankEffectInput.Entity, this.weaponEffectInputs.Entity };
            ECSBehaviour.EngineService.Engine.NewEvent<StopEmergencyProtectionTankShaderEffectEvent>().AttachAll(entities).Schedule();
            base.enabled = false;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            if ((this.waveIterator + this.waveTimeoutIterator) <= 0)
            {
                this.StopEffect();
            }
            else
            {
                this.phaseTimer += Time.deltaTime;
                float time = 0f;
                if (this.state == AnimationStates.TIMEOUT)
                {
                    if ((this.phaseTimer / this.waveTimeoutLength) >= 1f)
                    {
                        this.phaseTimer = 0f;
                        this.state = AnimationStates.WAVE;
                        this.waveTimeoutIterator--;
                    }
                }
                else
                {
                    time = this.phaseTimer / this.waveAnimationTime;
                    if (time < 1f)
                    {
                        AnimationCurve curve = !this.frontDirection ? this.reverseStepCurve : this.straightStepCurve;
                        this.UpdateFrontCoeff(curve.Evaluate(time));
                    }
                    else
                    {
                        this.phaseTimer = 0f;
                        this.state = AnimationStates.TIMEOUT;
                        this.frontDirection = !this.frontDirection;
                        this.waveIterator--;
                    }
                }
            }
        }

        private void UpdateFrontCoeff(float coeff)
        {
            this.UpdateFrontCoeff(coeff, this.tankEffectInput);
            this.UpdateFrontCoeff(coeff, this.weaponEffectInputs);
        }

        private void UpdateFrontCoeff(float coeff, HealingGraphicEffectInputs inputs)
        {
            Material[] materials = inputs.Renderer.materials;
            int length = materials.Length;
            for (int i = 0; i < length; i++)
            {
                materials[i].SetFloat("_EmergencyProtectionFrontCoeff", coeff);
            }
        }

        private void UpdateFrontCoeff(float coeff, WeaponHealingGraphicEffectInputs inputs)
        {
            Material[] materials = inputs.Renderer.materials;
            int length = materials.Length;
            float num2 = -0.01745329f * inputs.RotationTransform.localEulerAngles.y;
            for (int i = 0; i < length; i++)
            {
                Material material = materials[i];
                material.SetFloat("_RepairRotationAngle", num2);
                material.SetFloat("_EmergencyProtectionFrontCoeff", coeff);
            }
        }

        public float DelayWithWaveEffect =>
            this.delayWithWaveEffect;

        public float Duration =>
            this.duration;

        public ParticleSystem WaveEffect =>
            this.waveEffect;

        public bool UseWaveEffect =>
            this.useWaveEffect;

        private enum AnimationStates
        {
            WAVE,
            TIMEOUT
        }
    }
}

