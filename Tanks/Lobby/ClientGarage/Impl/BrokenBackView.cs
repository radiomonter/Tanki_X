namespace Tanks.Lobby.ClientGarage.Impl
{
    using System;
    using UnityEngine;

    public class BrokenBackView : MonoBehaviour
    {
        [SerializeField]
        private float animationTime = 1f;
        [SerializeField]
        private AnimationCurve curve;
        [SerializeField]
        private RectTransform[] parts;
        private BrokenBackPartsData[] partsData;
        private float timer;

        public void BreakBack()
        {
            this.Init();
            for (int i = 0; i < this.parts.Length; i++)
            {
                this.parts[i].offsetMin = this.partsData[i].OffsetMin;
                this.parts[i].offsetMax = this.partsData[i].OffsetMax;
            }
        }

        private void Init()
        {
            if (this.partsData == null)
            {
                this.partsData = new BrokenBackPartsData[this.parts.Length];
                for (int i = 0; i < this.parts.Length; i++)
                {
                    this.partsData[i] = new BrokenBackPartsData(this.parts[i].offsetMin, this.parts[i].offsetMax);
                }
            }
        }

        private void OnEnable()
        {
            this.Init();
            this.timer = 0f;
        }

        private void Update()
        {
            this.timer += Time.deltaTime;
            float t = this.curve.Evaluate(Mathf.Clamp01(this.timer / this.animationTime));
            for (int i = 0; i < this.parts.Length; i++)
            {
                this.parts[i].offsetMin = Vector3.Lerp((Vector3) this.partsData[i].OffsetMin, (Vector3) Vector2.zero, t);
                this.parts[i].offsetMax = Vector3.Lerp((Vector3) this.partsData[i].OffsetMax, (Vector3) Vector2.zero, t);
            }
            if (this.timer >= this.animationTime)
            {
                base.enabled = false;
            }
        }

        private class BrokenBackPartsData
        {
            private Vector2 offsetMin;
            private Vector2 offsetMax;

            public BrokenBackPartsData(Vector2 offsetMin, Vector2 offsetMax)
            {
                this.offsetMin = offsetMin;
                this.offsetMax = offsetMax;
            }

            public Vector2 OffsetMin =>
                this.offsetMin;

            public Vector2 OffsetMax =>
                this.offsetMax;
        }
    }
}

