namespace UnityStandardAssets.Water
{
    using System;
    using UnityEngine;

    [ExecuteInEditMode]
    public class WaterTile : MonoBehaviour
    {
        public PlanarReflection reflection;
        public WaterBase waterBase;

        private void AcquireComponents()
        {
            if (!this.reflection)
            {
                this.reflection = !base.transform.parent ? base.transform.GetComponent<PlanarReflection>() : base.transform.parent.GetComponent<PlanarReflection>();
            }
            if (!this.waterBase)
            {
                this.waterBase = !base.transform.parent ? base.transform.GetComponent<WaterBase>() : base.transform.parent.GetComponent<WaterBase>();
            }
        }

        public void OnWillRenderObject()
        {
            if (this.reflection)
            {
                this.reflection.WaterTileBeingRendered(base.transform, Camera.current);
            }
            if (this.waterBase)
            {
                this.waterBase.WaterTileBeingRendered(base.transform, Camera.current);
            }
        }

        public void Start()
        {
            this.AcquireComponents();
        }
    }
}

