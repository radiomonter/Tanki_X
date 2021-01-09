namespace UnityStandardAssets.Water
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(WaterBase)), ExecuteInEditMode]
    public class SpecularLighting : MonoBehaviour
    {
        public Transform specularLight;
        private WaterBase m_WaterBase;

        public void Start()
        {
            this.m_WaterBase = (WaterBase) base.gameObject.GetComponent(typeof(WaterBase));
        }

        public void Update()
        {
            if (!this.m_WaterBase)
            {
                this.m_WaterBase = (WaterBase) base.gameObject.GetComponent(typeof(WaterBase));
            }
            if (this.specularLight && this.m_WaterBase.sharedMaterial)
            {
                this.m_WaterBase.sharedMaterial.SetVector("_WorldLightDir", this.specularLight.transform.forward);
            }
        }
    }
}

