namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class UpdateRankEffectFadeInOutScale : MonoBehaviour
    {
        public UpdateRankEffectFadeInOutStatus FadeInOutStatus;
        public float Speed = 1f;
        public float MaxScale = 2f;
        private Vector3 oldScale;
        private float time;
        private float oldSin;
        private bool updateTime = true;
        private bool canUpdate = true;
        private Transform t;
        private UpdateRankEffectSettings effectSettings;
        private bool isInitialized;
        private bool isCollisionEnter;

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

        public void InitDefaultVariables()
        {
            if (this.FadeInOutStatus == UpdateRankEffectFadeInOutStatus.OutAfterCollision)
            {
                this.t.localScale = this.oldScale;
                this.canUpdate = false;
            }
            else
            {
                this.t.localScale = Vector3.zero;
                this.canUpdate = true;
            }
            this.updateTime = true;
            this.time = 0f;
            this.oldSin = 0f;
            this.isCollisionEnter = false;
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
            this.canUpdate = true;
        }

        private void Start()
        {
            this.t = base.transform;
            this.oldScale = this.t.localScale;
            this.isInitialized = true;
            this.GetEffectSettingsComponent(base.transform);
            if (this.effectSettings != null)
            {
                this.effectSettings.CollisionEnter += new EventHandler<UpdateRankCollisionInfo>(this.prefabSettings_CollisionEnter);
            }
        }

        private void Update()
        {
            if (this.canUpdate)
            {
                float maxScale;
                if (this.updateTime)
                {
                    this.time = Time.time;
                    this.updateTime = false;
                }
                float num = Mathf.Sin((Time.time - this.time) / this.Speed);
                if (this.oldSin <= num)
                {
                    maxScale = num * this.MaxScale;
                }
                else
                {
                    this.canUpdate = false;
                    maxScale = this.MaxScale;
                }
                if (this.FadeInOutStatus == UpdateRankEffectFadeInOutStatus.In)
                {
                    this.t.localScale = (maxScale >= this.MaxScale) ? new Vector3(this.MaxScale, this.MaxScale, this.MaxScale) : new Vector3(this.oldScale.x * maxScale, this.oldScale.y * maxScale, this.oldScale.z * maxScale);
                }
                if (this.FadeInOutStatus == UpdateRankEffectFadeInOutStatus.Out)
                {
                    this.t.localScale = (maxScale <= 0f) ? Vector3.zero : new Vector3((this.MaxScale * this.oldScale.x) - (this.oldScale.x * maxScale), (this.MaxScale * this.oldScale.y) - (this.oldScale.y * maxScale), (this.MaxScale * this.oldScale.z) - (this.oldScale.z * maxScale));
                }
                if ((this.FadeInOutStatus == UpdateRankEffectFadeInOutStatus.OutAfterCollision) && this.isCollisionEnter)
                {
                    this.t.localScale = (maxScale <= 0f) ? Vector3.zero : new Vector3((this.MaxScale * this.oldScale.x) - (this.oldScale.x * maxScale), (this.MaxScale * this.oldScale.y) - (this.oldScale.y * maxScale), (this.MaxScale * this.oldScale.z) - (this.oldScale.z * maxScale));
                }
                this.oldSin = num;
            }
        }
    }
}

