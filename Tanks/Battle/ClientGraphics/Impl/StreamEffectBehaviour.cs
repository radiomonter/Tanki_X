namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using Tanks.Battle.ClientCore.API;
    using Tanks.Battle.ClientGraphics.API;
    using UnityEngine;

    public class StreamEffectBehaviour : MonoBehaviour
    {
        private const float STRAIGHT_DIR = 1f;
        private const float REVERSE_DIR = -1f;
        [SerializeField]
        private float range;
        [SerializeField]
        private AnimationCurve nearIntensityEasing;
        [SerializeField]
        private AnimationCurve farIntensityEasing;
        [SerializeField]
        private Light nearLight;
        [SerializeField]
        private Light farLight;
        [SerializeField]
        private float farLightDefaultSpeed = 1f;
        [SerializeField]
        private float farLightMaxSpeed = 4f;
        private Animator nearAnimator;
        private Animator farAnimator;
        private ParticleSystem particleSystem;
        private float nearIntensityTime;
        private float farIntensityTime;
        private float easingDirection;
        private Vector3 farLightStartPosition;
        private bool lightIsEnabled = true;

        public virtual void AddCollisionLayer(int layer)
        {
        }

        public virtual void ApplySettings(StreamWeaponSettingsComponent streamWeaponSettings)
        {
            if (!streamWeaponSettings.LightIsEnabled)
            {
                this.lightIsEnabled = false;
                this.farLight.enabled = false;
                this.nearLight.enabled = false;
                base.enabled = false;
            }
        }

        public void Awake()
        {
            this.particleSystem = base.GetComponent<ParticleSystem>();
            this.nearAnimator = this.nearLight.GetComponent<Animator>();
            this.farAnimator = this.farLight.GetComponent<Animator>();
            this.farLightStartPosition = this.farLight.transform.localPosition;
            this.Stop();
        }

        private void LateUpdate()
        {
            float deltaTime = Time.deltaTime;
            float epsilon = float.Epsilon;
            this.nearIntensityTime = Mathf.Clamp(this.nearIntensityTime + (this.easingDirection * deltaTime), 0f, this.nearIntensityEasing.keys[this.nearIntensityEasing.keys.Length - 1].time);
            this.farIntensityTime = Mathf.Clamp(this.farIntensityTime + (this.easingDirection * deltaTime), 0f, this.farIntensityEasing.keys[this.farIntensityEasing.keys.Length - 1].time);
            float num3 = this.nearIntensityEasing.Evaluate(this.nearIntensityTime);
            float num4 = this.farIntensityEasing.Evaluate(this.farIntensityTime);
            this.nearLight.enabled = num3 > epsilon;
            this.farLight.enabled = num4 > epsilon;
            this.nearLight.intensity *= num3;
            this.farLight.intensity *= num4;
            this.nearAnimator.enabled = this.nearLight.enabled;
            this.farAnimator.enabled = this.farLight.enabled;
            this.enabled = this.nearLight.enabled || this.farLight.enabled;
        }

        public void Play()
        {
            this.farLight.transform.localPosition = this.farLightStartPosition;
            this.particleSystem.Play(true);
            this.easingDirection = 1f;
            base.enabled = this.lightIsEnabled;
        }

        public void Stop()
        {
            base.enabled = this.lightIsEnabled;
            this.easingDirection = -1f;
            if (this.particleSystem)
            {
                this.particleSystem.Stop(true);
            }
        }

        private void Update()
        {
            RaycastHit hit;
            float b = this.range + this.farLightStartPosition.z;
            Vector3 end = base.transform.position + (base.transform.forward * b);
            float farLightDefaultSpeed = this.farLightDefaultSpeed;
            if (Physics.Linecast(base.transform.position - base.transform.forward, end, out hit, LayerMasks.VISUAL_STATIC))
            {
                farLightDefaultSpeed = this.farLightMaxSpeed;
                b = Vector3.Distance(base.transform.position, hit.point);
            }
            Vector3 localPosition = this.farLight.transform.localPosition;
            localPosition.z = Mathf.Lerp(localPosition.z, b, Time.deltaTime * farLightDefaultSpeed);
            this.farLight.transform.localPosition = localPosition;
        }

        public float Range
        {
            get => 
                this.range;
            set => 
                this.range = value;
        }
    }
}

