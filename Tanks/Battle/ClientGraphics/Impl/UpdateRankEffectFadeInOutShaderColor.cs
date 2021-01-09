namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class UpdateRankEffectFadeInOutShaderColor : MonoBehaviour
    {
        public string ShaderColorName = "_Color";
        public float StartDelay;
        public float FadeInSpeed;
        public float FadeOutDelay;
        public float FadeOutSpeed;
        public bool UseSharedMaterial;
        public bool FadeOutAfterCollision;
        public bool UseHideStatus;
        private Material mat;
        private Color oldColor;
        private Color currentColor;
        private float oldAlpha;
        private float alpha;
        private bool canStart;
        private bool canStartFadeOut;
        private bool fadeInComplited;
        private bool fadeOutComplited;
        private bool isCollisionEnter;
        private bool isStartDelay;
        private bool isIn;
        private bool isOut;
        private UpdateRankEffectSettings effectSettings;
        private bool isInitialized;

        private void FadeIn()
        {
            this.alpha = this.oldAlpha + (Time.deltaTime / this.FadeInSpeed);
            if (this.alpha >= this.oldColor.a)
            {
                this.fadeInComplited = true;
                this.alpha = this.oldColor.a;
                base.Invoke("SetupFadeOutDelay", this.FadeOutDelay);
            }
            this.currentColor.a = this.alpha;
            this.mat.SetColor(this.ShaderColorName, this.currentColor);
            this.oldAlpha = this.alpha;
        }

        private void FadeOut()
        {
            this.alpha = this.oldAlpha - (Time.deltaTime / this.FadeOutSpeed);
            if (this.alpha <= 0f)
            {
                this.alpha = 0f;
                this.fadeOutComplited = true;
            }
            this.currentColor.a = this.alpha;
            this.mat.SetColor(this.ShaderColorName, this.currentColor);
            this.oldAlpha = this.alpha;
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
            this.isCollisionEnter = false;
            this.oldAlpha = 0f;
            this.alpha = 0f;
            this.canStart = false;
            this.currentColor = this.oldColor;
            if (this.isIn)
            {
                this.currentColor.a = 0f;
            }
            this.mat.SetColor(this.ShaderColorName, this.currentColor);
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
                this.oldAlpha = this.oldColor.a;
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
                    this.oldColor = this.mat.GetColor(this.ShaderColorName);
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

