namespace Tanks.Battle.ClientGraphics.Impl
{
    using System;
    using UnityEngine;

    public class ShaftReticleSpotBehaviour : MonoBehaviour
    {
        [SerializeField]
        private float minSpotScale = 1f;
        [SerializeField]
        private float maxSpotScale = 8f;
        [SerializeField]
        private float minSpotDistance = 50f;
        [SerializeField]
        private float maxSpotDistance = 5000f;
        private RectTransform transform;

        private float CalculateScaleByDistance(float distance, bool isInsideTankPart) => 
            !isInsideTankPart ? ((distance >= this.minSpotDistance) ? ((distance <= this.maxSpotDistance) ? (((1f - ((distance - this.minSpotDistance) / (this.maxSpotDistance - this.minSpotDistance))) * (this.maxSpotScale - this.minSpotScale)) + this.minSpotScale) : this.minSpotScale) : this.maxSpotScale) : this.maxSpotScale;

        public void Init()
        {
            this.transform = base.gameObject.GetComponent<RectTransform>();
        }

        public void SetDefaultSize()
        {
            this.transform.localScale = Vector3.one;
        }

        public void SetSizeByDistance(float distance, bool isInsideTankPart)
        {
            float x = this.CalculateScaleByDistance(distance, isInsideTankPart);
            this.transform.localScale = new Vector3(x, x, 1f);
        }
    }
}

