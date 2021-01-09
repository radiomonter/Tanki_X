namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class UpdateRankEffectRotateAround : MonoBehaviour
    {
        public float Speed = 1f;
        public float LifeTime = 1f;
        public float TimeDelay;
        public float SpeedFadeInTime;
        public bool UseCollision;
        public UpdateRankEffectSettings EffectSettings;
        private bool canUpdate;
        private float currentSpeedFadeIn;
        private float allTime;

        private void ChangeUpdate()
        {
            this.canUpdate = true;
        }

        private void EffectSettings_CollisionEnter(object sender, UpdateRankCollisionInfo e)
        {
            this.canUpdate = false;
        }

        private void OnEnable()
        {
            this.canUpdate = true;
            this.allTime = 0f;
        }

        private void Start()
        {
            if (this.UseCollision)
            {
                this.EffectSettings.CollisionEnter += new EventHandler<UpdateRankCollisionInfo>(this.EffectSettings_CollisionEnter);
            }
            if (this.TimeDelay > 0f)
            {
                base.Invoke("ChangeUpdate", this.TimeDelay);
            }
            else
            {
                this.canUpdate = true;
            }
        }

        private void Update()
        {
            if (this.canUpdate)
            {
                this.allTime += Time.deltaTime;
                if ((this.allTime < this.LifeTime) || (this.LifeTime <= 0.0001f))
                {
                    this.currentSpeedFadeIn = (this.SpeedFadeInTime <= 0.001f) ? this.Speed : ((this.currentSpeedFadeIn >= this.Speed) ? this.Speed : (this.currentSpeedFadeIn + ((Time.deltaTime / this.SpeedFadeInTime) * this.Speed)));
                    base.transform.Rotate((Vector3.forward * Time.deltaTime) * this.currentSpeedFadeIn);
                }
            }
        }
    }
}

