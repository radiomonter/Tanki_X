namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using Platform.Library.ClientUnityIntegration.API;
    using System;
    using Tanks.Battle.ClientGraphics.Impl;
    using UnityEngine;

    public class TankOutlineMapEffectComponent : BehaviourComponent
    {
        private const string GLOBAL_OUTLINE_EFFECT_NAME = "_GlobalTankOutlineEffectAlpha";
        private const string WORLD_SPACE_CENTER_NAME = "_WorldSpaceEffectCenter";
        private const string TANK_OUTLINE_EFFECT_RADIUS = "_TankOutlineEffectRadius";
        private const string GLOBAL_WORKING_COEFF = "_WorkingOutlineCoeff";
        private const string GLOBAL_TANK_OUTLINE_MULTIPLIER = "_GlobalTankOutlineScaleMultiplier";
        public static bool IS_OUTLINE_EFFECT_RUNNING;
        [SerializeField]
        private float maxEffectRadius = 3000f;
        [SerializeField]
        private float globalRadiusTime = 15f;
        [SerializeField]
        private float globalAlphaFadeInTime = 0.5f;
        [SerializeField]
        private float globalAlphaFadeOutTime = 1.5f;
        [SerializeField]
        private float minAlphaWhileBlinking = 0.25f;
        [SerializeField]
        private float enterSorkingStateSoundDelay = 1f;
        [SerializeField]
        private float workingFadeTimeOffset;
        [SerializeField]
        private float generalBlinkTime = 20f;
        [SerializeField]
        private float maxBlinkIterationTime = 2f;
        [SerializeField]
        private float pauseWhenBlinkOnMaxAlpha = -1f;
        [SerializeField]
        private float pauseWhenBlinkOnMinAlpha = 0.1f;
        [SerializeField]
        private float radarSoundInterval = 1f;
        [SerializeField]
        private float radarFadeSoundDelay = 0.5f;
        [SerializeField]
        private float workingStateFadeTime;
        [SerializeField]
        private float blinkRadius = 1.1f;
        [SerializeField]
        private SoundController radarSplashSound;
        [SerializeField]
        private SoundController radarFadeSound;
        [SerializeField]
        private SoundController deactivationRadarSound;
        [SerializeField]
        private SoundController activationRadarSound;
        private bool workingFadePause;
        private bool enterWorkingState;
        private float workingStateTimer;
        private int globalOutlineEffectAlphaID;
        private int globalWorkingCoeffID;
        private int worldSpaceEffectCenterID;
        private int tankOutlineEffectRadiusID;
        private Entity mapEffectEntity;
        private Transform effectCenterTransform;
        private float blinkTimer;
        private float pauseTimer;
        private bool blinkForward;
        private float blinkSpeed;
        private float alphaSpeedFadeOut;
        private float alphaSpeedFadeIn;
        private float radiusSpeed;
        private float globalOutlineEffectRadius;
        private float globalWorkingCoeff;
        private float globalOutlineEffectAlpha;
        private States previousState;
        private States state;

        public void ActivateEffect()
        {
            IS_OUTLINE_EFFECT_RUNNING = true;
            this.State = States.ACTIVATION;
        }

        public void DeactivateEffect()
        {
            this.State = States.DEACTIVATION;
        }

        public void InitializeOutlineEffect(Entity mapEffectEntity, Transform effectCenterTransform)
        {
            this.mapEffectEntity = mapEffectEntity;
            this.effectCenterTransform = effectCenterTransform;
            this.radiusSpeed = this.maxEffectRadius / this.globalRadiusTime;
            this.alphaSpeedFadeOut = 1f / this.globalAlphaFadeOutTime;
            this.alphaSpeedFadeIn = 1f / this.globalAlphaFadeInTime;
            this.globalWorkingCoeffID = Shader.PropertyToID("_WorkingOutlineCoeff");
            this.globalOutlineEffectAlphaID = Shader.PropertyToID("_GlobalTankOutlineEffectAlpha");
            this.worldSpaceEffectCenterID = Shader.PropertyToID("_WorldSpaceEffectCenter");
            this.tankOutlineEffectRadiusID = Shader.PropertyToID("_TankOutlineEffectRadius");
            Shader.SetGlobalFloat("_GlobalTankOutlineScaleMultiplier", this.blinkRadius);
            this.State = States.IDLE;
            base.enabled = false;
        }

        private void OnDestroy()
        {
            IS_OUTLINE_EFFECT_RUNNING = false;
            this.GlobalOutlineEffectAlpha = 0f;
        }

        private void PlayRadarSplashSound()
        {
            this.StopRadarSounds();
            this.GlobalWorkingCoeff = 0.375f;
            this.radarSplashSound.SetSoundActive();
            this.workingFadePause = true;
            this.workingStateTimer = this.radarFadeSoundDelay;
        }

        public void RunBlinkerForEffect()
        {
            this.State = States.BLINKER;
        }

        private void StopRadarSounds()
        {
            this.radarFadeSound.StopImmediately();
            this.radarSplashSound.StopImmediately();
        }

        private void SwitchEntityState<T>() where T: Node
        {
            ECSBehaviour.EngineService.Engine.ScheduleEvent(new TankOutlineMapEffectSwitchStateEvent<T>(), this.mapEffectEntity);
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            switch (this.State)
            {
                case States.IDLE:
                    base.enabled = false;
                    break;

                case States.ACTIVATION:
                    this.GlobalOutlineEffectAlpha += this.alphaSpeedFadeIn * deltaTime;
                    this.GlobalOutlineEffectRadius += this.radiusSpeed * deltaTime;
                    if ((this.GlobalOutlineEffectAlpha >= 1f) && (this.GlobalOutlineEffectRadius >= this.maxEffectRadius))
                    {
                        this.State = States.WORKING;
                    }
                    break;

                case States.WORKING:
                    this.workingStateTimer -= deltaTime;
                    if (this.enterWorkingState)
                    {
                        if (this.workingStateTimer <= 0f)
                        {
                            this.enterWorkingState = false;
                            this.PlayRadarSplashSound();
                        }
                        return;
                    }
                    if (!this.workingFadePause)
                    {
                        if (this.workingStateTimer <= 0f)
                        {
                            this.PlayRadarSplashSound();
                            return;
                        }
                        float num3 = (this.workingStateTimer - this.workingFadeTimeOffset) / (this.workingStateFadeTime - this.workingFadeTimeOffset);
                        this.GlobalWorkingCoeff = num3;
                        break;
                    }
                    if (this.workingStateTimer > 0f)
                    {
                        this.GlobalWorkingCoeff -= (1f / this.workingStateFadeTime) * deltaTime;
                    }
                    else
                    {
                        this.workingFadePause = false;
                        this.workingStateTimer = this.workingStateFadeTime;
                        this.radarFadeSound.SetSoundActive();
                        this.GlobalWorkingCoeff = 1f;
                    }
                    return;

                case States.BLINKER:
                {
                    this.GlobalOutlineEffectRadius += this.radiusSpeed * deltaTime;
                    this.pauseTimer -= deltaTime;
                    if (this.pauseTimer > 0f)
                    {
                        return;
                    }
                    this.blinkTimer += deltaTime;
                    if (this.blinkTimer >= this.generalBlinkTime)
                    {
                        this.GlobalOutlineEffectAlpha = 1f;
                        this.State = States.DEACTIVATION;
                        return;
                    }
                    float num2 = (2f * (1f - this.minAlphaWhileBlinking)) / this.maxBlinkIterationTime;
                    if (this.blinkForward)
                    {
                        this.GlobalOutlineEffectAlpha += num2 * deltaTime;
                        if (this.GlobalOutlineEffectAlpha >= 1f)
                        {
                            this.GlobalOutlineEffectAlpha = 1f;
                            this.pauseTimer = this.pauseWhenBlinkOnMaxAlpha;
                            this.blinkForward = false;
                        }
                    }
                    else
                    {
                        this.GlobalOutlineEffectAlpha -= num2 * deltaTime;
                        if (this.GlobalOutlineEffectAlpha <= this.minAlphaWhileBlinking)
                        {
                            this.GlobalOutlineEffectAlpha = this.minAlphaWhileBlinking;
                            this.pauseTimer = this.pauseWhenBlinkOnMinAlpha;
                            this.blinkForward = true;
                        }
                    }
                    break;
                }
                case States.DEACTIVATION:
                    this.GlobalOutlineEffectAlpha -= this.alphaSpeedFadeOut * deltaTime;
                    if (this.GlobalOutlineEffectAlpha <= 0f)
                    {
                        this.State = States.IDLE;
                    }
                    break;

                default:
                    break;
            }
        }

        private void UpdateEffectTransformCenter()
        {
            Vector3 position = this.effectCenterTransform.position;
            Shader.SetGlobalVector(this.worldSpaceEffectCenterID, new Vector4(position.x, position.y, position.z, 1f));
        }

        private float GlobalOutlineEffectRadius
        {
            get => 
                this.globalOutlineEffectRadius;
            set
            {
                this.globalOutlineEffectRadius = Mathf.Clamp(value, 0f, this.maxEffectRadius);
                Shader.SetGlobalFloat(this.tankOutlineEffectRadiusID, this.globalOutlineEffectRadius);
            }
        }

        private float GlobalWorkingCoeff
        {
            get => 
                this.globalWorkingCoeff;
            set
            {
                this.globalWorkingCoeff = Mathf.Clamp01(value);
                Shader.SetGlobalFloat(this.globalWorkingCoeffID, this.globalWorkingCoeff);
            }
        }

        private float GlobalOutlineEffectAlpha
        {
            get => 
                this.globalOutlineEffectAlpha;
            set
            {
                this.globalOutlineEffectAlpha = Mathf.Clamp01(value);
                Shader.SetGlobalFloat(this.globalOutlineEffectAlphaID, this.globalOutlineEffectAlpha);
            }
        }

        private States State
        {
            get => 
                this.state;
            set
            {
                this.previousState = this.state;
                this.state = value;
                this.GlobalWorkingCoeff = 0f;
                switch (this.state)
                {
                    case States.IDLE:
                        this.GlobalOutlineEffectAlpha = 0f;
                        this.GlobalOutlineEffectRadius = 0f;
                        IS_OUTLINE_EFFECT_RUNNING = false;
                        base.enabled = false;
                        this.activationRadarSound.FadeOut();
                        this.StopRadarSounds();
                        this.SwitchEntityState<TankOutlineMapEffectStates.IdleState>();
                        break;

                    case States.ACTIVATION:
                        this.UpdateEffectTransformCenter();
                        IS_OUTLINE_EFFECT_RUNNING = true;
                        base.enabled = true;
                        this.deactivationRadarSound.FadeOut();
                        this.activationRadarSound.SetSoundActive();
                        this.StopRadarSounds();
                        this.SwitchEntityState<TankOutlineMapEffectStates.ActivationState>();
                        break;

                    case States.WORKING:
                        this.workingFadePause = false;
                        this.enterWorkingState = true;
                        this.workingStateTimer = this.enterSorkingStateSoundDelay;
                        this.GlobalOutlineEffectAlpha = 1f;
                        this.GlobalOutlineEffectRadius = this.maxEffectRadius;
                        IS_OUTLINE_EFFECT_RUNNING = true;
                        base.enabled = true;
                        this.deactivationRadarSound.FadeOut();
                        this.StopRadarSounds();
                        this.SwitchEntityState<TankOutlineMapEffectStates.WorkingState>();
                        break;

                    case States.BLINKER:
                        if ((this.previousState == States.ACTIVATION) && (this.GlobalOutlineEffectAlpha < 1f))
                        {
                            this.DeactivateEffect();
                            return;
                        }
                        IS_OUTLINE_EFFECT_RUNNING = true;
                        this.blinkTimer = 0f;
                        this.pauseTimer = -1f;
                        this.blinkForward = false;
                        base.enabled = true;
                        this.activationRadarSound.FadeOut();
                        this.deactivationRadarSound.SetSoundActive();
                        this.StopRadarSounds();
                        this.SwitchEntityState<TankOutlineMapEffectStates.BlinkerState>();
                        break;

                    case States.DEACTIVATION:
                        IS_OUTLINE_EFFECT_RUNNING = true;
                        base.enabled = true;
                        this.activationRadarSound.FadeOut();
                        this.deactivationRadarSound.SetSoundActive();
                        this.StopRadarSounds();
                        this.SwitchEntityState<TankOutlineMapEffectStates.DeactivationState>();
                        break;

                    default:
                        break;
                }
            }
        }

        private enum States
        {
            IDLE,
            ACTIVATION,
            WORKING,
            BLINKER,
            DEACTIVATION
        }
    }
}

