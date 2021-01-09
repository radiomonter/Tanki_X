namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public abstract class BaseHealingGraphicEffectComponent<T> : BehaviourComponent where T: Event, new()
    {
        protected const string FRONT_BORDER_COEFF_KEY = "_RepairFrontCoeff";
        protected const string MOVEMENT_DIRECTION_KEY = "_RepairMovementDirection";
        private const string REPAIR_TEXTURE_KEY = "_RepairTex";
        private const string ROTATION_ANGLE_KEY = "_RepairRotationAngle";
        [SerializeField]
        private float durationInLoopMode;
        [SerializeField]
        private float arbitraryDuration;
        [SerializeField]
        private float pauseInLoopMode;
        [SerializeField]
        private float waveAnimationTime;
        [SerializeField]
        private Texture2D repairTexture;
        [SerializeField]
        private AudioSource sound;
        [SerializeField]
        private AnimationCurve straightStepCurve;
        [SerializeField]
        private AnimationCurve reverseStepCurve;
        private float phaseTimer;
        private float pauseBetweenFullAnimationCycleTimer;
        private int waveCount;
        private int waveIterator;
        private int waveTimeoutIterator;
        private float waveTimeoutLength;
        private AnimationStates<T> state;
        private int currentDirection;
        private HealingGraphicEffectInputs tankInputs;
        private WeaponHealingGraphicEffectInputs weaponInputs;
        private bool isLoop;

        protected BaseHealingGraphicEffectComponent()
        {
            this.durationInLoopMode = 2f;
            this.arbitraryDuration = 2f;
            this.pauseInLoopMode = 2f;
            this.waveAnimationTime = 1f;
        }

        private float CalculateWeaponRotationAngle() => 
            -0.01745329f * this.weaponInputs.RotationTransform.localEulerAngles.y;

        public virtual void InitRepairGraphicsEffect(HealingGraphicEffectInputs tankInputs, WeaponHealingGraphicEffectInputs weaponInputs, Transform soundRoot, Transform mountPoint)
        {
            this.isLoop = false;
            this.weaponInputs = weaponInputs;
            this.tankInputs = tankInputs;
            this.InitSound(soundRoot);
        }

        private void InitSound(Transform soundRoot)
        {
            Transform transform = this.sound.transform;
            transform.parent = soundRoot;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

        private void PlaySoundEffect()
        {
            if (this.sound != null)
            {
                this.sound.Stop();
                this.sound.Play();
            }
        }

        private void ResetAnimationParameters()
        {
            this.phaseTimer = 0f;
            this.pauseBetweenFullAnimationCycleTimer = -1f;
            this.state = AnimationStates<T>.TIMEOUT;
            this.currentDirection = 1;
        }

        private void ResetAnimationWaveIterators()
        {
            this.waveIterator = this.waveCount;
            this.waveTimeoutIterator = this.waveCount + 1;
        }

        protected void SetInitialTankPartsParameters(Material mat)
        {
            mat.SetFloat("_RepairRotationAngle", 0f);
        }

        private void SetRepairTextureParameters(Material mat, HealingGraphicEffectInputs inputs)
        {
            mat.SetTexture("_RepairTex", this.repairTexture);
            mat.SetTextureScale("_RepairTex", new Vector2(inputs.TilingX, 1f));
        }

        public void StartEffect(Shader shaderWithEffect, float duration = 0f)
        {
            float num = (this.arbitraryDuration <= 0f) ? duration : this.arbitraryDuration;
            this.waveCount = Mathf.FloorToInt(num / this.waveAnimationTime);
            this.ResetAnimationParameters();
            this.ResetAnimationWaveIterators();
            this.waveTimeoutLength = (num - (this.waveAnimationTime * this.waveCount)) / ((float) this.waveTimeoutIterator);
            this.UpdateDirection(shaderWithEffect);
            this.PlaySoundEffect();
            base.enabled = true;
        }

        public void StartLoop(Shader shaderWithEffect)
        {
            this.isLoop = true;
            this.StartEffect(shaderWithEffect, this.durationInLoopMode);
        }

        public void StopEffect()
        {
            this.UpdateDirection(null);
            this.StopSoundEffect();
            base.enabled = false;
            this.isLoop = false;
            if ((this.tankInputs.Entity != null) && (this.weaponInputs.Entity != null))
            {
                Entity[] entities = new Entity[] { this.tankInputs.Entity, this.weaponInputs.Entity };
                ECSBehaviour.EngineService.Engine.NewEvent<T>().AttachAll(entities).Schedule();
            }
        }

        private void StopSoundEffect()
        {
            if (this.sound != null)
            {
                this.sound.Stop();
            }
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            if ((this.waveIterator + this.waveTimeoutIterator) <= 0)
            {
                if (!this.isLoop)
                {
                    this.StopEffect();
                }
                else
                {
                    this.ResetAnimationParameters();
                    this.ResetAnimationWaveIterators();
                    this.currentDirection = 1;
                    this.UpdateDirection(null);
                    this.pauseBetweenFullAnimationCycleTimer = this.pauseInLoopMode;
                }
            }
            else if (this.pauseBetweenFullAnimationCycleTimer > 0f)
            {
                this.pauseBetweenFullAnimationCycleTimer -= deltaTime;
                if (this.pauseBetweenFullAnimationCycleTimer <= 0f)
                {
                    this.PlaySoundEffect();
                }
            }
            else
            {
                this.phaseTimer += Time.deltaTime;
                float time = 0f;
                if (this.state == AnimationStates<T>.TIMEOUT)
                {
                    if ((this.phaseTimer / this.waveTimeoutLength) >= 1f)
                    {
                        this.phaseTimer = 0f;
                        this.state = AnimationStates<T>.WAVE;
                        this.waveTimeoutIterator--;
                    }
                }
                else
                {
                    time = this.phaseTimer / this.waveAnimationTime;
                    if (time < 1f)
                    {
                        AnimationCurve curve = (this.currentDirection <= 0) ? this.reverseStepCurve : this.straightStepCurve;
                        this.UpdateFront(curve.Evaluate(time));
                    }
                    else
                    {
                        this.phaseTimer = 0f;
                        this.state = AnimationStates<T>.TIMEOUT;
                        this.UpdateDirection(null);
                        this.waveIterator--;
                    }
                }
            }
        }

        private void UpdateDirection(Shader shaderWithEffect = null)
        {
            this.UpdateDirection(this.tankInputs, (float) this.currentDirection, shaderWithEffect);
            this.UpdateDirection(this.weaponInputs, (float) this.currentDirection, this.CalculateWeaponRotationAngle(), shaderWithEffect);
            this.currentDirection ^= 1;
        }

        private void UpdateDirection(HealingGraphicEffectInputs inputs, float dir, Shader shaderWithEffect = null)
        {
            Material[] materials = inputs.Renderer.materials;
            int length = materials.Length;
            for (int i = 0; i < length; i++)
            {
                Material mat = materials[i];
                if (shaderWithEffect != null)
                {
                    mat.shader = shaderWithEffect;
                    this.SetRepairTextureParameters(mat, inputs);
                }
                mat.SetFloat("_RepairFrontCoeff", 0f);
                mat.SetFloat("_RepairMovementDirection", dir);
            }
        }

        private void UpdateDirection(WeaponHealingGraphicEffectInputs inputs, float dir, float angle, Shader shaderWithEffect = null)
        {
            Material[] materials = inputs.Renderer.materials;
            int length = materials.Length;
            for (int i = 0; i < length; i++)
            {
                Material mat = materials[i];
                if (shaderWithEffect != null)
                {
                    mat.shader = shaderWithEffect;
                    this.SetRepairTextureParameters(mat, inputs);
                }
                mat.SetFloat("_RepairFrontCoeff", 0f);
                mat.SetFloat("_RepairMovementDirection", dir);
                mat.SetFloat("_RepairRotationAngle", angle);
            }
        }

        private void UpdateFront(float front)
        {
            this.UpdateFront(this.tankInputs, front);
            this.UpdateFront(this.weaponInputs, front, this.CalculateWeaponRotationAngle());
        }

        private void UpdateFront(HealingGraphicEffectInputs inputs, float front)
        {
            Material[] materials = inputs.Renderer.materials;
            int length = materials.Length;
            for (int i = 0; i < length; i++)
            {
                materials[i].SetFloat("_RepairFrontCoeff", front);
            }
        }

        private void UpdateFront(WeaponHealingGraphicEffectInputs inputs, float front, float angle)
        {
            Material[] materials = inputs.Renderer.materials;
            int length = materials.Length;
            for (int i = 0; i < length; i++)
            {
                Material material = materials[i];
                material.SetFloat("_RepairFrontCoeff", front);
                material.SetFloat("_RepairRotationAngle", angle);
            }
        }

        private enum AnimationStates
        {
            public const BaseHealingGraphicEffectComponent<T>.AnimationStates WAVE = BaseHealingGraphicEffectComponent<T>.AnimationStates.WAVE;,
            public const BaseHealingGraphicEffectComponent<T>.AnimationStates TIMEOUT = BaseHealingGraphicEffectComponent<T>.AnimationStates.TIMEOUT;
        }
    }
}

