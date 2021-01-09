namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class RFX4_ScaleCurves : MonoBehaviour
    {
        public AnimationCurve FloatCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        public float GraphTimeMultiplier = 1f;
        public float GraphIntensityMultiplier = 1f;
        public bool IsLoop;
        private bool canUpdate;
        private float startTime;
        private Transform t;
        private int nameId;
        private Projector proj;
        private Vector3 startScale;

        private void Awake()
        {
            this.t = base.GetComponent<Transform>();
            this.startScale = this.t.localScale;
            this.t.localScale = Vector3.zero;
            this.proj = base.GetComponent<Projector>();
        }

        private void OnEnable()
        {
            this.startTime = Time.time;
            this.canUpdate = true;
            this.t.localScale = Vector3.zero;
        }

        private void Update()
        {
            float num = Time.time - this.startTime;
            if (this.canUpdate)
            {
                float num2 = this.FloatCurve.Evaluate(num / this.GraphTimeMultiplier) * this.GraphIntensityMultiplier;
                this.t.localScale = (Vector3) (num2 * this.startScale);
                if (this.proj != null)
                {
                    this.proj.orthographicSize = num2;
                }
            }
            if (num >= this.GraphTimeMultiplier)
            {
                if (this.IsLoop)
                {
                    this.startTime = Time.time;
                }
                else
                {
                    this.canUpdate = false;
                }
            }
        }
    }
}

