namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class UpdateRankEffectFadeInOutShaderFloat : MonoBehaviour
    {
        public string PropertyName = "_CutOut";
        public float MaxFloat = 1f;
        public float StartDelay;
        public float FadeInSpeed;
        public float FadeOutDelay;
        public float FadeOutSpeed;
        public bool FadeOutAfterCollision;
        public bool UseHideStatus;
        private Material OwnMaterial;
        private Material mat;
        private float oldFloat;
        private float currentFloat;
        private bool canStart;
        private bool canStartFadeOut;
        private bool fadeInComplited;
        private bool fadeOutComplited;
        private bool previousFrameVisibleStatus;
        private bool isCollisionEnter;
        private bool isStartDelay;
        private bool isIn;
        private bool isOut;
        private UpdateRankEffectSettings effectSettings;
        private bool isInitialized;

        private void FadeIn()
        {
            this.currentFloat = this.oldFloat + ((Time.deltaTime / this.FadeInSpeed) * this.MaxFloat);
            if (this.currentFloat >= this.MaxFloat)
            {
                this.fadeInComplited = true;
                this.currentFloat = this.MaxFloat;
                base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
            }
            this.mat.SetFloat(this.PropertyName, this.currentFloat);
            this.oldFloat = this.currentFloat;
        }

        private void FadeOut()
        {
            this.currentFloat = this.oldFloat - ((Time.deltaTime / this.FadeOutSpeed) * this.MaxFloat);
            if (this.currentFloat <= 0f)
            {
                this.currentFloat = 0f;
                this.fadeOutComplited = true;
            }
            this.mat.SetFloat(this.PropertyName, this.currentFloat);
            this.oldFloat = this.currentFloat;
        }

        private void GetEffectSettingsComponent(Transform tr)
        {
            Transform parent = tr.parent;
            if (parent != null)
            {
                this.effectSettings = parent.GetComponentInChildren<UpdateRankEffectSettings>();
                if (this.effectSettings == null)
                {
                    this.GetEffectSettingsComponent(parent.transform);
                }
            }
        }

        private void InitDefaultVariables()
        {
            this.fadeInComplited = false;
            this.fadeOutComplited = false;
            this.canStartFadeOut = false;
            this.canStart = false;
            this.isCollisionEnter = false;
            this.oldFloat = 0f;
            this.currentFloat = this.MaxFloat;
            if (this.isIn)
            {
                this.currentFloat = 0f;
            }
            this.mat.SetFloat(this.PropertyName, this.currentFloat);
            if (this.isStartDelay)
            {
                base.Invoke("SetupStartDelay", this.StartDelay);
            }
            else
            {
                this.canStart = true;
            }
            if (!this.isIn)
            {
                if (!this.FadeOutAfterCollision)
                {
                    base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
                }
                this.oldFloat = this.MaxFloat;
            }
        }

        private void InitMaterial()
        {
            if (!this.isInitialized)
            {
                if (base.GetComponent<Renderer>() != null)
                {
                    this.mat = base.GetComponent<Renderer>().material;
                }
                else
                {
                    LineRenderer component = base.GetComponent<LineRenderer>();
                    if (component != null)
                    {
                        this.mat = component.material;
                    }
                    else
                    {
                        Projector projector = base.GetComponent<Projector>();
                        if (projector != null)
                        {
                            if (!projector.material.name.EndsWith("(Instance)"))
                            {
                                Material material = new Material(projector.material) {
                                    name = projector.material.name + " (Instance)"
                                };
                                projector.material = material;
                            }
                            this.mat = projector.material;
                        }
                    }
                }
                if (this.mat != null)
                {
                    this.isStartDelay = this.StartDelay > 0.001f;
                    this.isIn = this.FadeInSpeed > 0.001f;
                    this.isOut = this.FadeOutSpeed > 0.001f;
                    this.InitDefaultVariables();
                    this.isInitialized = true;
                }
            }
        }

        private void OnEnable()
        {
            if (this.isInitialized)
            {
                this.InitDefaultVariables();
            }
        }

        private void prefabSettings_CollisionEnter(object sender, UpdateRankCollisionInfo e)
        {
            this.isCollisionEnter = true;
            if (!this.isIn && this.FadeOutAfterCollision)
            {
                base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
            }
        }

        private void SetupFadeOutDelay()
        {
            this.canStartFadeOut = true;
        }

        private void SetupStartDelay()
        {
            this.canStart = true;
        }

        private void Start()
        {
            this.GetEffectSettingsComponent(base.transform);
            if (this.effectSettings != null)
            {
                this.effectSettings.CollisionEnter += new EventHandler<UpdateRankCollisionInfo>(this.prefabSettings_CollisionEnter);
            }
            this.InitMaterial();
        }

        private void Update()
        {
            if (this.canStart)
            {
                if ((this.effectSettings != null) && this.UseHideStatus)
                {
                    if (!this.effectSettings.IsVisible && this.fadeInComplited)
                    {
                        this.fadeInComplited = false;
                    }
                    if (this.effectSettings.IsVisible && this.fadeOutComplited)
                    {
                        this.fadeOutComplited = false;
                    }
                }
                if (this.UseHideStatus)
                {
                    if (this.isIn && ((this.effectSettings != null) && (this.effectSettings.IsVisible && !this.fadeInComplited)))
                    {
                        this.FadeIn();
                    }
                    if (this.isOut && ((this.effectSettings != null) && (!this.effectSettings.IsVisible && !this.fadeOutComplited)))
                    {
                        this.FadeOut();
                    }
                }
                else if (!this.FadeOutAfterCollision)
                {
                    if (this.isIn && !this.fadeInComplited)
                    {
                        this.FadeIn();
                    }
                    if (this.isOut && (this.canStartFadeOut && !this.fadeOutComplited))
                    {
                        this.FadeOut();
                    }
                }
                else
                {
                    if (this.isIn && !this.fadeInComplited)
                    {
                        this.FadeIn();
                    }
                    if (this.isOut && (this.isCollisionEnter && (this.canStartFadeOut && !this.fadeOutComplited)))
                    {
                        this.FadeOut();
                    }
                }
            }
        }

        public void UpdateMaterial(Material instanceMaterial)
        {
            this.mat = instanceMaterial;
            this.InitMaterial();
        }
    }
}

