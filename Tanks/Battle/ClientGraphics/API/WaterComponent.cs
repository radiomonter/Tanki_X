namespace Tanks.Battle.ClientGraphics.API
{
    using Platform.Kernel.ECS.ClientEntitySystem.API;
    using System;
    using UnityEngine;
    using UnityStandardAssets.Water;

    public class WaterComponent : MonoBehaviour, Component
    {
        public void DisableReflection()
        {
            this.DisableReflection(base.transform);
        }

        private void DisableReflection(Transform root)
        {
            WaterTile component = root.GetComponent<WaterTile>();
            if (component != null)
            {
                component.reflection.enabled = false;
            }
            for (int i = 0; i < root.childCount; i++)
            {
                this.DisableReflection(root.GetChild(i));
            }
        }

        private void OnEnable()
        {
            this.SetWaterRenderQueue(base.transform);
        }

        private void SetWaterRenderQueue(Transform root)
        {
            if (root.GetComponent<WaterTile>() != null)
            {
                Material[] materials = root.GetComponent<Renderer>().materials;
                for (int j = 0; j < materials.Length; j++)
                {
                    materials[j].renderQueue = 0x993;
                }
            }
            for (int i = 0; i < root.childCount; i++)
            {
                this.SetWaterRenderQueue(root.GetChild(i));
            }
        }

        public bool EdgeBlend
        {
            set => 
                base.GetComponent<WaterBase>().edgeBlend = value;
        }
    }
}

